using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int GridXLength;
    [SerializeField] private int GridYLength;

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private Transform StartPos;

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < GridXLength; x++)
        {
            for (int y = 0; y < GridYLength; y++)
            {
                GridTile gridTile = Instantiate(TilePrefab, new Vector3(StartPos.position.x + x, 0,StartPos.position.z + y), Quaternion.identity).GetComponent<GridTile>();
                gridTile.gridTileData.PosX = x;
                gridTile.gridTileData.PosY = y;
                gridTile.gridTileData.plotType = "Empty";
            }
        }

        StartCoroutine(CreateAccountRequest());
    }

    private IEnumerator CreateAccountRequest()
    {
        PlotRequest request = new PlotRequest();
        request.token = PlayerPrefs.GetString("Token");

        WebRequestHandler webRequestHandler = FindFirstObjectByType<WebRequestHandler>();

        yield return StartCoroutine(webRequestHandler.WebRequest<PlotRequest, PlotResponse>(request, response => {
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

public class PlotRequest : AbstractRequest
{
    public string token;
    public PlotRequest()
    {
        action = "PlotLoad";
    }
}

public class PlotResponse : AbstractResponse
{

}