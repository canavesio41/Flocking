using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour 
{
	public GameObject boid;
	public int nOfBoids;
	public List<Boid> boids;
	public float bounds;
	public float spawnRadius;

	public float wanderJitter;
	public float wanderRadius;
	public float wanderDistance;
	public float wanderPriority;

	public float cohesionRadius;
	public float cohesionPriority;

	public float aligmentRadius;
	public float aligmentPriority;

	public float separationRadius;
	public float separationPriority;

	public float avoidanceRadius;
	public float avoidancePriority;


	private void Start () 
	{
		boids = new List<Boid> ();
		Spawn (boid.transform, nOfBoids);
		boids.AddRange (FindObjectsOfType<Boid> ());
	}
	
	private void Update () 	
	{
	}

	//Spawn boids in random area
	private void Spawn(Transform unit, int count)
	{
		for (int i = 0; i < count; i++) 
		{
			 Instantiate (unit, new Vector3 (Random.Range (-spawnRadius, spawnRadius), 0, Random.Range (-spawnRadius, spawnRadius)), Quaternion.identity);	
		}
	}

	public List<Boid> GetNeighbours(Boid b, float radius)
	{
		List<Boid> neighbours = new List<Boid> ();

		foreach (var other in boids) 
		{
			if(other == b)
			{
				continue;	
			}

			if(Vector3.Distance(b.transform.position, other.transform.position) <= radius)
			{
				neighbours.Add (other);	
			}
		}
		return neighbours;
	}
}
