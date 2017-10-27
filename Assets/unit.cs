using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class unit : MonoBehaviour 
{
	public AllUnit manager;
	public Vector2 location = Vector2.zero;
	public Vector2 velocity;
	private Vector2 goalPos = Vector2.zero;
	private Vector2 currentForce;


	// Use this for initialization
	void Start ()
	{
		velocity = new Vector2 (Random.Range (0.01f, 0.1f), Random.Range (0.01f, 0.1f));
		location = new Vector2 (transform.position.x, transform.position.y);
		//manager = FindObjectOfType<AllUnit> ();
	}

	Vector2 seek(Vector2 target)
	{
		return(target - location);
	}

	void applyForce(Vector2 f)
	{
		Vector2 force = f;
		if(force.magnitude > manager.maxForce)
		{
			force = force.normalized;
			force *= manager.GetComponent<AllUnit> ().maxForce;
		}
		this.GetComponent<Rigidbody2D> ().AddForce (force);

		if(this.GetComponent<Rigidbody2D> ().velocity.magnitude > manager.maxVelocity)
		{
			this.GetComponent<Rigidbody2D> ().velocity = this.GetComponent<Rigidbody2D> ().velocity.normalized;
			this.GetComponent<Rigidbody2D> ().velocity *= manager.GetComponent<AllUnit> ().maxVelocity;
		}
	
		Debug.DrawRay (this.transform.position, f, Color.white);
	}

	Vector2 Align()
	{
		float neighbourDist = manager.neighbourDistance;
		Vector2 sum = Vector2.zero;
		int count = 0;
		foreach (var item in manager.units) 
		{
			if(item == this.gameObject) 
			{
				continue;
			}

			float d = Vector2.Distance (location, item.GetComponent<unit> ().location);

			if (d < neighbourDist)	
			{
				sum += item.GetComponent<unit> ().velocity;
				count++;
			}
		}

		if(count > 0)
		{
			sum /= count;
			Vector2 steer = sum - velocity;
			return steer;
		}

		return Vector2.zero;
	}

	Vector2 Cohesion()
	{
		float neighbourDist = manager.neighbourDistance;
		Vector2 sum = Vector2.zero;
		int count = 0;

		foreach (var item in manager.units) 
		{
			if(item == this.gameObject) 
			{
				continue;
			}

			float d = Vector2.Distance (location, item.GetComponent<unit> ().location);

			if (d < neighbourDist)	
			{
				sum += item.GetComponent<unit> ().velocity;
				count++;
			}
		}

		if(count > 0)
		{
			sum /= count;
			return seek(sum);
		}

		return Vector2.zero;
	}

	void Flock()
	{
		location = this.transform.position;
		velocity = GetComponent<Rigidbody2D> ().velocity;
		if(manager.obedient && Random.Range(0, 50) <= 1)
		{
			Vector2 ali = Align ();
			Vector2 coh = Cohesion ();
			Vector2 gl;
			if(manager.seekGoal)
			{
				gl = seek (goalPos);
				currentForce = gl + ali + coh;
			}
			else
			{
				currentForce = ali + coh;
			}
			currentForce = currentForce.normalized;
		}
			
		if (manager.willful && Random.Range (0, 50) <= 1)
		{
			if (Random.Range (0, 50) < 1)
			{
				currentForce = new Vector2 (Random.Range (0.01f, 0.1f), Random.Range (0.01f, 0.1f));
			}
		}
		applyForce (currentForce);
	}

	// Update is called once per frame
	void Update () 
	{
		Flock ();
		goalPos = manager.transform.position;
	}
}
