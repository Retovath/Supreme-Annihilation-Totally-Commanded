﻿using RTS;
using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	//this looks at the folder that the script is a subsidy of 
	private Player player;
	// Use this for initialization
	void Start () {
		//this initilizes the player Class/Object to the script by the name of player
		player = transform.root.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.Human) {
			MoveCamera ();
			RotateCamera ();
		}
	}
	//all mesurements are made in pixels
	private void MoveCamera()
	{
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0,0,0);

		// horizontal camera movement, casting out to the
		// Resource Manager script which is compiled from the RTS folder
		// it checks the bounding box to be setup within the camera
		// and ensures that the camera stays within the bounds of the map
		if(xpos >= 0 && xpos < ResourceManager.ScrollWidth) {
			movement.x -= ResourceManager.ScrollSpeed;
		} else if(xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth) {
			movement.x += ResourceManager.ScrollSpeed;
		}
		
		// vertical camera movement, casting out to the
		// Resource Manager script which is compiled from the RTS folder
		// it checks the bounding box to be setup within the camera
		// and ensures that the camera stays within the bounds of the map
		if(ypos >= 0 && ypos < ResourceManager.ScrollWidth) {
			movement.z -= ResourceManager.ScrollSpeed;
		} else if(ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) {
			movement.z += ResourceManager.ScrollSpeed;
		}
		//make sure movement is in the direction the camera is pointing
		//but ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.mainCamera.transform.TransformDirection(movement);
		movement.y = 0;

		//away from ground movement 
		// potental to add time.deltatime to smooth out any jerkyness
		//if so may hardcode 1k buffer

		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

		//calculate desired camera position based on received input
		Vector3 origin = Camera.mainCamera.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;

		//Sanity Check, am I inside my maximum and minimum camera bounds?
		if(destination.y > ResourceManager.MaxCameraHeight) {
			destination.y = ResourceManager.MaxCameraHeight;
		} else if(destination.y < ResourceManager.MinCameraHeight) {
			destination.y = ResourceManager.MinCameraHeight;
		}
		// actual camera movment
		if(destination != origin) {
			Camera.mainCamera.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}


	}
	private void RotateCamera()
	{
		Vector3 origin = Camera.mainCamera.transform.eulerAngles;
		Vector3 destination = origin;
		
		//detect rotation amount if ALT is being held and the Right mouse button is down
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1)) {
			destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}
		
		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.mainCamera.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}

	}




}