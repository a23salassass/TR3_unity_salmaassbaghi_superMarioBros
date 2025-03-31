using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public TextMeshProUGUI coinText;

    public void UpdateHearts(int lives)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < lives ? fullHeart : emptyHeart;
        }
    }

    public void UpdateCoins(int coins)
    {
        coinText.text = "" + coins;
    }
}
