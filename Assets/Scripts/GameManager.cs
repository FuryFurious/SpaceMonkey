using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private float levelRadius = 100.0f;
    [SerializeField]
    private float levelRadiusNoCluster = 10.0f;
    [SerializeField]
    private GameObject[] collisionPrefabs;
    [SerializeField]
    private float minScaleCollider = 0.75f;
    [SerializeField]
    private float maxScaleCollider = 2.0f;
    [SerializeField]
    private float clusterRadiusMin = 2.0f;
    [SerializeField]
    private float clusterRadiusMax = 10.0f;
    [SerializeField]
    private int clusterSize = 5;
    [SerializeField]
    private int numCluster = 100;

    [SerializeField]
    private Banana bananaPrefab;
    [SerializeField]
    private Planet planetPrefab;

    [SerializeField]
    private Color[] planetBaseColors;
    [SerializeField]
    private Color[] planetColorsOne;
    [SerializeField]
    private Color[] planetColorsTwo;
    [SerializeField]
    private Color[] planetColorShine;

    [SerializeField]
    private float planeSizeMin = 1.25f;
    [SerializeField]
    private float planetSizeMax = 4.0f;

    [SerializeField]
    private float planetSpawnChance = 0.15f;

	private void Awake () 
    {
        CreateLevel();
	}
	
    private void CreateLevel()
    {
        CreateMeteoriteCluster();
    }

    private void CreateMeteoriteCluster()
    {
        for (int i = 0; i < numCluster; i++)
        {
            Vector2 center = GetRandDir() * Random.Range(levelRadiusNoCluster, levelRadius);

            if (Random.value < planetSpawnChance)
                CreatePlanetAt(center);

            else
                CreateBananaAt(center);

            int numColliders = Random.Range(1, clusterSize);

            for (int j = 0; j < numColliders; j++)
            {
                Vector2 colliderDir = GetRandDir(Random.Range(clusterRadiusMin, clusterRadiusMax));

                GameObject collider = Instantiate(RandomColliderPrefab());
                collider.transform.position = center + colliderDir;

                float scaleX = Random.Range(minScaleCollider, maxScaleCollider);
                float scaleY = Random.Range(minScaleCollider, maxScaleCollider);

                collider.transform.localScale = new Vector3(scaleX, scaleY, 1.0f);
                collider.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

                Rigidbody2D body = collider.GetComponent<Rigidbody2D>();

                body.mass = body.mass * scaleX * scaleY;
            }
        }
    }

    private GameObject RandomColliderPrefab()
    {
        return collisionPrefabs[Random.Range(0, collisionPrefabs.Length)];
    }

    private Vector2 GetRandDir(float scale = 1.0f)
    {
        return GetDir(Random.value * Mathf.PI * 2.0f) * scale;
    }

    private Vector2 GetDir(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    private void CreateBananaAt(Vector3 pos)
    {
        GameObject bananaInstance = Instantiate(bananaPrefab.gameObject);
        bananaInstance.transform.position = pos;
        bananaInstance.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
    }

    private void CreatePlanetAt(Vector3 pos)
    {
        Planet p = Instantiate(planetPrefab.gameObject).GetComponent<Planet>();

        p.gameObject.transform.position = pos;
        p.gameObject.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        p.transform.localScale *= Random.Range(planeSizeMin, planetSizeMax);

        p.baseRenderer.color = planetBaseColors[Random.Range(0, planetBaseColors.Length)];
        p.rendererOne.color  = planetColorsOne[Random.Range(0, planetColorsOne.Length)];
        p.rendererTwo.color  = planetColorsTwo[Random.Range(0, planetColorsTwo.Length)];
        p.shineRenderer.color = planetColorShine[Random.Range(0, planetColorShine.Length)];

        p.rendererOne.gameObject.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        p.rendererOne.flipX = Random.value < 0.5f;
        p.rendererOne.flipY = Random.value < 0.5f;
    }
}
