using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    private UIDocument UIDocument;
    [SerializeField] private GameObject MainMenu;

    private string url = "http://127.0.0.1/edsa-webdev/accountManager.php";

    private TextField email;
    private TextField password;

    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        email = root.Q<TextField>("Email");
        password = root.Q<TextField>("Password");

        Button submit = root.Q<Button>("Submit");
        Button back = root.Q<Button>("back");

        submit.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(LoginRequest());
        });

        back.RegisterCallback<ClickEvent>(evt =>
        {
            this.gameObject.SetActive(false);
            MainMenu.SetActive(true);
        });

    }

    private IEnumerator LoginRequest()
    {
        // De volgende instance kun je aanmaken in de coroutine zelf, of je kunt hem als parameter meegeven als je die definieert
        // De variabelen email en password haal je in dit geval normaal uit de TextFields
        LoginRequest request = new LoginRequest();
        request.email = email.text;
        request.password = password.text;

        // Zorg dat je je WebRequestHandler class instance kunt aanroepen, in mijn geval doe
        // ik dat met een FindFirstObjectByType (lelijk) als voorbeeld
        WebRequestHandler webRequestHandler = FindFirstObjectByType<WebRequestHandler>();

        yield return StartCoroutine(webRequestHandler.WebRequest<LoginRequest, LoginResponse>(request, response => {
            if (response != null)
            {
                // Hier komt de code die uitgevoerd wordt na een succesvolle response
                // Je kunt nu uit de 'response' variable je data halen die je terug hebt gekregen van de server
            }
            else
            {
                Debug.LogError("Failed to get a valid response.");
            }
        }));
    }
}

[System.Serializable]
public class LoginRequest : AbstractRequest
{
    public string email;
    public string password;
    public LoginRequest()
    {
        action = "loginAccount";
    }
}

[System.Serializable]
public class LoginResponse : AbstractResponse
{
    public string token;
}