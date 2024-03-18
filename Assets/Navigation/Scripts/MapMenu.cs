using UnityEngine;


public class MapMenu : MonoBehaviour
{
    public GameObject mapCanvas; 
    private bool _mapActive;
	void Update () {
		if(Input.GetKeyDown(KeyCode.M)){ // if you press the E key
			_mapActive = !_mapActive; // change the state of your bool
            Cursor.visible = _mapActive;
            mapCanvas.SetActive(_mapActive); // display or not the canvas (following the state of the bool)
            if (_mapActive)
                Cursor.lockState = CursorLockMode.None; 
            else
                Cursor.lockState = CursorLockMode.Locked; 
		}
	}
}

