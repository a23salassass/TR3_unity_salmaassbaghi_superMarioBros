using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Player player = other.GetComponent<Player>();
                player.Death();
            
        }
        else{
            Destroy(other.gameObject);
        }
    }
}
