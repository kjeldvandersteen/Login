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
                gridTile.gridTileData.PlotType = "Empty";
            }
        }

        //StartCoroutine(CreateAccountRequest());
    }

    public IEnumerator AttemptToCreatePlotAsync(GridTile gridTile, string newPlotType)
    {
        CreatePlotRequest request = new CreatePlotRequest();
        request.gridTileData = gridTile.gridTileData;
        request.newPlotType = newPlotType;
        request.token = PlayerPrefs.GetString("Token");

        WebRequestHandler webRequestHandler = FindFirstObjectByType<WebRequestHandler>();

        yield return StartCoroutine(webRequestHandler.WebRequest<CreatePlotRequest, CreatePlotResponse>(request, response => {
            if (response != null)
            {
                if (response.status == "succes")
                {
                    gridTile.gridTileData.PlotType = newPlotType;
                    gridTile.GetComponent<MeshRenderer>().material.color = Color.white;
                } else
                {
                    Debug.Log("Server zegt dat het niet mag");
                }
                Debug.Log($"Response: {response.status} {response.customMessage}");
                
            }
            else
            {
                Debug.LogError("Failed to get a valid response.");
            }
        }));
    }
}

public class CreatePlotRequest : AbstractRequest
{
    public string token;
    public GridTileData gridTileData;
    public string newPlotType;
    public CreatePlotRequest()
    {
        action = "CreatePlot";
    }
}

public class CreatePlotResponse : AbstractResponse
{

}