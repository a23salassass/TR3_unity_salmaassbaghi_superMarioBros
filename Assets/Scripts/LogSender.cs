using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LogSender : MonoBehaviour
{
    private static string logEndpoint = "http://localhost:4000/api/logs";

    public static void SendLog(string message)
    {
        GameObject sender = new GameObject("LogSender");
        LogSender instance = sender.AddComponent<LogSender>();
        instance.StartCoroutine(instance.SendLogCoroutine(message));
    }

    private IEnumerator SendLogCoroutine(string message)
    {
        string json = JsonUtility.ToJson(new LogMessage(message));
        UnityWebRequest www = new UnityWebRequest(logEndpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Log enviado correctamente.");
        }
        else
        {
            Debug.LogWarning("Error enviando log: " + www.error);
        }

        Destroy(gameObject); 
    }

    [System.Serializable]
    private class LogMessage
    {
        public string message;

        public LogMessage(string msg)
        {
            message = msg;
        }
    }
}
