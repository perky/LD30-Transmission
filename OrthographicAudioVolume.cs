using UnityEngine;
using System.Collections;

public class OrthographicAudioVolume : MonoBehaviour
{
    public float maxVolume;
    public float minOrthoSize;
    public float maxOrthoSize;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float orthoSize = Camera.main.orthographicSize;
        float modifier = 1.0f - ((orthoSize - minOrthoSize) / (maxOrthoSize - minOrthoSize));
        modifier = Mathf.Clamp(modifier, 0, 1);
        audioSource.volume = maxVolume * modifier;
    }
}

