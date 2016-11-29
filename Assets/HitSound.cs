using UnityEngine;
using System.Collections;

public class HitSound : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.gameObject.tag == "Player")
        {
            GameManager.Instance.PlayHitSound();
        }
    }
}
