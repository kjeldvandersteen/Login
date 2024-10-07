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
            if (hit.transform.gameObject.GetComponent<GridTile>().gridTileData.plotType == "Empty")
            {
                hit.transform.gameObject.GetComponent<GridTile>().gridTileData.plotType = "Plot";
                hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
        else
        {
            Debug.Log("Helaas pindakaas");
        }
    }
}
