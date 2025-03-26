using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class Register : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorMessage;
    
    private string apiUrl = "http://localhost:4000/api/register"; 
    private int defaultCharacterId = 1;

    public void OnRegisterButtonPressed() 
    {
        string name = nameInput.text;
        string username = usernameInput.text;
        string password = passwordInput.text;
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
        {
            errorMessage.text = "Si us plau, omple tots els camps!";
            return;
        }

        StartCoroutine(RegisterRequest(name, username, password, defaultCharacterId));
    }

    private IEnumerator RegisterRequest(string name, string username, string password, int characterId)
    {
        string jsonData = $@"{{
            ""nom"": ""{name}"",
            ""usuari"": ""{username}"",
            ""contrassenya"": ""{password}"",
            ""id_personatge"": {characterId}
        }}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            errorMessage.text = "Registrant...";
            errorMessage.color = Color.yellow;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Registre exitós: " + request.downloadHandler.text);
                errorMessage.text = "Registre completat!";
                errorMessage.color = Color.green;
                
                // Esperar 2 segundos antes de cambiar de escena
                yield return new WaitForSeconds(2);
                StartCoroutine(LoadSceneAsync("1-1"));
            }
            else 
            {
                Debug.LogError("Error en el registre: " + request.error);
                
                    errorMessage.text = "ERROR EN EL REGISTRE";
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

    public void OnBackToLoginPressed()
    {
        SceneManager.LoadScene("LoginScene");
    }
}