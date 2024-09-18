using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Main : MonoBehaviour
{
    private UIDocument UIDocument;
    [SerializeField] private GameObject CreateAccount;
    [SerializeField] private GameObject Login;

    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();

        VisualElement root = UIDocument.rootVisualElement;

        Button createAccount = root.Q<Button>("createaccount");
        Button login = root.Q<Button>("login");

        createAccount.RegisterCallback<ClickEvent>(evt =>
        {
            this.gameObject.SetActive(false);
            CreateAccount.SetActive(true);

        });

        login.RegisterCallback<ClickEvent>(evt => 
        {
            this.gameObject.SetActive(false);
            Login.SetActive(true);
        
        });
    }
}
