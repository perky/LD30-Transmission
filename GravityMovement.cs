using UnityEngine;
using System.Collections;

public class GravityMovement : MonoBehaviour
{
	public static float G = 0.1f;

	protected Rigidbody2D[] bodies;
	protected Rigidbody2D myBody;

	private Vector2 velocity;

	void Start()
	{
		myBody = rigidbody2D;
		bodies = GameObject.FindObjectsOfType<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		if (rigidbody2D.isKinematic)
		{
			return;
		}

		foreach(var body in bodies)
		{
			if (body != myBody)
			{
				ApplyGravityForce(body);
			}
		}
	}

	protected void ApplyGravityForce(Rigidbody2D body)
	{
		Vector2 difference = (body.position - myBody.position);
		Vector2 direction = difference.normalized;
		float distance = difference.SqrMagnitude();
		float force = G * ((body.mass * myBody.mass) / distance);

		myBody.AddForce(direction * force);
	}
}

