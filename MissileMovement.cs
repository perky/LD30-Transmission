using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour 
{
	public float LaunchThrust;
	public float FlyThrust;
	public float BrakePower;
	public GameObject Thruster;

	protected bool bHasFired;
    protected Collider2D lastCollider;
    protected bool bThrusterEnabled;
    protected ParticleSystem thrusterParticles;

    void Awake()
    {
        thrusterParticles = Thruster.GetComponent<ParticleSystem>();
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Fire();
		}
	}

	void FixedUpdate()
	{
		if (bHasFired)
		{
			RotateToVelocity();
			
            if (Input.GetMouseButton(0))
            {
                rigidbody2D.velocity *= BrakePower;
                Thruster.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                thrusterParticles.startSpeed = 20;
            }
            else
            {
                Thruster.transform.localRotation = Quaternion.Euler(90, 0, 0);
                thrusterParticles.startSpeed = 5;
            }

            if (Input.GetMouseButton(1) || rigidbody2D.isKinematic)
            {
                bThrusterEnabled = false;
                thrusterParticles.enableEmission = false;
            }
            else
            {
                bThrusterEnabled = true;
                thrusterParticles.enableEmission = true;
            }

            if (bThrusterEnabled)
            {
                rigidbody2D.AddForceAtPosition(transform.up * FlyThrust, Thruster.transform.position, ForceMode2D.Force);
            }
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		rigidbody2D.isKinematic = true;
		transform.parent = collision.transform;

		Vector3 dir = (transform.position - collision.transform.position).normalized;
		float zRot = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90.0f;
		CircleCollider2D circle = (CircleCollider2D)collision.collider;
		transform.rotation = Quaternion.Euler( 0, 0, zRot );
		transform.position = collision.transform.position + (dir * circle.radius * collision.transform.localScale.x * 1.4f);
        collider2D.enabled = false;
		bHasFired = false;
	}

	protected void RotateToVelocity()
	{
		float zRot = (Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg) - 90.0f;
		transform.rotation = Quaternion.Euler(0, 0, zRot);
	}

	protected void Fire()
	{
		bHasFired = true;
		transform.parent = null;
		rigidbody2D.isKinematic = false;

		rigidbody2D.AddForce( transform.up * LaunchThrust, ForceMode2D.Impulse);
        Invoke("EnableLastCollider", 0.2f);
	}

    protected void EnableLastCollider()
    {
        collider2D.enabled = true;
    }
}
