using UnityEngine;
using System.Collections;

public class ShadowEntryNotifier : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == Satellite.LAYER)
        {
            other.SendMessage("OnEnterShadow", SendMessageOptions.RequireReceiver);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Satellite.LAYER)
        {
            other.SendMessage("OnExitShadow", SendMessageOptions.RequireReceiver);
        }
    }
}
