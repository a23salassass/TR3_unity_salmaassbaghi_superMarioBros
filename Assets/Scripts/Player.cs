using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public DeathAnimation deathAnimation { get; private set; }

    public PlayerSpriteRender smallRenderer;
    public PlayerSpriteRender bigRenderer;

    public bool small => smallRenderer.enabled;
    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    public float moveSpeed = 5f; // Velocidad base del jugador

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
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
        // TODO: Implement shrinking
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
}
