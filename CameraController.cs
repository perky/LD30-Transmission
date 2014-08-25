using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform focus;
	public float smoothing;
    public float massToCamSizeRatio = 1.0f;
    public float zoomScrollSpeed = 10.0f;
    public float maxZoom = 1000.0f;
    public float minZoom = 10.0f;

	protected Camera cam;
	protected float targetSize;
    protected CelestialBody[] bodies;
    protected int currentBodyIndex;

	void Awake()
	{
		cam = GetComponent<Camera>();

	}

    void Start()
    {
        bodies = GameObject.FindObjectsOfType<CelestialBody>();
        NextBody();
    }

    public void NextBody()
    {
        currentBodyIndex++;
        if (currentBodyIndex >= bodies.Length)
        {
            currentBodyIndex = 0;
        }
        targetSize = bodies[currentBodyIndex].rigidbody2D.mass * massToCamSizeRatio;
        transform.parent = bodies [currentBodyIndex].transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextBody();
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            targetSize -= zoomScrollSpeed;
            targetSize = Mathf.Max(minZoom, targetSize);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            targetSize += zoomScrollSpeed;
            targetSize = Mathf.Min(maxZoom, targetSize);
        }
        Vector3 targetPos = new Vector3(0, 0, -10);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, smoothing);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothing);
    }

    //DEPRECATED
	void UpdateOLD () 
	{
		//
		float radius = focus.position.magnitude;
		float dot = Mathf.Abs( Vector3.Dot (Vector3.right, focus.position.normalized) );
		targetSize = Mathf.Lerp(radius, radius / cam.aspect, dot) * 1.5f;

		
	}
}
