using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public PlayerHUD hud;

    public static GameManager Instance { get; private set; }

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;

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
}
