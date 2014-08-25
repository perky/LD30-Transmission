using UnityEngine;
using System.Collections;

public class ConnectivityCalculator : MonoBehaviour
{
    public float connectivityPercent;
	
    void Update()
    {
        int numCities = City.cities.Length;
        int maxConnections = (numCities * numCities) - numCities;
        int connections = 0;
        foreach (var city in City.cities)
        {
            connections += city.connectedCities;
        }
        connectivityPercent = (float)connections / (float)maxConnections;
    }
}
