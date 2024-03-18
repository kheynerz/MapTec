using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField] public List<GameObject> neighbors = new();
    [SerializeField] public string buildingName;
}