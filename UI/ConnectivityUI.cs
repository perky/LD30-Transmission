using UnityEngine;
using System.Collections;

public class ConnectivityUI : DynamicTextUI 
{
    public ConnectivityCalculator connectivityCalculator;
    public float smoothing = 0.1f;

    private float connectivity;

    void Update()
    {
        connectivity = Mathf.Lerp(connectivity, connectivityCalculator.connectivityPercent, smoothing);
        string conn = string.Format("{0:00} percent", connectivity * 100.0f);
        textRenderer.text = conn;
    }
}
