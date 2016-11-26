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
        Banana banana = collider.GetComponent<Banana>();

        if(banana)
        {
            if (attracks)
            {
                Vector2 toCenter = gameObject.transform.position - banana.gameObject.transform.position;
                toCenter.Normalize();

                banana.myBody.AddForce(toCenter * attractPower);
            }

            else
            {
                player.AddPoint();
                banana.PlayParticleSystem();
                Destroy(banana.gameObject);
            }
        }

        else if(attracks)
        {
            PlanetSphere planet = collider.GetComponent<PlanetSphere>();

            if(planet)
            {
                Vector2 toPlanet = planet.gameObject.transform.position - player.transform.position;
                float increaseFactor = 1.0f / Mathf.Sqrt(toPlanet.magnitude);

                toPlanet.Normalize();

                player.GetRigidBody().AddForce(toPlanet * (planet.GetPullForce() + increaseFactor));
            }
        }
    }
}
