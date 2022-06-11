using UnityEngine;
using System.Collections;

public class Underwater : MonoBehaviour
{
    [SerializeField] GameObject[] water;

    // [SerializeField] Projector caustic;

    private bool isUnderwater;
    private Color normalColor;
    private Color underwaterColor;

    void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
    }

    void Update()
    {
        foreach (var body in water)
        {
            bool aboveFloor = 2 * body.transform.position.y - Terrain.activeTerrain.SampleHeight(transform.position) < transform.position.y;
            bool belowSurface = transform.position.y < body.transform.position.y;
            bool withinX = body.transform.position.x - body.GetComponent<Renderer>().bounds.size.x / 2 < transform.position.x
                        && body.transform.position.x + body.GetComponent<Renderer>().bounds.size.x / 2 > transform.position.x;
            bool withinZ = body.transform.position.z - body.GetComponent<Renderer>().bounds.size.z / 2 < transform.position.z
                        && body.transform.position.z + body.GetComponent<Renderer>().bounds.size.z / 2 > transform.position.z;

            if ((aboveFloor && belowSurface && withinX && withinZ) != isUnderwater)
            {
                isUnderwater = transform.position.y < body.transform.position.y;
                if (isUnderwater)
                {
                    SetUnderwater();
                }
                else
                {
                    SetNormal();
                }
            }
        }
    }

    void SetNormal()
    {
        RenderSettings.fogColor = normalColor;
        RenderSettings.fogDensity = 0f;
    }

    void SetUnderwater()
    {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = 0.05f;
    }
}