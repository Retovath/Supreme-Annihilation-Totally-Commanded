using RTS;
using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {
	//Hierarchtical desention of objects and usable objects in the game world
	//Should be able to attch this to any prefab in order to instansiate a 
	//unit or object
	public string objectName;
	public Texture2D buildImage;
	//using ints across the board, no partial values for HP and the like
	public int cost, sellValue, hitPoints, maxHitPoints;
	protected Bounds selectionBounds;
	protected Player player;
	protected string[] actions = {};
	protected bool currentlySelected = false;
	protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

	//Protected is essentially equal to abstract in java
	protected virtual void Awake() 
	{
		selectionBounds = ResourceManager.InvalidBounds;
		CalculateBounds();
	}
	//however it can have a great deal of stuff inserted into its base declaration
	protected virtual void Start () 
	{
		player = transform.root.GetComponentInChildren< Player >();
	}
	
	protected virtual void Update () 
	{
		
	}
	
	protected virtual void OnGUI() 
	{
		if(currentlySelected) DrawSelection();
	}

	//Double Check that the playing area is set when we say hey we selected this object
	public virtual void SetSelection(bool selected, Rect playingArea) {
		currentlySelected = selected;
		if(selected) this.playingArea = playingArea;
	}

	public string[] GetActions() 
	{
		return actions;
	}
	
	public virtual void PerformAction(string actionToPerform) 
	{
		//it is up to children with specific actions to determine what to do with each of those actions
	}
	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
	{
		//only handle input if currently selected
		if(currentlySelected && hitObject && hitObject.name != "Ground") {
			WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
			//clicked on another selectable object
			if(worldObject) ChangeSelection(worldObject, controller);
		}
	}

	private void ChangeSelection(WorldObject worldObject, Player controller) 
	{
		//this should be called by the following line, but there is an outside chance it will not
		SetSelection(false, playingArea);
		if(controller.SelectedObject) controller.SelectedObject.SetSelection(false, playingArea);
		controller.SelectedObject = worldObject;
		worldObject.SetSelection(true, controller.hud.GetPlayingArea());
	}

	private void DrawSelection() 
	{
		GUI.skin = ResourceManager.SelectBoxSkin;
		Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
		//Draw the selection box around the currently selected object, within the bounds of the playing area
		GUI.BeginGroup(playingArea);
		DrawSelectionBox(selectBox);
		GUI.EndGroup();
	}
	//yay, i'm looking at calculating the boundy for my game objects
	public void CalculateBounds() 
	{
		selectionBounds = new Bounds(transform.position, Vector3.zero);
		foreach(Renderer r in GetComponentsInChildren< Renderer >()) {
			selectionBounds.Encapsulate(r.bounds);
		}
	}

	protected virtual void DrawSelectionBox(Rect selectBox)
	{
		GUI.Box(selectBox, "");
	}

	public virtual void SetHoverState(GameObject hoverObject) 
	{
		//only handle input if owned by a human player and currently selected
		if(player && player.Human && currentlySelected) {
			if(hoverObject.name != "Ground") player.hud.SetCursorState(CursorState.Select);
		}
	}

	public bool IsOwnedBy(Player owner)
	{
		if(player && player.Equals(owner)) {
			return true;
		} else {
			return false;
		}
	}
	public Bounds GetSelectionBounds() 
	{
		return selectionBounds;
	}
}
