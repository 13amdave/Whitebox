//Script by Diggermoth (C T)
//this script should be attached to building units
/*
 * changes
 * 2013-11-21:
 * 
 * 2013-11-23:
 * -bugfix, spawning moved from FixedUpdate to Update to stop generation of an odd collision error in another script
 * */
using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
	
	//the prefab unit type that will be spawned
	public Rigidbody spawnedUnit;
	//Private GameObject 
	
	public string buildingName;
	//private since the script sets it to the object attached
	//private GameObject buildingSprite;
	public float buildingSize = 1.0f;
	public float buildingHealth = 25.0f;
	//How many units will spawn per minute
	public float buildingSpawnRate = 12.0f;
	//not sure how to handle upgrades at this stage
	//private var Building Upgrades
	public Transform rallyPoint;
	public GameObject parentNode;
	
	//how far away units will spawn(needed to prevent units spawning inside buildings, if this is wanted)
	private float spawnOffset = 1.3f;
	//spawnprogress will build up until a number is reached, then the building will spawn a unit and reset
	private float spawnProgress = 0.0f;
	private NodeScript parentScript;
	
	public int Team;
	Unit newUnitScript;
	
	// Use this for initialization
	void Start () {
		//buildingSprite = this.gameObject;
		
		Debug.Log (rallyPoint);
		if(rallyPoint == null) {
			rallyPoint = this.transform;	
		}
		if(parentNode != null) {
			parentScript = parentNode.GetComponent<NodeScript>();
			if(parentScript.rallyPoint != null){
				rallyPoint = parentScript.rallyPoint;	
			}
		}
		if(Team == 0) this.renderer.material.color = Color.red;
		if(Team == 1) this.renderer.material.color = Color.blue;
	}



	//fixed update runs at a fixed interval ... I think
	void FixedUpdate() {
		if(buildingSpawnRate > 0){
			spawnProgress += Time.deltaTime;
			//Debug.Log (spawnProgress + " / " + (60 / buildingSpawnRate));
			//spawning units from fixedupdate seems to create collider issues in another script,
			//spawn in update instead
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(spawnProgress > (60 / buildingSpawnRate)){
			if(parentScript.Active == true && (GameObject.FindGameObjectsWithTag("0").Length + GameObject.FindGameObjectsWithTag("1").Length) < 128) {
				SpawnUnit ();
				/*Debug.Log (parentScript.health);*/
			}
			spawnProgress -= (60 / buildingSpawnRate);
		}
	}
	
	//Spawns an extra unit when someone clicks the building
	void OnMouseDown(){
		Debug.Log ("OMD");
		SpawnUnit ();
	}

	void SpawnUnit (){
		//float randomAngle;
		Rigidbody newUnit;
		Vector3 spawnPosition = this.gameObject.transform.position;
		
		if(parentNode != null) {
			parentScript = parentNode.GetComponent<NodeScript>();
			if(parentScript.rallyPoint != null){
				rallyPoint = parentScript.rallyPoint;	
			}
		}
		//GameObject targetUnit;
		if(rallyPoint == this.transform) {
			float randomAngle = Random.Range(0.0f, 6f);// 0.0f;
			spawnPosition += new Vector3(spawnOffset*Mathf.Cos(randomAngle), spawnOffset*Mathf.Sin(randomAngle), 0.0f);
		} else {
			float xDist = rallyPoint.position[0]-transform.position[0];	//How far to target on x axis
			float yDist = rallyPoint.position[1]-transform.position[1];	//How far to target on z axis
			float zDist = 0.0f;
			spawnPosition += Vector3.Normalize(new Vector3 (xDist,yDist,zDist)) * spawnOffset;
		}
		
		newUnit = Instantiate(spawnedUnit,  spawnPosition, new Quaternion(0f, 0f, 0f, 0f)) as Rigidbody;
		//Change targetUnit, or replace it variable with a wayPoint/rallyPoint to change where units move to
		//targetUnit = GameObject.FindGameObjectsWithTag("SpawnBuilding")[0];
		
		newUnitScript = newUnit.GetComponent <Unit> ();
		newUnitScript.Team = Team;//Mathf.FloorToInt( Random.Range(0, 32767) % 2);//team;
		newUnitScript.CheckPointTarget = rallyPoint;//targetUnit.transform;//GameObject.FindGameObjectsWithTag("defaultSpawnBuilding")[0].transform;//target;
		newUnitScript.Team = Team;
		if (Team == 0){
			newUnit.gameObject.tag = "0";
			newUnitScript.EnemyTeamName = "1";
		}else{
			newUnit.gameObject.tag = "1";
			newUnitScript.EnemyTeamName = "0";
		}
		
		//newUnitScript.EnemyTeamName = EnemyTeam[0];
		//movementScript.CheckPointTarget = target[(int)(Random.value * 2)];
		//newUnitScript.CheckPointTarget = target[0];
		
	}

}
