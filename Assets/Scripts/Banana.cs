using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Banana : MonoBehaviour 
{
    [SerializeField]
    private ParticleSystem destroyParticleSystem;

    public Rigidbody2D myBody;

    [SerializeField]
    private SpriteRenderer bananaDingPrefab;

    private SpriteRenderer bananaDingInstance;

    void Start()
    {
        bananaDingInstance = Instantiate(bananaDingPrefab.gameObject).GetComponent<SpriteRenderer>();
        bananaDingInstance.enabled = false;
        StickToCam();
    }

    void OnDestroy()
    {
        Destroy(bananaDingInstance);

        GameManager.Instance.PlayPickup();
    }

    void Update()
    {
        if(GameManager.Instance.gameIsRunning && !bananaDingInstance.enabled)
        {
            bananaDingInstance.enabled = true;
        }

        if(bananaDingInstance.gameObject.activeSelf)
        {
            Vector2 pos = StickToCam();

            float dist = (new Vector2(0.5f, 0.5f) - pos).magnitude;
            Color color = bananaDingInstance.color;
            color.a = dist * 0.85f;
            bananaDingInstance.color = color;
        }
    }

    Vector2 StickToCam()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        bananaDingInstance.transform.position = Camera.main.ViewportToWorldPoint(pos);

        return pos;
    }

    public void PlayParticleSystem()
    {
        destroyParticleSystem.transform.SetParent(null, true);
        destroyParticleSystem.gameObject.SetActive(true);

        Destroy(destroyParticleSystem.gameObject, 2.0f);
    }

}
