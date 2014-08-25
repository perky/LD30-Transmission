using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public float SlowMotionTimeScale;
    public float FastMotionTimeScale;

	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = SlowMotionTimeScale;
        } 
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = FastMotionTimeScale;
        } 
        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1;
        }
	}
}

