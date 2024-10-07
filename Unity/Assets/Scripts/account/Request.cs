using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Request : MonoBehaviour
{
    private string url = "http://127.0.0.1/edsa-webdev/API.php";
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RequestExample());
        }
    }

    public IEnumerator RequestExample()
    {
        HighScore highScore = new HighScore();
        highScore.name = "Laurence";
        highScore.score = 43256;

        string json = JsonUtility.ToJson(highScore);

        List<IMultipartFormSection> form = new();
        form.Add(new MultipartFormDataSection("json", json));

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();
            Villager villager = JsonUtility.FromJson<Villager>(webRequest.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class Villager
{
    public string name;
    public int age;
    public int craftSkill;
    public int fightSkill;
}
