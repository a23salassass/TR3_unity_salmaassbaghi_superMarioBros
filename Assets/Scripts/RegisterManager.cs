using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class Register : MonoBehaviour
{
    public TMP_InputField nameInput1;
    public TMP_InputField usernameInput1;
    public TMP_InputField passwordInput1;
    public TMP_InputField nameInput2;
    public TMP_InputField usernameInput2;
    public TMP_InputField passwordInput2;
    public TMP_Text errorMessage;
    
    private string apiUrl = "http://localhost:4000/api/register"; 
    private int defaultCharacterId = 1;

    public void OnLoginBothPlayers()
    {
        Debug.Log("Register both players clicked");
        StartCoroutine(LoginBoth());
    }

    private IEnumerator LoginBoth()
    {
        Debug.Log("Starting Register for both players");
        if (usernameInput1 == null || passwordInput1 == null || usernameInput2 == null || passwordInput2 == null)
{
    Debug.LogError("Algún campo de input no está asignado en el inspector.");
    errorMessage.text = "Falta assignar camps al codi.";
    yield break;
}
        Debug.Log("Starting Register for both players");

        string json1 = $"{{\"nom\":\"{nameInput1.text}\",\"usuari\":\"{usernameInput1.text}\",\"contrassenya\":\"{passwordInput1.text}\",\"id_personatge\":{defaultCharacterId}}}";
        string json2 = $"{{\"nom\":\"{nameInput2.text}\",\"usuari\":\"{usernameInput2.text}\",\"contrassenya\":\"{passwordInput2.text}\",\"id_personatge\":{defaultCharacterId}}}";

        int player1Id = -1;
        int player2Id = -1;

        yield return StartCoroutine(LoginPlayer(json1, id => player1Id = id));
        yield return StartCoroutine(LoginPlayer(json2, id => player2Id = id));

        if (player1Id > 0 && player2Id > 0)
        {
            if (GameManager.Instance == null)
{
    Debug.LogError("❌ GameManager.Instance és null. Assegura't que el GameManager és present a l'escena de login.");
    errorMessage.text = "Error intern: GameManager no trobat.";
    yield break;
}

            GameManager.Instance.jugador1Id = player1Id;
            GameManager.Instance.jugador2Id = player2Id;
Destroy(gameObject); // Destruye el Register actual para que no persista

            SceneManager.LoadScene("1-1");
        }
        else
        {
            Debug.Log("Login failed for one or both players");
            errorMessage.text = "Login fallit per algun jugador";
        }
    }

    private IEnumerator LoginPlayer(string jsonData, System.Action<int> onSuccess)
    {
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                int jugadorId = JsonUtility.FromJson<JugadorResponse>(jsonResponse).jugador.id;
                onSuccess(jugadorId);
            }
            else
            {
                onSuccess(-1);
            }
        }
    }

    [System.Serializable]
    public class Jugador
    {
        public int id;
        public string nom;
        public string usuari;
    }

    [System.Serializable]
    public class JugadorResponse
    {
        public string missatge;
        public Jugador jugador;
    }
}