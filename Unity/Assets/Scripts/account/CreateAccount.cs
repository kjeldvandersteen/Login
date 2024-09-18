using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.Networking;

public class CreateAccount : MonoBehaviour
{
    private UIDocument UIDocument;
    [SerializeField] private GameObject MainMenu;
    private string url = "http://127.0.0.1/edsa-webdev/CreateAccount.php";
    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        TextField email = root.Q<TextField>("Email");
        TextField username = root.Q<TextField>("Username");
        TextField password = root.Q<TextField>("Password");

        Button submit = root.Q<Button>("Submit");
        Button back = root.Q<Button>("back");

        submit.RegisterCallback<ClickEvent>(evt =>
        {

            StartCoroutine(CreateAccountRequestAsync(new CreateAccountRequest
            {
                email = email.text,
                usermane = username.text,
                password = password.text
            }));
        });

        back.RegisterCallback<ClickEvent>(evt =>
        {
            this.gameObject.SetActive(false);
            MainMenu.SetActive(true);
        });
    }

    private IEnumerator CreateAccountRequestAsync(CreateAccountRequest request)
    {
        string json = JsonUtility.ToJson(request);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>();
        form.Add(new MultipartFormDataSection("data", json));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {

            // Zet er een timeout op, bijvoorbeeld 10 seconden, zodat de gebruiker niet te lang hoeft te wachten
            webRequest.timeout = 10;

            yield return webRequest.SendWebRequest();

            // Check of er errors waren, gezien het normaal over het internet gaat kan er vanalles mis gaan
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {

                // Laat de error zien als er iets anders mis gaat als wat er in PHP gebeurd.
                Debug.LogError($"Request failed: {webRequest.error}");
            }
            else
            {
                // Check of de response valide is.
                if (webRequest.downloadHandler != null && !string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    try
                    {
                        CreateAccountResponse response = JsonUtility.FromJson<CreateAccountResponse>(webRequest.downloadHandler.text);
                        // Het is een goed idee om tijdens de development deze twee variabelen te debug loggen.
                        Debug.Log($"Account created reponse: {response.status}, {response.customMessage}");

                        Debug.Log($"password is: {response.passwordLog}");
                        // Op deze plek komt de code die het succesvol creëren van het account afhandelt.

                    }
                    catch (Exception ex)
                    {
                        // Mocht hij de JSON niet kunnen decoden, log dan wat het probleem is
                        Debug.LogError($"Failed to parse response: {ex.Message}");
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
public class CreateAccountRequest
{
    public string action = "createAccount";
    public string email;
    public string usermane;
    public string password;

}

[System.Serializable]
public class CreateAccountResponse
{
    public string status;
    public string customMessage;
    public string passwordLog;
}
