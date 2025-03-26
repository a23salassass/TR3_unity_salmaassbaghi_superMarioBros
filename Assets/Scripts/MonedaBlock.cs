using UnityEngine;
using System.Collections;
public class MonedaBlock : MonoBehaviour
{
   private void Start()
    {
        GameManager.Instance.AddCoin();
        StartCoroutine(Animate());
        
    }
        private IEnumerator Animate()
    {
        Vector3 restPosition = transform.localPosition;
        Vector3 animatedPosition = restPosition + Vector3.up * 2f;
        yield return Move(restPosition, animatedPosition);
        yield return Move(animatedPosition, restPosition);

        Destroy(gameObject);
    }
        private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float duration = 0.25f;
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
