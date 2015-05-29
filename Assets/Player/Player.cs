using RTS;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public string Username;
	public bool Human;
	public HUD hud;
	public WorldObject SelectedObject { get; set; }

	public int startMinerals, startMineralsLimit, startForcium, startForciumLimit, startUnitCount, startUnitCountLimit;

	private Dictionary< ResourceType, int > resources, resourceLimits;

	// Use this for initialization
	void Start () {
		hud = GetComponentInChildren<HUD> ();
		AddStartResourceLimits();
		AddStartResources();

	
	}

	// Update is called once per frame
	void Update () 
	{
		if (Human) {
			hud.SetResourceValues (resources, resourceLimits);

	
		}
	}
	void Awake() 
	{
		resources = InitResourceList();
		resourceLimits = InitResourceList();
	}
	private Dictionary< ResourceType, int > InitResourceList() 
	{
		Dictionary< ResourceType, int > list = new Dictionary< ResourceType, int >();
		list.Add(ResourceType.Forcium, 0);
		list.Add(ResourceType.Minerals, 0);
		list.Add (ResourceType.UnitCap, 0);
		return list;
	}

	private void AddStartResourceLimits()
	
	{
		IncrementResourcesLimit (ResourceType.Minerals, startMineralsLimit);
		IncrementResourcesLimit (ResourceType.Forcium, startForciumLimit);
		IncrementResourcesLimit (ResourceType.UnitCap, startUnitCountLimit);
	}

	private void AddStartResources()
	{
		AddResource (ResourceType.Minerals, startMineralsLimit);
		AddResource (ResourceType.Forcium, startForciumLimit);
		AddResource (ResourceType.UnitCap, startUnitCountLimit);
	} 

	public void AddResource(ResourceType type, int ammount)
	{
		resources [type] += ammount;
	}

	public void IncrementResourcesLimit(ResourceType type, int ammount)
	{
		resourceLimits [type] += ammount;
	}

	public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation) {
		Debug.Log ("add " + unitName + " to player");
		Units units = GetComponentInChildren< Units >();
		GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
		newUnit.transform.parent = units.transform;
	}

}
