using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Player player = other.GetComponent<Player>();
            if (player.big)
            {
                player.Shrink();
            }
            else
            {
                player.Death();
            }
        }
        else{
            Destroy(other.gameObject);
        }
    }
}
