using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    private bool shell;
    private bool movingRight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!shell && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (collision.transform.DotTest(transform, Vector2.down)) 
            {
                ShellEntering();
            } 
            else 
            {
                player.Hit();
            }
        }
    }

    private void ShellEntering()
    {
        shell = true;
        GetComponent<EntityMovement>().enabled = false;  // Desactiva el movimiento del Koopa
        GetComponent<AnimatedSprite>().enabled = false;  // Desactiva la animación del Koopa
        GetComponent<SpriteRenderer>().sprite = shellSprite; // Cambia el sprite al de la caparazón

        // En lugar de destruirlo, podrías hacer que el jugador pueda patearlo luego
        gameObject.layer = 8; // Cambia la etiqueta para identificarlo como caparazón
    }
}
