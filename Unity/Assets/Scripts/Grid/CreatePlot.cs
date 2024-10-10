using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlot : MonoBehaviour
{
    [SerializeField] private Camera MainCam;

    [SerializeField] public LayerMask LayerMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTile();
        }
    }
    private void SelectTile()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask))
        {
            GridTile gridTile = hit.transform.gameObject.GetComponent<GridTile>();
            if (gridTile != null)
            {
                if (gridTile.gridTileData.PlotType == "Empty")
                {
                    StartCoroutine(FindFirstObjectByType<GridController>().AttemptToCreatePlotAsync(gridTile, "corn")); 
                }
            }
        }
        else
        {
            Debug.Log("Helaas pindakaas");
        }
    }
}
