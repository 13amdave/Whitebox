//attached to testObject... replace test object with click indicator if one is to be used
using UnityEngine;
using System.Collections;

public class testObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 0.75f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
