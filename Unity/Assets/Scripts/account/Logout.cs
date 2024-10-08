using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Logout : MonoBehaviour
{
    private UIDocument UIDocument;
    [SerializeField] private GameObject MainMenu;


    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        Button Logout = root.Q<Button>("logout");
        Button back = root.Q<Button>("back");

        Logout.RegisterCallback<ClickEvent>(evt =>
        {
            if (PlayerPrefs.GetString("Token") != string.Empty)
            {
                StartCoroutine(LogoutRequest());
            }
        });

        back.RegisterCallback<ClickEvent>(evt =>
        {
            this.gameObject.SetActive(false);
            MainMenu.SetActive(true);
        });

    }

    private IEnumerator LogoutRequest()
    {
        // De volgende instance kun je aanmaken in de coroutine zelf, of je kunt hem als parameter meegeven als je die definieert
        // De variabelen email en password haal je in dit geval normaal uit de TextFields
        LogoutRequest request = new LogoutRequest();
        request.token = PlayerPrefs.GetString("Token");
        Debug.Log(request.token);

        // Zorg dat je je WebRequestHandler class instance kunt aanroepen, in mijn geval doe
        // ik dat met een FindFirstObjectByType (lelijk) als voorbeeld
        WebRequestHandler webRequestHandler = FindFirstObjectByType<WebRequestHandler>();

        yield return StartCoroutine(webRequestHandler.WebRequest<LogoutRequest, LogoutResponse>(request, response => {
            if (response != null)
            {
                Debug.Log($"Error: {response.status} {response.customMessage}");
                if (response.status == "error")
                {
                    return;
                }
                PlayerPrefs.DeleteKey("Token");
            }
            else
            {
                Debug.LogError("Failed to get a valid response.");
            }
        }));
    }

}

public class LogoutRequest : AbstractRequest
{
    public string token;
    public LogoutRequest()
    {
        action = "logoutRequest";
    }
}

[System.Serializable]
public class LogoutResponse : AbstractResponse
{
}

