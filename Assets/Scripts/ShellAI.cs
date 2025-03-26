using UnityEngine;

public class ShellAI : MonoBehaviour
{
    public float shellSpeed = 5f;
    private Rigidbody2D rb;
    private bool isMoving = false;

private void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    rb.gravityScale = 1f; // Activar gravedad
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.down)) // Mario pisa la shell
            {
                if (!isMoving) 
                {
                    KickShell(collision.transform.position.x); // Patear la shell
                }
                else 
                {
                    Destroy(gameObject); // Si ya está en movimiento y la pisa, desaparece
                }
            }
        }
        else if (collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("Wall")) 
        {
            Bounce(); // Rebota en tuberías y paredes
        }
    }

    private void KickShell(float playerX)
    {
        isMoving = true;
        float direction = (transform.position.x < playerX) ? 1f : -1f;
        rb.linearVelocity = new Vector2(shellSpeed * direction, rb.linearVelocity.y);
    }

    private void Bounce()
    {
        shellSpeed *= -1; // Invierte la dirección
        rb.linearVelocity = new Vector2(shellSpeed, rb.linearVelocity.y);
    }
}
