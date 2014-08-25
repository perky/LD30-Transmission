using UnityEngine;
using System.Collections;

[System.Serializable]
public class Ellipse
{
	public float x;
	public float y;
	public float semiMajorAxis;
	public float semiMinorAxis;
	public float angle;

	public Ellipse(float x, float y, float semiMajorAxis, float semiMinorAxis, float angle)
	{
		this.x = x;
		this.y = y;
		this.semiMajorAxis = semiMajorAxis;
		this.semiMinorAxis = semiMinorAxis;
		this.angle = angle;
	}

	public Vector2 PointAt(float alpha)
	{
		float xCosT = (x - FociRadius()) + (semiMajorAxis * Mathf.Cos(alpha));
		float ySinT = y + (semiMinorAxis * Mathf.Sin(alpha));
		float cosAngle = Mathf.Cos(angle);
		float sinAngle = Mathf.Sin(angle); 
		float pointX = (xCosT * cosAngle) - (ySinT * sinAngle);
		float pointY = (xCosT * sinAngle) + (ySinT * cosAngle);

		return new Vector2(pointX, pointY);
	}

	public float FociRadius()
	{
		return Mathf.Sqrt( (semiMajorAxis * semiMajorAxis) - (semiMinorAxis * semiMinorAxis) );
	}

    public Vector2[] AllPoints(int size = 100)
    {
        Vector2[] points = new Vector2[size];
        float maxAlpha = Mathf.PI * 2;
        float interval = maxAlpha / (float)size;
        int index = 0;
        for (float alpha = 0.0f; alpha <= maxAlpha; alpha += interval) 
        {
            points[index++] = PointAt(alpha);
        }
        return points;
    }

    public void DebugDraw(Vector3 offset, float interval = 0.01f)
    {
        Vector3 lastPos = Vector3.zero;
        //Gizmos.DrawWireSphere((Vector3)PointAt(0) + offset, 1);
        Gizmos.color = new Color(1, 1, 1, .03f);
        float maxAlpha = Mathf.PI * 2;
        for (float alpha = 0.0f; alpha <= maxAlpha; alpha += interval) 
        {
            Vector3 pos = (Vector3)PointAt(alpha);
            if (alpha != 0.0f)
            {
                var newPos = new Vector3(pos.x, pos.y, 0);
                Gizmos.DrawLine(lastPos + offset, newPos + offset);
            }
            lastPos = pos;
        }
    }
}
