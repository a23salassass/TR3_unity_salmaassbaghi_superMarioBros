using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorMessage;
    public GameObject loginCanvas;

    private string apiUrl = "http://localhost:4000/api/login"; 


    public void OnLoginButtonPressed()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            errorMessage.text = "Si us plau, omple tots els camps!";
            return;
        }

        StartCoroutine(LoginRequest(username, password));
    }

    private IEnumerator LoginRequest(string username, string password)
    {
        string jsonData = $"{{\"usuari\":\"{username}\",\"contrassenya\":\"{password}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            errorMessage.text = "Iniciant sessió...";
            errorMessage.color = Color.yellow;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Successful: " + request.downloadHandler.text);
                

    StartCoroutine(LoadSceneAsync("1-1"));

            }
            else {
                  Debug.Log("FATAL REINA");

                 errorMessage.text = "Error inciant sessió";
                errorMessage.color = Color.red;
            }
        }
    }

private IEnumerator LoadSceneAsync(string sceneName)
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
    while (!asyncLoad.isDone)
    {
        yield return null;
    }
}

    // Método para el botón de registro (si lo tienes)
    public void OnRegisterButtonPressed()
    {
        // Aquí puedes cargar la escena de registro o mostrar un panel de registro
        // SceneManager.LoadScene("RegisterScene");
    }
}