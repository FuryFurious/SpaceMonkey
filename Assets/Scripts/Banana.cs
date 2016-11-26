using UnityEngine;
using System.Collections;

public class Banana : MonoBehaviour 
{
    [SerializeField]
    private ParticleSystem destroyParticleSystem;

    public Rigidbody2D myBody;

    public void PlayParticleSystem()
    {
        destroyParticleSystem.transform.SetParent(null, true);
        destroyParticleSystem.gameObject.SetActive(true);

        Destroy(destroyParticleSystem.gameObject, 2.0f);
    }

}
