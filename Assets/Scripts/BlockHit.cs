using UnityEngine;
using System.Collections;
public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public int maxHits = -1;
    public Sprite emptyBlock;
    private bool animating ; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && collision.gameObject.CompareTag("Player"))
        {
if (collision.transform.DotTest(transform, Vector2.up))
            {
Hit();
            }
        }
    }

private void Hit()
{
    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
    maxHits--;

    if (maxHits == 0)
    {
        spriteRenderer.sprite = emptyBlock;
    }

    // Solo instanc
    // iar el objeto si aún quedan golpes disponibles
    if (item != null && maxHits >= 0)
    {
        Instantiate(item, transform.position, Quaternion.identity);
    }

    StartCoroutine(Animate());
}


    private IEnumerator Animate()
    {
        animating = true;
        Vector3 restPosition = transform.localPosition;
        Vector3 animatedPosition = restPosition + Vector3.up * 0.5f;
        yield return Move(restPosition, animatedPosition);
        yield return Move(animatedPosition, restPosition);
        animating = false;

    }
    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float duration = 0.125f;
        float time = 0;
        while (time < duration)
        {
            float t = time / duration;
            transform.localPosition = Vector3.Lerp(from, to, t);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = to;
        
    }
}
