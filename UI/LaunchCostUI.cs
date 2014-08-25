using UnityEngine;
using System.Collections;

public class LaunchCostUI : DynamicTextUI
{
    void Update()
    {
	    if (SatelliteLauncher.bIsLaunching)
        {
            textRenderer.enabled = true;
            textRenderer.text = "$" + SatelliteLauncher.ActiveLauncher.LaunchCost().ToString();
            transform.position = Input.mousePosition;
        }
        else
        {
            textRenderer.enabled = false;
        }
    }
}

