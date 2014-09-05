using UnityEngine;
using System.Collections;

public class SatelliteLauncher : MonoBehaviour 
{
    public static bool bIsLaunching;
    public static SatelliteLauncher ActiveLauncher;

    public GameObject satellitePrefab;
    public float launchCostMultiplier = 100.0f;

    protected bool bIsActive;
    protected Ellipse orbit;
    protected float launchRadius;

    protected Satellite satellite;
    protected OrbitalMovement orbitalMovement;

    private Vector3 lastMousePosition;
    private AudioSource audio;

    void Awake()
    {
        var sAudioPrefab = Resources.Load("SatelliteLauncherAudio") as GameObject;
        var sAudio = Instantiate(sAudioPrefab) as GameObject;
        sAudio.transform.parent = transform;
        this.audio = sAudio.GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        bIsActive = true;
        SatelliteLauncher.bIsLaunching = true;
        SatelliteLauncher.ActiveLauncher = this;

        orbit = new Ellipse(transform.position.x, transform.position.y, 1, 1, 0);

        satellite = ((GameObject)Instantiate(satellitePrefab)).GetComponent<Satellite>();
        //satellite.enabled = false;
        satellite.collider2D.enabled = false;

        orbitalMovement = satellite.GetComponent<OrbitalMovement>();
        orbitalMovement.parent = transform;
        orbitalMovement.enabled = false;

        audio.Play();
    }

    void Update()
    {
        if (bIsActive)
        {
            Time.timeScale = 0;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
            launchRadius = Vector3.Distance(mousePos, transform.position);
            orbit.x = 0;
            orbit.y = 0;
            orbit.semiMajorAxis = launchRadius;
            orbit.semiMinorAxis = launchRadius;

            Vector3 dir = (mousePos - transform.position).normalized;
            orbit.angle = Mathf.Atan2(dir.y, dir.x);

            orbitalMovement.ellipse = orbit;
            orbitalMovement.DrawOrbit();
            satellite.transform.position = mousePos;

            if ((Input.mousePosition - lastMousePosition).sqrMagnitude > 0)
            {
                audio.mute = false;
            }
            else
            {
                audio.mute = true;
            }
            lastMousePosition = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
               Time.timeScale = 1;
               audio.Stop();
               Launch();
            }
        }
        //
    }

    protected void Launch()
    {
        if (CreditsController.SubtractCredits(LaunchCost()))
        {
            satellite.enabled = true;
            satellite.collider2D.enabled = true;
            orbitalMovement.enabled = true;
        }
        else
        {
            Destroy(satellite.gameObject);
        }

        bIsActive = false;
        SatelliteLauncher.bIsLaunching = false;
        SatelliteLauncher.ActiveLauncher = null;
    }

    public int LaunchCost()
    {
        return (int)((launchRadius * launchRadius) * launchCostMultiplier);
    }

    void OnDrawGizmos()
    {
        if (bIsActive)
        {
            orbit.DebugDraw(transform.position);
        }
    }
}
