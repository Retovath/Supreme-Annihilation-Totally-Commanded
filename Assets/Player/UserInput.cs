using RTS;
using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour {
	//this looks at the folder that the script is a subsidy of 
	private Player player;
	public HUD hud;
	// Use this for initialization
	void Start () {
		//this initilizes the player Class/Object to the script by the name of player
		player = transform.root.GetComponent<Player> ();
		hud = GetComponentInChildren<HUD> ();


	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.Human) {
			MoveCamera ();
			RotateCamera ();
			MouseActivity();
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
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;

		//away from ground movement 
		// potental to add time.deltatime to smooth out any jerkyness
		//if so may hardcode 1k buffer

		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
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
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}


	}

	private void RotateCamera()
	{
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;
		
		//detect rotation amount if ALT is being held and the Right mouse button is down
		if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1)) {
			destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}
		
		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}

	}
	//is used to ask what's going on with the mouse
	private void MouseActivity() 
	{
		if(Input.GetMouseButtonDown(0)) LeftMouseClick();
		else if(Input.GetMouseButtonDown(1)) RightMouseClick();
	}
	// things are gonna get funky, I'm not doing standard RTS ctrls
	// left is select, right is going to be dispell click for now
	// May try to change this in the future with case switches

	private void LeftMouseClick() 
	{
		if(player.hud.MouseInBounds()) {
			//findHitObject is located in User Input
			GameObject hitObject = FindHitObject();
			//findHitPoint is in
			Vector3 hitPoint = FindHitPoint();
			if(hitObject && hitPoint != ResourceManager.InvalidPosition) {
				if(player.SelectedObject) player.SelectedObject.MouseClick(hitObject, hitPoint, player);
				//We can tell if we hit the ground by naming the ground Ground
				//could also be done with a tag not a name referance
				// i did select the ground and move it before this fix
				else if(hitObject.name!="Ground") {
					WorldObject worldObject = hitObject.transform.root.GetComponent< WorldObject >();
					if(worldObject) {
						//we already know the player has no selected object
						player.SelectedObject = worldObject;
						worldObject.SetSelection(true);
					}
				}
			}
		}
	}

	private GameObject FindHitObject() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
		return null;
	}
	//Make everything Private
	private Vector3 FindHitPoint() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.point;
		return ResourceManager.InvalidPosition;
	}
	// dispell if the player holds leftalt and clicks the rmb

	private void RightMouseClick() 
	{
		if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject) {
			player.SelectedObject.SetSelection(false);
			player.SelectedObject = null;
		}
	}


}
