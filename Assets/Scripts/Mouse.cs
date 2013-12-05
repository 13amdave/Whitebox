using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {

	void OnMouseDown ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        Physics.Raycast(ray, out hit);
        
        if(hit.collider.gameObject == gameObject)
        {
           	Debug.Log (hit.collider.gameObject.tag);
        }
    }
}
