using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour 
{
	public float FOV = 180;
	public float maxAcc;
	public float maxVel;

	public Vector2 position;
	public Vector2 velocity;
	public Vector2 acceleration;
	public Vector2 Target;

	public Manager manager;

	// Use this for initialization
	private void Start () 
	{
		manager = FindObjectOfType<Manager> ();
		position = transform.position;
		velocity = new Vector2 (Random.Range(-3,3), Random.Range(-3,3));
	}

	private void WrapAround(ref Vector2 v, float min, float max)
	{
		v.x = WrapAroundFloat (v.x, min, max);
		v.y = WrapAroundFloat (v.y, min, max);
	}

	private float WrapAroundFloat(float value, float min, float max)
	{
		if(value > max)
		{
			value = min;
		}

		else if (value < min)
		{
			value = max;
		}
		return value;
	}

	private float RandomBinomial()
	{
		return Random.Range (0f, 1f) - Random.Range (0f, 1f);	
	}

	public Vector2 Wander()
	{
		float jitter = manager.wanderJitter * Time.deltaTime;
		Target += new Vector2(RandomBinomial() * jitter, RandomBinomial() * jitter);
		Target = Target.normalized;
		Target *=  manager.wanderRadius;

		Vector2 targetInLocalSpace = Target + new Vector2 (0,  manager.wanderDistance);
		Vector2 targetInWorldSpace = transform.TransformPoint (targetInLocalSpace);

		targetInLocalSpace -= this.position;

		return targetInWorldSpace.normalized;
	}
	public Vector2 Aligment()
	{
		Vector2 aligmentVector = new Vector2 ();
		var members = manager.GetNeighbours (this,  manager.aligmentRadius);
		if(members.Count == 0)
		{
			return aligmentVector;	
		}

		foreach (var item in members) 
		{
			if(isInFOV(item.position))
			{
				aligmentVector += item.velocity;
			}	
		}

		return aligmentVector.normalized;
	}

	public Vector2 Separation()
	{
		Vector2 separateVector = new Vector2 ();
		var m = manager.GetNeighbours (this, manager.separationRadius);
		if(m.Count == 0)
		{
			return separateVector;
		}

		foreach (var item in m) 
		{
			if(isInFOV(item.position))
			{
				Vector2 movingTowards = this.transform.position - item.transform.position;
				if(movingTowards.magnitude > 0)
				{
					separateVector += movingTowards.normalized / movingTowards.magnitude;
				}
			}	
		}
		return separateVector.normalized;
	}

	public Vector2 Cohesion()
	{
		Vector2 cohesionVector = new Vector2 ();
		int count = 0;
		var neighbours = manager.GetNeighbours (this,  manager.cohesionRadius);
		if(neighbours.Count == 0)
		{
			return cohesionVector;	
		}
		foreach (var item in neighbours) 
		{
			if(isInFOV(item.position))
			{
				cohesionVector += item.position;
				count++;
			}	
		}

		if(count == 0)
		{
			return cohesionVector;	
		}

		cohesionVector /= count;
		cohesionVector = cohesionVector - this.position;
		cohesionVector = Vector3.Normalize (cohesionVector);
		return cohesionVector;
	}

	virtual protected Vector2 Combine()
	{
		Vector2 finalVec =  manager.cohesionPriority * Cohesion () +  manager.wanderPriority * Wander () +  manager.aligmentPriority * Aligment ();
		return finalVec;
	}

	bool isInFOV(Vector2 v)
	{
		return Vector2.Angle (velocity, v - this.position) <= FOV;	
	}
	// Update is called once per frame
	private void Update ()
	{
		acceleration = Combine ();
		acceleration = Vector2.ClampMagnitude (acceleration, maxAcc);
		velocity = velocity + acceleration * Time.deltaTime;
		velocity = Vector2.ClampMagnitude (velocity, maxVel);
		position = position + velocity * Time.deltaTime;
		WrapAround (ref position, -manager.bounds, manager.bounds);
		transform.position = position;
	}
}
