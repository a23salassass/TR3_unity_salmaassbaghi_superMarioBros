using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; } = 1;
    public int stage { get; private set; } = 1;
    public int lives { get; private set; } = 3;
    public int coins { get; private set; } = 0;
    
    private void Awake()
    {
        // En algún script de inicio, como GameManager o en Awake():
Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"));

        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    public void NewGame()
    {
        LogSender.SendLog("Un usuari inicia una partida");
        Debug.Log("New Game, enviado a mongo");
        lives = 3;
        coins = 0;

        LoadLevel(1, 1);
    }

    public void GameOver()
    {
        LogSender.SendLog("Un usari ha perdut la partida.");
        Debug.Log("Game Over, enviado a mongo");
SceneManager.LoadScene("TheEnd");
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void ResetLevel(float delay)
    {
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0) {
            LoadLevel(world, stage);
        } else {
            GameOver();
        }
    }

    public void AddCoin()
    {
        Player player = FindObjectOfType<Player>();
        coins++;
        LogSender.SendLog("Un jugador ha recollit una moneda.");
        Debug.Log($"Monedas: {coins}, enviado a mongo");

        if (coins == 100)
        {
            coins = 0;
            AddLife();
        }
    }

    public void AddLife()
    {
        LogSender.SendLog("Un jugador ha recollit una vida.");
        Debug.Log("Add Life, enviado a mongo");
        lives++;
    }

}
