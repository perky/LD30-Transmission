using UnityEngine;
using System.Collections;

public class CreditsUI : DynamicTextUI
{
    void Update()
    {
        textRenderer.text = "$" + CreditsController.Credits.ToString();
    }
}

