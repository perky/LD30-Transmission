using UnityEngine;
using System.Collections;

public class TrajectoryPredictor : MonoBehaviour 
{
	public Rigidbody2D sun;
	public int simulationSize = 100;

	protected Rigidbody2D[] bodies;
	protected Rigidbody2D myBody;
	protected MissileMovement missileMovement;

	protected Vector2 position;
	protected Vector2 velocity;
	protected Vector2[] trajectory;

	void Start()
	{
		myBody = rigidbody2D;
		bodies = GameObject.FindObjectsOfType<Rigidbody2D>();
		missileMovement = GetComponent<MissileMovement> ();
	}

	void FixedUpdate() 
	{
		Simulate ();
	}

	void Simulate()
	{
		trajectory = new Vector2[simulationSize];
		position = myBody.position;
		if (myBody.isKinematic)
		{
			velocity = (Vector2)(transform.up * missileMovement.LaunchThrust);
		}
		else
		{
			velocity = myBody.velocity;
		}
		for (int i = 0; i < simulationSize; i++)
		{
			trajectory[i] = position;
			SimulateStep();
		}
	}

	void SimulateStep()
	{
		foreach(var body in bodies)
		{
			if (body != myBody)
			{
				ApplyGravityForce(body);
			}
		}
		velocity += (Vector2)(transform.up * missileMovement.FlyThrust * Time.fixedDeltaTime);
		SimulateMove (velocity);
	}

	void OnDrawGizmos()
	{
		if (trajectory != null && Application.isPlaying)
		{
			for (int i = 0; i < simulationSize; i += 2)
			{
				var p1 = new Vector3(trajectory[i].x, trajectory[i].y, 0);
				var p2 = new Vector3(trajectory[i+1].x, trajectory[i+1].y, 0);
				Gizmos.DrawLine(p1, p2);
			}
		}
	}

	protected void ApplyGravityForce(Rigidbody2D body)
	{
		Vector2 difference = (body.position - position);
		Vector2 direction = difference.normalized;
		float distance = difference.sqrMagnitude;
		float force = GravityMovement.G * ((body.mass * myBody.mass) / distance);

		velocity += (direction * force * Time.fixedDeltaTime);
	}

	protected void SimulateMove(Vector2 vel)
	{
		position += vel * Time.fixedDeltaTime;
	}
}
