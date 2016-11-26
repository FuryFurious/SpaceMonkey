using UnityEngine;
using System.Collections;

public class Collecter : MonoBehaviour {

    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private float attractPower = 0.005f;

    [SerializeField]
    private bool attracks = true;

    void OnTriggerStay2D(Collider2D collider)
    {
        Banana b = collider.GetComponent<Banana>();

        if(b)
        {
            if (attracks)
            {
                Vector2 toCenter = gameObject.transform.position - b.gameObject.transform.position;
                toCenter.Normalize();

                b.myBody.AddForce(toCenter * attractPower);
            }

            else
            {
                player.AddPoint();
                b.PlayParticleSystem();
                Destroy(b.gameObject);
            }
        }

        else
        {
            PlanetSphere sphere = collider.GetComponent<PlanetSphere>();

            if(sphere)
            {
                
            }
        }
    }
}
