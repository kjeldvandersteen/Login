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
    private string url = "http://127.0.0.1/edsa-webdev/accountManager.php";

    private TextField email;
    private TextField username;
    private TextField password;

    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        email = root.Q<TextField>("Email");
        username = root.Q<TextField>("Username");
        password = root.Q<TextField>("Password");

        Button submit = root.Q<Button>("Submit");
        Button back = root.Q<Button>("back");

        submit.RegisterCallback<ClickEvent>(evt =>
        {
            StartCoroutine(CreateAccountRequest());
        });

        back.RegisterCallback<ClickEvent>(evt =>
        {
            this.gameObject.SetActive(false);
            MainMenu.SetActive(true);
        });
    }


    private IEnumerator CreateAccountRequest()
    {
        // De volgende instance kun je aanmaken in de coroutine zelf, of je kunt hem als parameter meegeven als je die definieert
        // De variabelen email en password haal je in dit geval normaal uit de TextFields
        CreateAccountRequest request = new CreateAccountRequest();
        request.email = email.text;
        request.username = username.text;
        request.password = password.text;

        // Zorg dat je je WebRequestHandler class instance kunt aanroepen, in mijn geval doe
        // ik dat met een FindFirstObjectByType (lelijk) als voorbeeld
        WebRequestHandler webRequestHandler = FindFirstObjectByType<WebRequestHandler>();

        yield return StartCoroutine(webRequestHandler.WebRequest<CreateAccountRequest, CreateAccountResponse>(request, response => {
            if (response != null)
            {
                Debug.Log($"Error: {response.status} {response.customMessage}");
            }
            else
            {
                Debug.LogError("Failed to get a valid response.");
            }
        }));
    }
    
}

public class CreateAccountRequest : AbstractRequest
{
    public string email;
    public string username;
    public string password;
    public CreateAccountRequest()
    {
        action = "createAccount";
    }
}

[System.Serializable]
public class CreateAccountResponse : AbstractResponse
{
    public string token;
}

