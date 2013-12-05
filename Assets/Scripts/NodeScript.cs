//Script by Diggermoth (C T)
//this script should be attached to the primary node for each player
/*
 * changes
 * 2013-11-21:
 * -circle appears on the ground around the selected node
 * -left-clicking in the circle will create a new basic building using SpawnBuilding.cs
 * 2013-11-23:
 * -
 * */
using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour {
	
	public string nodeName;
	public float nodeSize = 1.0f;
	public float health = 100.0f;
	public float nodeHealthMax = 100.0f;
	//public GameObject[] childBuildings;
	
	public int Team;

	
	//private GameObject nodeSprite;
	private bool Dead = false;
	public bool Active = true;

	public GameObject selectRegion;
	
	public Transform rallyPoint;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		Vector3 setSpot;
		selectRegion = Instantiate(selectRegion, this.transform.position, selectRegion.transform.rotation) as GameObject;
		setSpot = selectRegion.transform.position;
		setSpot[2] = 0.0f;
		selectRegion.transform.position = setSpot;
		selectRegion.renderer.enabled = false;
		if(Team == 0) this.renderer.material.color = Color.red;
		if(Team == 1) this.renderer.material.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
		//if(health <=0) Destroy (nodeSprite, 0.1f);
	}
	
	void OnCollisionEnter (Collision other)
	{

		//--copied and edited from unit script--//
	switch (other.gameObject.tag)
		{
		case "Bomb":
			{
				health--;
				checkHealth ();
			}
			break;
		case "Creature":
			{
				Unit unitScript;
				unitScript = other.gameObject.GetComponent<Unit>();
				if(unitScript.Team != Team){
					health--;
					checkHealth ();
					Destroy (other.gameObject);
				}
			}
			break;
		case "0":
		{
			Unit unitScript;
			unitScript = other.gameObject.GetComponent<Unit>();
			if(unitScript.Team != Team){
				health--;
				checkHealth ();
				Destroy (other.gameObject);
			}
		}
			break;
		case "1":
		{
			Unit unitScript;
			unitScript = other.gameObject.GetComponent<Unit>();
			if(unitScript.Team != Team){
				health--;
				checkHealth ();
				Destroy (other.gameObject);
			}
		}
			break;
		}
	}
	
	public void checkHealth ()
	{
		if (health <= 0 && ! Dead)
		{
			Dead = true;
			Active = false;
			gameObject.renderer.enabled = false;
			this.collider.enabled = false;
			selectRegion.renderer.enabled = false;
			if(inputController.lastClickedNode == this.gameObject) inputController.lastClickedNode = null;
			//Destroy (selectRegion);
			//Destroy (gameObject);
		}
	}
	

	
	void OnMouseDown(){
		Debug.Log ("OMD");
		if ( inputController.lastClickedNode == null ){
			inputController.lastClickedNode = this.gameObject;
			selectRegion.renderer.enabled = true;
		} else {
			inputController.lastClickedNode.GetComponent<NodeScript>().selectRegion.renderer.enabled = false;
			inputController.lastClickedNode.renderer.material.SetColor ("_Color",inputController.lastClickedNodeColor);
			inputController.lastClickedNode = this.gameObject;
			selectRegion.renderer.enabled = true;
			//Debug.Log ("Length pre: " + inputController.currentSelectedObjects.Length);
			//inputController.currentSelectedObjects[inputController.currentSelectedObjects.Length] = this.gameObject;
			//Debug.Log ("Length post: " + inputController.currentSelectedObjects.Length);
		}
		inputController.lastClickedNodeColor = renderer.material.color;
		renderer.material.SetColor ("_Color",Color.yellow);
		//SpawnUnit ();
	}
}
