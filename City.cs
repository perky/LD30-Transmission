using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class City : MonoBehaviour
{
    public static int LAYER = 9;
    public static int SATS_ONLY_MASK = (1 << Satellite.LAYER);
    public static int IGNORE_CITIES_MASK = ~(1 << City.LAYER);
    public static int IGNORE_CITIES_AND_SHADOWS_MASK = ~((1 << City.LAYER) | (1 << 12));
    public static int CREDIT_PER_CONNECTED_CITY = 10;
    public static City[] cities;

    public int connectedCities;
    public LineRenderPool linePool;

    protected CircleCollider2D circleCollider;
    protected List<SatConnection> satConns;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        satConns = new List<SatConnection>();
        linePool.Init(10);
    }

    void Start()
    {
        if (cities == null)
        {
            cities = GameObject.FindObjectsOfType<City>();
        }

        InvokeRepeating("PayBandwidthFees", 0, 1);
    }

    void Update()
    {
        UpdateVisibleSats();
    }

    void PayBandwidthFees()
    {

        StartCoroutine(ConnectToCities());
    }

    public List<Satellite> GetConnectedSats()
    {
        List<Satellite> sats = new List<Satellite>(satConns.Count);
        foreach (var satConn in satConns)
        {
            sats.Add(satConn.sat);
        }
        return sats;
    }

    protected void UpdateVisibleSats()
    {
        satConns.Clear();
        linePool.ReleaseAll();
        Collider2D[] satColliders = Physics2D.OverlapCircleAll(rigidbody2D.position, circleCollider.radius, SATS_ONLY_MASK);
        foreach (var satCollider in satColliders)
        {
            RaycastHit2D hit = Physics2D.Linecast(rigidbody2D.position, satCollider.rigidbody2D.position, IGNORE_CITIES_AND_SHADOWS_MASK);
            if (hit.collider == satCollider)
            {
                var satConn = new SatConnection();
                satConn.sat = satCollider.GetComponent<Satellite>();
                satConn.bIsVisible = true;
                satConns.Add(satConn);
                RenderConnection(satConn.sat);
            }
        }
    }

    void RenderConnection(Satellite sat)
    {
        LineRenderer line = linePool.GetLine();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, sat.transform.position);
    }

    IEnumerator ConnectToCities()
    {
        int connections = 0;
        foreach (var city in cities)
        {
            if (city == this)
                continue;
            if (IsCityConnected(city))
            {
                CreditsController.AddCredits(CREDIT_PER_CONNECTED_CITY);
                connections++;
                yield return null;
            }
        }
        connectedCities = connections;
    }

    public bool IsCityConnected(City city)
    {
        List<Satellite> otherSats = city.GetConnectedSats();
        List<Satellite> mySats = GetConnectedSats();
        List<Satellite> closedSats = new List<Satellite>();

        for (int i = 0; i < mySats.Count; i++)
        {
            Satellite sat = mySats[i];
            if (sat.bIsPowered && otherSats.Contains(sat))
            {
                return true;
            }
            else
            {
                List<Satellite> connected = sat.GetConnectedSats();
                connected.RemoveAll(otherSat => closedSats.Contains(otherSat));
                mySats.AddRange(connected);
                closedSats.Add(sat);
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(.2f, 1, 0, .15f);
        if (satConns != null)
        {
            foreach (var satConn in satConns)
            {
                Gizmos.DrawLine(transform.position, satConn.sat.transform.position);
            }
        }
    }
}
