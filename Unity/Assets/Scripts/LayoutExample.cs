using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LayoutExample : MonoBehaviour
{
    private UIDocument UIDocument;
    private string url = "http://127.0.0.1/edsa-webdev/Conect.php";

    private void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        TextField Fruit_name = root.Q<TextField>("Fruit_name");
        TextField Fruit_color = root.Q<TextField>("Fruit_color");
        TextField Fruit_quantity = root.Q<TextField>("Fruit_quantity");
        Button button = root.Q<Button>("Submit");

        button.RegisterCallback<ClickEvent>(evt => {

            Fruit newFruit = new Fruit();

            newFruit.Fruit_Name = Fruit_name.text;
            newFruit.Fruit_Color = Fruit_color.text;
            newFruit.Fruit_Quantity = Fruit_quantity.text;
            Debug.Log(Fruit_name.text); 
            Debug.Log(Fruit_color.text);
            Debug.Log(Fruit_quantity.text);
        } );
    }
}
    [System.Serializable]
    public class Fruit
    {
        public string Fruit_Name;
        public string Fruit_Color;
        public string Fruit_Quantity;
    }
