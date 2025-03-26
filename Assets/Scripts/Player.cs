using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    public DeathAnimation deathAnimation { get; private set; }

    public PlayerSpriteRender smallRenderer;
    public PlayerSpriteRender bigRenderer;

    public bool small => smallRenderer.enabled;
    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    public float moveSpeed = 5f; // Default
    public float coinMultiplier = 1f;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }

    private void Start()
    {
        StartCoroutine(LoadStatsFromServer("http://localhost:4000/api/personatges/1"));
    }

    private IEnumerator LoadStatsFromServer(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                PlayerStats stats = JsonUtility.FromJson<PlayerStats>(www.downloadHandler.text);
                moveSpeed = stats.moveSpeed;
                coinMultiplier = stats.coinMultiplier;
                Debug.Log($"Stats cargados desde el servidor: speed={moveSpeed}, coinX{coinMultiplier}");
            }
            else
            {
                Debug.LogError("Error cargando datos: " + www.error);
            }
        }
    }

    public void Hit()
    {
        if (big)
        {
            Shrink();
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;
        GameManager.Instance.ResetLevel(3f);
    }

    public void Shrink()
    {
        // TODO
    }

    public void IncreaseSpeedTemporarily(float multiplier, float duration)
    {
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            StartCoroutine(SpeedBoostRoutine(controller, multiplier, duration));
        }
    }

    private IEnumerator SpeedBoostRoutine(PlayerController controller, float multiplier, float duration)
    {
        float originalSpeed = controller.moveSpeed;
        controller.moveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        controller.moveSpeed = originalSpeed;
    }

    private void CoinMultiplier()
    {
        coinMultiplier = 2;
    }
}
