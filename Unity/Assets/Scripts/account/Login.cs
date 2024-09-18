using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class Login : MonoBehaviour
{
    private UIDocument UIDocument;
    [SerializeField] private GameObject MainMenu;

    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        TextField email = root.Q<TextField>("Email");
        TextField password = root.Q<TextField>("Password");

        Button submit = root.Q<Button>("Submit");
        Button back = root.Q<Button>("back");

        submit.RegisterCallback<ClickEvent>(evt =>
        {
            LoginRequest LoginRequest = new LoginRequest();

            LoginRequest.Email = email.text;
            LoginRequest.Password = password.text;

        });

        back.RegisterCallback<ClickEvent>(evt =>
        {
            this.gameObject.SetActive(false);
            MainMenu.SetActive(true);
        });

    }
}

[System.Serializable]
public class LoginRequest
{
    public string Action = "LoginAccount";
    public string Email;
    public string Password;
}