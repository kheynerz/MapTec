using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArrow : MonoBehaviour
{
    public GameObject pointerObject;
    private GameObject _targetObject;
    private List<GameObject> _graph;
    public GameObject orientation;
    private Camera _camera;


    private GameObject _player; 
    private void Start()
    {
        _camera = Camera.main;
        _player = GameObject.Find("Player");
    }

    public void PointArrowAt(GameObject target)
    {
        _targetObject = target;
        pointerObject.SetActive(true);
    }

    public void StopNavigation()
    {
        _targetObject = null;
        pointerObject.SetActive(false);
    }
    
    void Update()
    {
        if (!_targetObject || !_camera) return;
        /*
                Vector3 objectPosition = transform.position;
                Vector3 screenPosition = _camera.WorldToScreenPoint(objectPosition);
                screenPosition.y = Screen.height;
                Vector3 newPosition = _camera.ScreenToWorldPoint(screenPosition);
        */
        
        
        
        float angleRadians = orientation.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float sinValue = Mathf.Sin(angleRadians);
        float cosValue = Mathf.Cos(angleRadians);

        Vector3 newPosition = _player.transform.position + new Vector3(sinValue * 5 , 2.8f, cosValue * 5 );
        pointerObject.transform.position = newPosition;

        Vector3 position = _targetObject.transform.position;
        pointerObject.transform.LookAt(new Vector3(position.x, 0, position.z));
        /*
        pointerObject.transform.position = newPosition;
        var position = _targetObject.transform.position;
       
        */
        //pointerObject.transform.position = _player.transform.position;
        

    }
}
