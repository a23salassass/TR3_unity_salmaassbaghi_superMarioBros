using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalZone : MonoBehaviour
{
    public string nextSceneName = "LoginScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Nivel completado!");
            SceneManager.LoadScene(nextSceneName);
        }
    }
    public void irALogin()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
