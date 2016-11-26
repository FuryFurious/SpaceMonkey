using UnityEngine;
using System.Collections;

public class ScaleAnimation : MonoBehaviour {

    [SerializeField]
    private Vector3 min = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField]
    private Vector3 max = new Vector3(1.0f, 1.0f, 1.0f);

    [SerializeField]
    private float speed = 1.0f;

    private float time = 0.0f;


    void Start()
    {
        time = Random.Range(0.0f, Mathf.PI * 2.0f);
    }
	
	// Update is called once per frame
	void Update () 
    {
        time += Time.deltaTime * speed;

        float t = Mathf.Sin(time) * 0.5f + 0.5f;

        gameObject.transform.localScale = (1.0f - t) * min + t * max;
	}
}
