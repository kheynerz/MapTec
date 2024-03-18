using System.Collections.Generic;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    public GameObject navigationArrow;
    private GameObject _targetObject;
    private List<GameObject> _graph;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void PointArrowAt(GameObject target)
    {
        _targetObject = target;
        navigationArrow.SetActive(true);
    }

    public void StopNavigation()
    {
        navigationArrow.SetActive(false);
    }
    
    void Update()
    {
        if (!_targetObject || !_camera) return;

  
        
        Vector3 pos = _camera.WorldToViewportPoint (transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
        pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
        navigationArrow.transform.position = _camera.ViewportToWorldPoint(pos);

        
        var position = _targetObject.transform.position;
        navigationArrow.transform.LookAt(new Vector3(position.x, 0, position.z));

        /*
        float anguloRotacion = Mathf.Atan2(direccionCamara.x, direccionCamara.z) * Mathf.Rad2Deg;
        navigationArrow.transform.rotation = Quaternion.Euler(0, 0, anguloRotacion);
       */
    }

}
