using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct SatConnection
{
    public Satellite sat;
    public bool bIsVisible;
}

public class Satellite : MonoBehaviour
{
    public static int LAYER = 8;

    public int maintananceCost;
    public float updateInterval;
    public LineRenderPool linePool;

    public bool bIsPowered;
    protected float updateAccumulator;
    protected List<SatConnection> satConns = new List<SatConnection>();
    protected Dictionary<Satellite, LineRenderer> lineDict = new Dictionary<Satellite, LineRenderer>();

    void Start()
    {
        linePool.Init(100);
        bIsPowered = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            if (collision.gameObject.GetComponent<Satellite>() != null)
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void Update()
    {
        updateAccumulator += Time.deltaTime;
        if (updateAccumulator >= updateInterval)
        {
            updateAccumulator = 0;
            satConns.Clear();
            if (bIsPowered)
            {
                UpdateSatList();
            }
            CreditsController.SubtractCredits(maintananceCost);
        }
        UpdateAllSatVisibility();
        bIsPowered = true;
    }

    void UpdateSatList()
    {
        Satellite[] tmpSatellites = GameObject.FindObjectsOfType<Satellite>();
        foreach (var sat in tmpSatellites)
        {
            var satConn = new SatConnection();
            satConn.sat = sat;
            satConn.bIsVisible = false;
            satConns.Add(satConn);
        }
    }

    void UpdateAllSatVisibility()
    {
        rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.None;
        collider2D.enabled = false;

        for (int i = 0; i < satConns.Count; i++)
        {
            satConns[i] = GetSatVisibility(satConns[i]);
            if (satConns[i].bIsVisible)
            {
                RenderConnectionLine(satConns[i].sat);
            }
            else
            {
                ClearConnectionLineRender(satConns[i].sat);
            }
        }

        rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        collider2D.enabled = true;
    }

    SatConnection GetSatVisibility(SatConnection satConn)
    {
        if (satConn.sat != null)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, satConn.sat.transform.position, City.IGNORE_CITIES_MASK);
            satConn.bIsVisible = (hit.rigidbody == satConn.sat.rigidbody2D);
        }
        else
        {
            satConn.bIsVisible = false;
        }
        return satConn;
    }

    void RenderConnectionLine(Satellite sat)
    {
        LineRenderer line;
        if (!lineDict.TryGetValue(sat, out line))
        {
            line = linePool.GetLine();
            lineDict.Add(sat, line);
        }
        if (line != null)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, sat.transform.position);
        }
    }

    void ClearConnectionLineRender(Satellite sat)
    {
        LineRenderer line;
        if (lineDict.TryGetValue(sat, out line))
        {
            lineDict.Remove(sat);
            linePool.ReleaseLine(line);
        }
    }

    void OnEnterShadow()
    {
        bIsPowered = false;
    }

    void OnExitShadow()
    {
        bIsPowered = true;
    }

    public List<Satellite> GetConnectedSats()
    {
        List<Satellite> sats = new List<Satellite>();
        foreach (var satConn in satConns)
        {
            if (satConn.bIsVisible)
            {
                sats.Add(satConn.sat);
            }
        }
        return sats;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, .03f);
        Vector3 pos = transform.position;
        foreach (var satConn in satConns)
        {
            if (satConn.bIsVisible)
            {
                Gizmos.DrawLine(pos, satConn.sat.transform.position);
            }
        }
    }
}
