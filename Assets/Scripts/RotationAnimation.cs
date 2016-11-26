using UnityEngine;
using System.Collections;

public class RotationAnimation : MonoBehaviour {

    [SerializeField]
    private float minSpeed = 0.0f;
    [SerializeField]
    private float maxSpeed = 45.0f;

    [SerializeField]
    private Rigidbody2D myBody;
	
    void Start()
    {
        myBody.angularVelocity = Random.Range(minSpeed, maxSpeed);
    }
}
