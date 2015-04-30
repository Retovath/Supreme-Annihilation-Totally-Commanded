using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionTest : MonoBehaviour {
	public GameObject Derp;

	private List<GameObject> ObjectsInRange = new List<GameObject>();

		void OnTriggerEnter (Collider other){
		ObjectsInRange.Add (other.gameObject);
		print ("added");
		print (other.gameObject);

	
	}
	void OnTriggerExit(Collider other)
	{
		ObjectsInRange.Remove (other.gameObject);
		print ("removed");
		print (other.gameObject);
	}
	void Update()
	{
		StartCoroutine (Attack ());
	}
	IEnumerator Attack()
	{
		if (ObjectsInRange.Capacity >= 0)
		{
		for(int i = 0 ; i<ObjectsInRange.Capacity-1 ; i++){
			if (ObjectsInRange[i].tag == "Enemy"){
				print("Enemy Destroyed");
				print(ObjectsInRange[i].transform);
				yield return new WaitForSeconds(5);

			}
		}

	}

}

}