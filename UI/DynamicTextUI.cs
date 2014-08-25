using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DynamicTextUI : MonoBehaviour
{
    protected Text textRenderer;
    
    void Awake()
    {
        textRenderer = GetComponent<Text>();
    }
}

