using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
{
    private string url = "http://127.0.0.1/edsa-webdev/accountManager.php";

    public IEnumerator WebRequest<TRequest, TResponse>(TRequest request, Action<TResponse> onComplete)
        where TRequest : AbstractRequest
        where TResponse : AbstractResponse
    {
        string json = JsonUtility.ToJson(request);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection("data", json));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            // Zet er een timeout op, bijvoorbeeld 10 seconden, zodat de speler niet te lang hoeft te wachten
            webRequest.timeout = 10;
            yield return webRequest.SendWebRequest();

            // Check of er errors waren, gezien het normaal over het internet gaat kan er vanalles mis gaan
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {

                // Laat de error zien als er iets anders mis gaat als wat er in PHP gebeurd.
                Debug.LogError($"Request failed: {webRequest.error}");
                onComplete?.Invoke(null); // Return null bij een error
            }
            else
            {
                // Check of de response valide is.
                if (webRequest.downloadHandler != null && !string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    Debug.Log($"Text response: {webRequest.downloadHandler.text}");
                    try
                    {
                        TResponse response = JsonUtility.FromJson<TResponse>(webRequest.downloadHandler.text);
                        Debug.Log($"Status: {response.status}, {response.customMessage}");
                        onComplete?.Invoke(response); // Invoke de callback met de response
                    }
                    catch (Exception ex)
                    {
                        // Mocht hij de JSON niet kunnen decoden, log dan wat het probleem is
                        Debug.LogError($"Failed to parse response: {ex.Message}");
                        onComplete?.Invoke(null); // Return null bij een parse error
                    }
                }
                else
                {
                    Debug.LogError("Received an empty or null response");
                }
            }
        }
    }
}


[System.Serializable]
public abstract class AbstractRequest
{
    public string action;
}

[System.Serializable]
public abstract class AbstractResponse
{
    public string status;
    public string customMessage;
}