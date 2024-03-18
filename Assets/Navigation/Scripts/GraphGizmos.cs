using Unity.VisualScripting;
using UnityEngine;

public class GraphGizmos : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showGizmos = false;
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, 1f);
        }
    }
}
