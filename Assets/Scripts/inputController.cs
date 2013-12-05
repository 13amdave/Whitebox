//Script by Diggermoth (C T)
//This script currently handles only camera commands
/*
 * changes
 * 2013-11-21:
 * 
 * 2013-11-23:
 * 
 * */
using UnityEngine;
using System.Collections;

public class inputController : MonoBehaviour {
	//how close to the edge of the screen the mouse has to be to make the camera scroll
	public SpawnBuilding spawnScript;
	public int mouseMoveBorder = 50;
	//keys used for camera scrolling
	public KeyCode camUpKey = KeyCode.UpArrow;
	public KeyCode camDownKey = KeyCode.DownArrow;
	public KeyCode camLeftKey = KeyCode.LeftArrow;
	public KeyCode camRightKey = KeyCode.RightArrow;
	public KeyCode camZoomOutKey = KeyCode.X;
	public KeyCode camZoomInKey = KeyCode.Z;
	public Camera mainCamera;
	public Rigidbody testObject;
	//public static GameObject[] currentSelectedObjects;
	public static GameObject lastClickedNode = null;
	public static Color lastClickedNodeColor;
	//mouse scroll makes debugging tricky
	//public bool useMouseScroll = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Check every relevant keypress and call appropriate functions or set appropriate variables
		//mouse and keyboard split
		//cameraControl variables will be reset after they are used to move the camera
		if(Input.GetKey(camLeftKey)){
			cameraControl.moveCameraHorizontal = 1;
		}
		if(Input.GetKey(camRightKey)){
			cameraControl.moveCameraHorizontal = -1;
		}
		if(Input.GetKey(camUpKey)){
			cameraControl.moveCameraVertical = 1;
		}
		if(Input.GetKey(camDownKey)){
			cameraControl.moveCameraVertical = -1;
		}
		if(Input.mousePosition.x < mouseMoveBorder){
			cameraControl.moveCameraHorizontalMouse = 1;
		}
		if(Input.mousePosition.x > Screen.width - mouseMoveBorder){
			cameraControl.moveCameraHorizontalMouse = -1;
		}
		if(Input.mousePosition.y > Screen.height - mouseMoveBorder){
			cameraControl.moveCameraVerticalMouse = 1;
		}
		if(Input.mousePosition.y < mouseMoveBorder){
			cameraControl.moveCameraVerticalMouse = -1;
		}
		//camera zoom...
		if(Input.GetKey(camZoomOutKey)){
			cameraControl.moveCameraZoom = 1.0f;
		}
		if(Input.GetKey(camZoomInKey)){
			cameraControl.moveCameraZoom = -1.0f;
		}
		if(Input.GetAxis("Mouse ScrollWheel") != 0) cameraControl.moveCameraZoomMouse -= Input.GetAxis("Mouse ScrollWheel");
		//just some debug data to check the mouse input etc...
		//Debug.Log (Input.GetAxis("Mouse X") + " - " + Input.GetAxisRaw("Mouse Y") + " - " + Input.mousePosition);
		//Screen.width			Screen.height
		//Debug.Log(mainCamera.ViewportToWorldPoint(new Vector3 (0.5f,0.5f, 9.5f)));
		
		//Get mouse click position in 3D space:
		if(Input.GetMouseButtonDown(0)){
			float nodeRange = 5;
			float nodeDist = 0;
			RaycastHit hit;
			//NodeScript nodesScript;
			Ray ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0.0f));
			if(Physics.Raycast(ray, out hit)) {
				float distanceToGround = hit.distance;
				if(lastClickedNode != null){
					Debug.Log (Vector3.Distance(hit.transform.position, lastClickedNode.transform.position));
					nodeDist = Vector3.Distance(hit.point, lastClickedNode.transform.position);
					if(nodeDist < nodeRange && nodeDist > 2){
						//nodesScript = lastClickedNode.GetComponent<NodeScript>();
						spawnScript.SpawnNewBuilding(hit.point, -1, lastClickedNode);
						Instantiate (testObject, hit.point + new Vector3(0.0f, 0.6f, 0.0f), new Quaternion(0.0f,0.0f,0.0f,0.0f));
					}
				}
				//the actual point and distance that the click ray hit something...
				Debug.Log("Click 0: Object distance: " + hit.distance + ", Clicked point: " + hit.point);	
				//remove the comment status from the following line to spawn spheres on top of where the mouse clicked.
				
			}
		}
		//right clicks here
		if(Input.GetMouseButtonDown(1)){
			GameObject[] allCheckPoints;
			GameObject nearestNode = null;
			float nearestNodeDistance, newNodeDistance;
			RaycastHit hit;
			NodeScript nodesScript;
			Ray ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0.0f));
			if(Physics.Raycast(ray, out hit)) {
				float distanceToGround = hit.distance;
				//the actual point and distance that the click ray hit something...
				Debug.Log("Click 1: Object distance: " + hit.distance + ", Clicked point: " + hit.point);	
				//replace testObject with mouse click indicator object when available
				Instantiate (testObject, hit.point + new Vector3(0.0f, 0.0f, 0.6f), new Quaternion(0.0f,0.0f,0.0f,0.0f));
				allCheckPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
				nearestNodeDistance = 999;
				for(int i = 0; i < allCheckPoints.Length; i++)
				{
					newNodeDistance = Vector3.Distance(hit.point, allCheckPoints[i].transform.position);
					if(nearestNode == null || newNodeDistance < nearestNodeDistance) {
						nearestNode = allCheckPoints[i];
						nearestNodeDistance = newNodeDistance;
					}
				}
				//indicates the checkpoint marked for a rally point
				Instantiate (testObject, nearestNode.transform.position + new Vector3(0.0f, 0.0f, 0.6f), new Quaternion(0.0f,0.0f,0.0f,0.0f));
				if(lastClickedNode != null) {
					nodesScript = lastClickedNode.GetComponent<NodeScript>();
					nodesScript.rallyPoint = nearestNode.transform;
				}
			}
			//elementsbytagname
		}
		//middle button clicks
		if(Input.GetMouseButtonDown(2)){
			RaycastHit hit;
			Ray ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 0.0f));
			if(Physics.Raycast(ray, out hit)) {
				float distanceToGround = hit.distance;
				//the actual point and distance that the click ray hit something...
				Debug.Log("Click 2: Object distance: " + hit.distance + ", Clicked point: " + hit.point);	
				//remove the comment status from the following line to spawn spheres on top of where the mouse clicked.
				Instantiate (testObject, hit.point + new Vector3(0.0f, 0.0f, 0.6f), new Quaternion(0.0f,0.0f,0.0f,0.0f));
			}
		}
	}
}
