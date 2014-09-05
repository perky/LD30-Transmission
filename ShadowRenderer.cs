using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct MeshData
{
    public Mesh mesh;
    public MeshRenderer renderer;
    public PolygonCollider2D collider;
}

public class ShadowRenderer : MonoBehaviour 
{
    public GameObject shadowPrefab;


    private GameObject[] bodies;
    private Dictionary<GameObject, MeshData> shadowDict;

    void Awake()
    {
        shadowDict = new Dictionary<GameObject, MeshData>();
        bodies = GameObject.FindGameObjectsWithTag("Body");
        for (int i = 0; i < bodies.Length; i++)
        {
            var shadow = (GameObject)Instantiate(shadowPrefab);
            shadow.transform.parent = transform;
            shadow.transform.localPosition = Vector3.zero;
            shadow.layer = gameObject.layer;

            var meshData = new MeshData();
            meshData.mesh = shadow.GetComponent<MeshFilter>().mesh;
            meshData.renderer = shadow.GetComponent<MeshRenderer>();
            meshData.collider = shadow.GetComponentInChildren<PolygonCollider2D>();
            SetShadowColors(meshData.mesh);
            shadowDict.Add(bodies[i], meshData);
        }
    }

    void Update()
    {
        foreach(var body in bodies)
        {
            CalculateShadow(body);
        }
    }

    void CalculateShadow(GameObject body)
    {
        float radius = GetBodyRadius(body);
        Vector2 bodyPos = body.rigidbody2D.position;
        Vector2 direction = bodyPos.normalized;
        Vector2 perpendicular = GetPerpendicular(direction, false);
        Vector2 perpendicular2 = GetPerpendicular(direction, true);
        Vector2 point1 = bodyPos + (perpendicular * radius * 0.95f);
        Vector2 point2 = bodyPos + (perpendicular2 * radius * 0.95f);
        RenderShadow(body, point1, point2);
    }

    void RenderShadow(GameObject body, Vector2 point1, Vector2 point2)
    {
        MeshData meshData = shadowDict[body];
        meshData.mesh.vertices = new Vector3[] {
            (Vector3)point1,
            (Vector3)(point2 + point2),
            (Vector3)point2,
            (Vector3)(point1 + point1)
        };

        meshData.mesh.RecalculateBounds();
        meshData.mesh.RecalculateNormals();
        meshData.collider.points = new Vector2[]
        {
            point1,
            point2,
            (point2 + point2),
            (point1 + point1)
        };
    }

    void SetShadowColors(Mesh shadowMesh)
    {
        shadowMesh.colors = new Color[] {
            new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 0),
            new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 0),
        };
    }

    void DebugShadow(Vector2 point1, Vector2 point2)
    {
        Debug.DrawRay(point1, point1, Color.red);
        Debug.DrawRay(point2, point2, Color.red);
        //Debug.DrawLine(Vector3.zero, point1);
        //Debug.DrawLine(Vector3.zero, point2);
    }

    float GetBodyRadius(GameObject body)
    {
        return body.GetComponent<CircleCollider2D>().radius * body.transform.localScale.x;
    }

    Vector2 GetPerpendicular(Vector2 vec, bool flip)
    {
        Vector3 referenceVec = flip ? Vector3.back : Vector3.forward;
        return (Vector2)Vector3.Cross((Vector3)vec, referenceVec);
    }
}
