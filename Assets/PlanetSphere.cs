using UnityEngine;
using System.Collections;

public class PlanetSphere : MonoBehaviour 
{
    [SerializeField]
    private Planet planet;

    [SerializeField]
    private float pullFactor = 1.0f;

    public float GetPullForce()
    {
        return planet.transform.localScale.x * planet.transform.localScale.y * pullFactor;
    }
}
