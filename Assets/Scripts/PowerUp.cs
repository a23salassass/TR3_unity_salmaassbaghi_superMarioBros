using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    public float speedBoost = 2f; // Factor para aumentar la velocidad
    public float boostDuration = 5f; // Duración del boost en segundos

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player)) {
            Collect(player);
        }
    }

    private void Collect(Player player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:
                GameManager.Instance.AddLife(); // Sumar una vida
                player.IncreaseSpeedTemporarily(speedBoost, boostDuration); // Aumentar velocidad
                break;

            case Type.Starpower:
                // player.Starpower();
                break;
        }

        Destroy(gameObject);
    }
}
