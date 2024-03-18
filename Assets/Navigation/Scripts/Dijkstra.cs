using System.Collections.Generic;
internal class Dijkstra
{
    private readonly int _v;
    private readonly float[,] _graph;

    public Dijkstra(int vertices, float[,] adjacencyMatrix)
    {
        _v = vertices;
        _graph = adjacencyMatrix;
    }

    private int MinDistance(float[] dist, bool[] visited)
    {
        float min = float.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < _v; v++)
        {
            if (!visited[v] && dist[v] < min)
            {
                min = dist[v];
                minIndex = v;
            }
        }
        return minIndex;
    }
    public List<int> FindShortestPath(int startNode, int endNode)
    {
        float[] dist = new float[_v];
        int[] parent = new int[_v];
        bool[] visited = new bool[_v];

        for (int i = 0; i < _v; i++)
        {
            dist[i] = float.MaxValue;
            parent[i] = -1;
            visited[i] = false;
        }

        dist[startNode] = 0;

        for (int count = 0; count < _v - 1; count++)
        {
            int u = MinDistance(dist, visited);
            visited[u] = true;

            for (int v = 0; v < _v; v++)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (visited[v] || _graph[u, v] == 0 
                               || dist[u] == float.MaxValue 
                               || !(dist[u] + _graph[u, v] < dist[v])) continue;
                dist[v] = dist[u] + _graph[u, v];
                parent[v] = u;
            }
        }

        List<int> path = new List<int>();
        int current = endNode;

        while (current != -1)
        {
            path.Insert(0, current);
            current = parent[current];
        }

        return path;
    }
}