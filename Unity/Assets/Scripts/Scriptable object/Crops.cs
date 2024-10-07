using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "crops", menuName ="CropsManager", order = 0)]
public class Crops : ScriptableObject
{

    public string Name;
    public GameObject Prefab;
    public int TimeForGroth;
}
