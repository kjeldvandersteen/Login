using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private int GridXLength;
    [SerializeField] private int GridYLength;

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private Transform StartPos;

    private void Start()
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
                gridTile.gridTileData.gridPos = new(x, y);
                gridTile.gridTileData.plotType = "Empty";
            }
        }
    }
}

public class PlotRequest : AbstractRequest
{
    public Vector2Int[] GridPos;
    public string[] PlotType;
}

public class PlotResponse : AbstractResponse
{

}
