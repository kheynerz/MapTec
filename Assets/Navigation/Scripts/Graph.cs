using System;
using UnityEngine;
using System.Collections.Generic;

public class Graph : MonoBehaviour {
    //GRAPH STRUCTURE RELATED
    private List<GameObject> _graphNodes;
    private float[,] _matrix;

    //PATHFINDING RELATED
    public  int distanceToChangeDirection = 5;
    private Dijkstra _dijkstra;
    private bool _navigating;
    private int _nextNodeIndex;
    private GameObject _nextNode;
    private List<int> _shortestPath;

    //LOCATIONS RELATED
    private readonly Dictionary<string, string> _locations = new();

    //PLAYER AND NAVIGATION RELATED (NAVIGATION ARROW AND PLAYER TRANSFORM)
    public GameObject player;
    private NavigationArrow _arrowScript;
    
    private void Start(){
        InitializeLocations();
        BuildAdjacencyMatrix();
        _dijkstra = new Dijkstra(_matrix.GetLength(0), _matrix);
    }

    private void InitializeLocations()
    {
        _arrowScript = gameObject.GetComponent<NavigationArrow>();
        GameObject[] buildingNodes = GameObject.FindGameObjectsWithTag("BuildingNode");

        foreach (GameObject node in buildingNodes) {
            GraphNode script = node.GetComponent<GraphNode>();
            string nodeId = node.name;
            string buildingName = script.buildingName;

            if (string.IsNullOrEmpty(buildingName)) {
                Debug.LogWarning($"The building node: {nodeId} does not have the buildingName variable set");
                continue;
            }
            _locations.Add(nodeId, buildingName);
        }
    }

    private void BuildAdjacencyMatrix()
    {
        _graphNodes = new List<GameObject>(GameObject.FindGameObjectsWithTag("GraphNode"));
        _graphNodes.AddRange(GameObject.FindGameObjectsWithTag("BuildingNode"));

        int totalNodes = _graphNodes.Count;
        _matrix = new float[totalNodes, totalNodes];
        
        //Initialize Adjacency matrix
        for (int i = 0; i < totalNodes; i++)
            for (int j = 0; j < totalNodes; j++)
                _matrix[i, j] = float.MaxValue;

        //Calculate distances and add them to the matrix
        foreach (GameObject node in _graphNodes)
        {
            var nodeScript = node.GetComponent<GraphNode>();
            foreach (GameObject neighbor in nodeScript.neighbors)
            {
                int i = int.Parse(node.name);
                int j = int.Parse(neighbor.name);
                float dist = Vector3.Distance(node.transform.position, neighbor.transform.position);
                _matrix[i, j] = dist;
            }
        }
    }
    
    private void WrongLocationValue(string locationID)
    {
        string message = $"Wrong Input on Location ID: {locationID}\nAvailable IDs:\n";
        foreach (var kvp in _locations)
           message += $"Location: {kvp.Value}, Value: {kvp.Key}\n";
        Debug.LogError(message);
    }

    public void RequestNavigation(string locationID)
    {
        if (!_locations.ContainsKey(locationID)) {
            WrongLocationValue(locationID);
            return;
        }
        
        int destinationNode = int.Parse(locationID);
        int playerNearestNode = GetPlayerNearestNode();
        
        
        //TODO UI TOASTER (ALREADY ON REQUESTED LOCATION)
        if (playerNearestNode == destinationNode) return;
        
        //TODO UI TOASTER (NAVIGATING TO {LOCATION})
         _shortestPath = _dijkstra.FindShortestPath(playerNearestNode, destinationNode);

        //ERROR WILL HAPPEN IF THERE IS NO ROUTE TO DESTINATION (CHECK IF NODES ARE CONNECTED)
        //TODO UI TOASTER (SHOW ERROR)
        if (_shortestPath.Count <= 1)
        {
            Debug.LogError("PATH TO DESTINATION NOT FOUND");
            return;
        }
        
        GameObject nearestNode = FindNodeByNodeName(playerNearestNode.ToString());
        GameObject nextNode = FindNodeByNodeName(_shortestPath[1].ToString());

        Vector3 nearestNodePosition = nearestNode.transform.position;
        Vector3 nextNodePosition = nextNode.transform.position;

        float distanceNextNodePlayer = Vector3.Distance(player.transform.position,nextNodePosition);
        float distanceBetweenNodes = Vector3.Distance(nearestNodePosition,nextNodePosition);

        _navigating = true;
        _nextNodeIndex = distanceNextNodePlayer > distanceBetweenNodes ? 0 : 1;
        _nextNode = distanceNextNodePlayer > distanceBetweenNodes ? nearestNode : nextNode;
        _arrowScript.PointArrowAt(_nextNode);
    }
    
    private GameObject FindNodeByNodeName(string nodeName)
    {
        return _graphNodes.Find(node => node.name == nodeName);
    }
    private int GetPlayerNearestNode()
    {
        int closestNode = -1;
        float closestDistance = float.MaxValue;
        
        foreach (GameObject node in _graphNodes)
        {
            float distance = Vector3.Distance(player.transform.position, node.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = int.Parse(node.name);
            }
        }
        return closestNode;
    }
    private void StopNavigation()
    {
        _navigating = false;
        _shortestPath = new List<int>();
        _nextNodeIndex = -1;
        _nextNode = null;
        _arrowScript.PointArrowAt(null);
    }

    private void Update()
    {
        if (!_navigating || _shortestPath.Count == 0 || _nextNodeIndex == -1) return;
        
        //Detect  if it is close enough to change current destination
        float distance = Vector3.Distance(player.transform.position, _nextNode.transform.position);
        if (distance > distanceToChangeDirection) return;
        
        //Destination reached
        if (_shortestPath.Count - 1 == _nextNodeIndex) {
            //TODO UI TOASTER (SHOW REACHED DESTINATION)
            StopNavigation();
            _arrowScript.StopNavigation();
            return;
        } 
        
        //Find Next Node 
        _nextNodeIndex += 1;
        _nextNode = _graphNodes.Find(node => node.name == $"{_shortestPath[_nextNodeIndex]}");
        _arrowScript.PointArrowAt(_nextNode);
    }        
}
