using UnityEngine;
using System.Collections;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);
        spriteRenderer.enabled = true;
        float time = 0;
        float duration = 0.5f;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition + Vector3.up; // Cambiado a Vector3.up

        while (time < duration)
        {
            float t = time / duration;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}