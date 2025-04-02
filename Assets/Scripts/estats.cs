using UnityEngine;
using TMPro;

public class estats : MonoBehaviour
{
    public TMP_Text coinsText;
    public TMP_Text timeText;

    void Start()
    {
        int coins = GameManager.Instance.coins;
        float temps = Time.time - GameManager.Instance.partidaStartTime;

        coinsText.text = $"{coins}";
        timeText.text = $"{temps:F2} s";
    }
}
