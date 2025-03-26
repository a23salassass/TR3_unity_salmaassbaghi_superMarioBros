using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private float inputAxis;
    private bool isSquashed = false;

    public bool running => Mathf.Abs(rb.linearVelocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool aplastado = false;
    public float moveSpeed = 1.5f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isSquashed)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pipe"))
        {
            moveSpeed *= -1;
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            Flip();
        }
    }

    private void Flip()
    {
        transform.rotation = Quaternion.Euler(0f, moveSpeed > 0 ? 0f : 180f, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.down))
            {
                Aplastar(); // Llamamos a la función para aplastar al Goomba
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 5f); // Rebote de Mario
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    private void Aplastar()
    {
        aplastado = true;
        isSquashed = true;
        moveSpeed = 0; // Detiene el movimiento
        rb.linearVelocity = Vector2.zero; // Evita que se desplace
        rb.simulated = false; // Desactiva las físicas para evitar bugs

        Debug.Log("Goomba aplastado");
        Destroy(gameObject, 0.5f); // Destruye al Goomba después de 0.5 segundos
    }
}
