//Script by Diggermoth (C T)
//This script should be attached to the main camera
/*
 * changes
 * 2013-11-23:
 * -bugfix for camera left/right controls flipping on upside down levels (uses xScale)
 * */
using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
	
	//Controls how fast the camera will pan
	public float cameraScrollSpeed = 1.0f;
	//How fast the mouse will make the camera move
	public float cameraScrollSpeedMouse = 0.1f;
	public float cameraZoomFactor = 1.0f;
	public float cameraMouseZoomFactor = 10.0f;
	
	//SUPPOSED to swap the axis used for camera scrolling, but it doesn't actually do anything that I can tell...
	public bool swapYZ = false;
	
	//public Camera mainCamera;
	
	public static int moveCameraHorizontal = 0;
	public static int moveCameraVertical = 0;
	public static int moveCameraHorizontalMouse = 0;
	public static int moveCameraVerticalMouse = 0;
	public static float moveCameraZoom = 0.0f;
	public static float moveCameraZoomMouse = 0.0f;
	
	//public static int moveCameraVerticalMouse = 0;
	
	
	private float targetCamX = 0;
	private float targetCamY = 0; // or targetCamZ
	private Vector3 initialOffset;
	private float initialCamScale = 1.0f;
	private float camScale = 1.0f;
	private int xScale = 1;
	
	
	// Use this for initialization
	void Start () {
		initialOffset = this.transform.position;
		initialCamScale = this.camera.orthographicSize;
		camScale = initialCamScale;
		if (initialOffset [2] < 0)
						xScale = -1;
	}
	
	//update at regular intervals or something
	void FixedUpdate(){
		//Debug.Log("pre: " + camScale + " - " + this.camera.orthographicSize);
		//move the camera if the keys are pressed
		if(moveCameraVertical != 0 || moveCameraHorizontal != 0 || moveCameraVerticalMouse != 0 || moveCameraHorizontalMouse != 0){
			targetCamX += moveCameraHorizontal * cameraScrollSpeed * xScale;
			targetCamY += moveCameraVertical * cameraScrollSpeed;
			targetCamX += moveCameraHorizontalMouse * cameraScrollSpeedMouse * xScale;
			targetCamY += moveCameraVerticalMouse * cameraScrollSpeedMouse;
			//Debug.Log (moveCameraVerticalMouse + " " + moveCameraHorizontalMouse + " " + cameraScrollSpeedMouse);
			//Debug.Log (targetCamX + " - " + targetCamY);
			if(swapYZ == true)	this.transform.position = new Vector3(targetCamX, 0, targetCamY) + initialOffset;
			if(swapYZ == false) this.transform.position = new Vector3(targetCamX, targetCamY, 0) + initialOffset;
			moveCameraVertical = 0;
			moveCameraHorizontal = 0;
			moveCameraVerticalMouse = 0;
			moveCameraHorizontalMouse = 0;
		}
		if(moveCameraZoom != 0 || moveCameraZoomMouse != 0){
			moveCameraZoomMouse *= cameraMouseZoomFactor;
			//Debug.Log("pre: " + initialCamScale + " - " + camScale + " - " + cameraZoomFactor + " - " + this.camera.orthographicSize);
			//Mouse wheel may require event listeners.
			//if(Input.GetAxis("Mouse ScrollWheel") != 0) moveCameraZoom += Input.GetAxis("Mouse ScrollWheel");
			camScale = camScale + ((moveCameraZoom + moveCameraZoomMouse) * cameraZoomFactor);
			if(camScale < 2.5f) camScale = 2.5f;
			this.camera.orthographicSize = camScale;
			//Debug.Log("post: " + camScale + " - " + this.camera.orthographicSize);
			moveCameraZoom = 0;
			moveCameraZoomMouse = 0;
			
		}
	}
	//		Debug.Log(this.camera.ViewportToWorldPoint(Vector3 (1,1, camera.nearClipPlane)));
	// Update is called once per frame
	void Update () {
		
	}
}
