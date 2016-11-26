using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float speed = 2.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 toTarget = target.gameObject.transform.position - gameObject.transform.position;
        toTarget.z = 0.0f;

        gameObject.transform.position += toTarget * speed * Time.fixedDeltaTime;
	}
}
