using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()//LateUpdate is called after Update each frame asi nos aseguramos que actua despues de que el jugador se haya movido
    {
        Vector3 position = transform.position;
        position.x = Mathf.Max(position.x,player.position.x);//Mathf.Max returns the largest of two or more values.
        transform.position = position;
    } 
}
