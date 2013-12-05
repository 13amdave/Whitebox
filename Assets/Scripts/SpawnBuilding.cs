//Script by Diggermoth (C T)
//This script is called from inputController to spawn a building at the clicked location when a node is selected
//Adjust buildSpacing if the units are getting stuck
using UnityEngine;
using System.Collections;

public class SpawnBuilding : MonoBehaviour {
	public GameObject buildType;
	public float buildSpacing = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//add parameter for structure type to build later
	public void SpawnNewBuilding(Vector3 targetSpot, int teamNumber, GameObject parentNode){
		GameObject[] checkBuildings;
		GameObject buildingSpawned;
		Building buildingScript;
		NodeScript parentScript;
		float minCollision = 999.99f, dist = 0.0f;;
		int i = 0;
		Debug.Log ("Spawn Script Run..." + targetSpot);
		targetSpot [2] = -0.5f;

		checkBuildings = GameObject.FindGameObjectsWithTag ("SpawnBuilding");
		minCollision = Vector3.Distance(targetSpot, parentNode.transform.position);
		for (i=0; i<checkBuildings.Length; i++) {
			dist = Vector3.Distance(targetSpot, checkBuildings[i].transform.position);
			if(dist < minCollision) {
				minCollision = dist;
			}
		}
		if (minCollision > buildSpacing) {
			buildingSpawned = Instantiate (buildType, targetSpot, new Quaternion (0.0f, 0.0f, 0.0f, 0.0f)) as GameObject;
			buildingScript = buildingSpawned.GetComponent<Building> ();
			parentScript = parentNode.GetComponent<NodeScript> ();

			buildingScript.buildingSpawnRate = 12;
			buildingScript.parentNode = parentNode;
			if (teamNumber >= 0)
				buildingScript.Team = teamNumber;
			else
				buildingScript.Team = parentScript.Team;
		}
		//buildingScript.spawnedUnit = 

	}
}
/* 
 * 
 * 
 * 
 * 
 * 
 * */