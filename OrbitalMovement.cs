using UnityEngine;
using System.Collections;

public class OrbitalMovement : MonoBehaviour
{
    public Ellipse ellipse;
	public float startPhase = 0f;
	public float rotateSpeed = 1f;
	public Transform parent;
    public LineRenderer orbitLine;

	private float accumulator;
    private Vector2[] orbitPoints;

    void Awake()
    {
        orbitLine = transform.FindChild("OrbitLine").GetComponent<LineRenderer>();
    }

    void Start()
    {
		accumulator = startPhase;
        transform.position = parent.position + (Vector3)ellipse.PointAt(accumulator);
    }

    void OnEnable()
    {
        orbitPoints = ellipse.AllPoints(300);
    }

    public float CalculateOrbitalSpeed()
    {
        float gParameter = GravityMovement.G * (rigidbody2D.mass + parent.rigidbody2D.mass);
        float radius = Vector3.Distance(transform.position, parent.position);
        return Mathf.Sqrt(gParameter * ((2.0f / radius) - (1.0f / ellipse.semiMajorAxis))) * 0.1f;
    }

    void Update()
    {
        if (orbitLine != null)
        {
            RenderOrbit();
        }
    }
	
    void FixedUpdate()
    {
        float speed = CalculateOrbitalSpeed();
        if (!float.IsNaN(speed))
        {
            accumulator += Time.fixedDeltaTime * CalculateOrbitalSpeed();
            Vector2 point = ellipse.PointAt(accumulator);

            transform.position = parent.position + new Vector3(point.x, point.y, 0);
            transform.Rotate(0, 0, rotateSpeed * Time.fixedDeltaTime);
        }
    }

	void OnDrawGizmos()
	{
        ellipse.DebugDraw(parent.position);

		if (!Application.isPlaying)
		{
			Vector3 startPos = (Vector3)ellipse.PointAt(startPhase);
			transform.position = parent.position + startPos;
		}
	}

    public void DrawOrbit()
    {
        orbitPoints = ellipse.AllPoints(100);
        RenderOrbit();
    }

    void RenderOrbit()
    {
        Vector3 offset = new Vector3(0, 0, 1);
        orbitLine.SetVertexCount(orbitPoints.Length);
        for (int i = 0; i < orbitPoints.Length; i++)
        {
            orbitLine.SetPosition(i, offset + parent.position + (Vector3)orbitPoints[i]);
        }
    }
}

