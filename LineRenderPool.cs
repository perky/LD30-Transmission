using UnityEngine;
using System.Collections;

public class LineRenderPool : MonoBehaviour
{
    public GameObject linePrefab;

    private LineRenderer[] lineRenderers;

    public void Init(int size)
    {
        lineRenderers = new LineRenderer[size];
        for (int i = 0; i < size; i++)
        {
            GameObject obj = (GameObject)Instantiate(linePrefab);
            obj.transform.parent = transform;
            lineRenderers[i] = obj.GetComponent<LineRenderer>();
            lineRenderers[i].enabled = false;
        }
    }

    public LineRenderer GetLine()
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            if (!lineRenderers[i].enabled)
            {
                lineRenderers[i].enabled = true;
                return lineRenderers[i];
            }
        }
        return null;
    }

    public void ReleaseLine(LineRenderer line)
    {
        line.enabled = false;
    }

    public void ReleaseAll()
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].enabled = false;
        }
    }
}

