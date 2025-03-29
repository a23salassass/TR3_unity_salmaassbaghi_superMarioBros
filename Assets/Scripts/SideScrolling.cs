using UnityEngine;
using System.Collections.Generic;
public class SideScrolling : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    private void LateUpdate()
    {
        Vector3 position = transform.position;
        float minX = Mathf.Min(player1.position.x, player2.position.x);
        if (minX > position.x)
        {
            position.x = minX;
            transform.position = position;
        }
    }
}
