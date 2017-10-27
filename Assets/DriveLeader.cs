using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveLeader : MonoBehaviour 
{
	public float speed = 5.0f;

	// Update is called once per frame
	void Update () 
	{
		var tY = Input.GetAxis ("Vertical") * speed;
		var tX = Input.GetAxis ("Horizontal") * speed;
		tY *= Time.deltaTime;
		tX *= Time.deltaTime;

		transform.Translate (0, tY , 0);
		transform.Translate (tX, 0, 0);
	}
}
