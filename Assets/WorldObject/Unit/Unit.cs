using RTS;
using UnityEngine;
using System.Collections;

public class Unit : WorldObject {
	//thanks unity for all the sealed class objects, this becomes way too easy

	/*** Game Engine methods, all can be overridden by subclass ***/
	
	protected override void Awake() {
		base.Awake();
	}
	
	protected override void Start () {
		base.Start();
	}
	
	protected override void Update () {
		base.Update();
	}
	
	protected override void OnGUI() {
		base.OnGUI();
	}

	public override void SetHoverState(GameObject hoverObject) {
		base.SetHoverState(hoverObject);
		//only handle input if owned by a human player and currently selected
		if(player && player.Human && currentlySelected) {
			if(hoverObject.name == "Ground") player.hud.SetCursorState(CursorState.Move);
		}
	}
}
