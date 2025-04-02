using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
public class GameManager : MonoBehaviour
{
    public PlayerHUD hud;

    public static GameManager Instance { get; private set; }

    public int jugador1Id;
    public int jugador2Id;

    public float partidaStartTime;
    public int coins = 0;

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"));

        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        partidaStartTime = Time.time;
        TryFindHUD();
        UpdateHUD();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void NewGame()
    {
        LogSender.SendLog("Un usuari inicia una partida");
        Debug.Log("New Game, enviado a mongo");
        lives = 3;
        coins = 0;

        UpdateHUD();

        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        float temps = Time.time - partidaStartTime;
        StartCoroutine(SavePartida(jugador1Id, coins, temps));
        StartCoroutine(SavePartida(jugador2Id, coins, temps));

        LogSender.SendLog("Un usuari ha perdut la partida.");
        Debug.Log("Game Over, enviado a mongo");
        SceneManager.LoadScene("TheEnd");
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        SceneManager.LoadScene($"{world}-{stage}");

        // Volver a buscar el HUD tras cargar escena
        StartCoroutine(FindHUDNextFrame());
    }

    private IEnumerator FindHUDNextFrame()
    {
        yield return null; // espera 1 frame tras la carga
        TryFindHUD();
        UpdateHUD();
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;
        UpdateHUD();

        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }

    public void AddCoin()
    {
        coins++;
        LogSender.SendLog("Un jugador ha recollit una moneda.");
        Debug.Log($"Monedas: {coins}, enviado a mongo");

        UpdateHUD();

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        lives++;
        LogSender.SendLog("Un jugador ha recollit una vida.");
        Debug.Log("Add Life, enviado a mongo");
        UpdateHUD();
    }

    // 🔄 Centraliza la lógica de actualizar HUD
    private void UpdateHUD()
    {
        TryFindHUD();
        if (hud != null)
        {
            hud.UpdateHearts(lives);
            hud.UpdateCoins(coins);
        }
    }

    // 🧠 Busca el HUD si aún no se ha asignado
    private void TryFindHUD()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<PlayerHUD>();
            if (hud == null)
            {
                Debug.LogWarning("⚠️ HUD no encontrado en la escena.");
            }
        }
    }
private IEnumerator SavePartida(int jugadorId, int puntuacio, float temps)
{
    Debug.Log($"Guardando partida para jugador {jugadorId} con puntuación {puntuacio} y tiempo {temps}");
    PartidaData data = new PartidaData
    {
        JugadorId = jugadorId,
        puntuacio = puntuacio,
        temps = temps
    };

    string jsonData = JsonUtility.ToJson(data);
    Debug.Log("📤 Enviant JSON: " + jsonData);

    using (UnityWebRequest request = new UnityWebRequest("http://supermariobros.dam.inspedralbes.cat:25670/api/partides", "POST"))
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ Partida guardada per jugador {jugadorId}");
        }
        else
        {
            Debug.LogError($"❌ Error guardant la partida: {request.responseCode} - {request.downloadHandler.text}");
        }
    }
}
[System.Serializable]
public class PartidaData
{
    public int JugadorId;
    public int puntuacio;
    public float temps;
}

}
