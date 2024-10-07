using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ImputExample : MonoBehaviour
{
    private UIDocument UIDocument;
    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        TextField textField = new TextField("Username");
        root.Add(textField);

        Button button = new Button();
        button.text = "Submit";
        button.RegisterCallback<ClickEvent>(evt => { Debug.Log(textField.text); });
        root.Add(button);

       /* for (int i = 0; i < 10; i++)
        {
            int index = i;
            Button button = new Button();
            button.text = "Button" + i;
            button.RegisterCallback<ClickEvent>(evt => { 
                Debug.Log("Mijn Nummer is: " + index); }) ;
            root.Add(button);
        }*/
        
    }

}
