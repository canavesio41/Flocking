using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnit : MonoBehaviour 
{
	public GameObject[] units;
	public GameObject unitPrefab;
	public int numOfUnits = 10;
	public Vector2 range = new Vector2(5,5);

	public bool seekGoal = true;
	public bool obedient = true;
	public bool willful = false;
	public float sphere = 0.2f;
	[Range(0,200)]
	public int neighbourDistance = 30;
	[Range (0,2)]
	public float maxForce = 0.5f;
	[Range(0,5)]
	public float maxVelocity = 2.0f;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube (this.transform.position, range * 2);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (this.transform.position, sphere);
	}

	void Start()
	{
		units = new GameObject[numOfUnits];
		for (int i = 0; i < numOfUnits; i++) 
		{
			Vector2 unitPos = new Vector2 (Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
			units[i] = Instantiate (unitPrefab, (Vector2)this.transform.position + unitPos, Quaternion.identity) as GameObject;
			units [i].GetComponent<unit> ().manager = this;
		}

	}

}
