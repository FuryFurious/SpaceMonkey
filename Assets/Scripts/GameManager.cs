using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

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

    [SerializeField]
    private int numBananas = 50;

    private List<GameObject> createdObjects = new List<GameObject>();

    public bool gameIsRunning = false;

    [SerializeField]
    private PlayerController player;

    public int GetNumBananas()
    {
        return numBananas;
    }

    public void StartGame()
    {
        Debug.Assert(createdObjects.Count != 0);

        gameIsRunning = true;
        UiManager.Instance.ShowInGame(true);
    }

    public void EndGame()
    {
        gameIsRunning = false;

        StartCoroutine(GameWonRoutine());
    }

    private void ResetGame()
    {
        player.Reset();

        CreateLevel();
    }

	private void Awake () 
    {
        Debug.Assert(!Instance);

        Instance = this;

        ResetGame();
	}
	
    private void CreateLevel()
    {
        StartCoroutine(CreateMeteoriteCluster());
    }

    private IEnumerator CreateMeteoriteCluster()
    {
        int bananaCount = 0;

        //create cluster:
        for (int i = 0; i < numCluster; i++)
        {
            Vector2 center = GetRandDir() * Random.Range(levelRadiusNoCluster, levelRadius);

            float radiusPercent = center.magnitude / levelRadius;

            if (Random.value < planetSpawnChance)
                CreatePlanetAt(center);

            else if(bananaCount < numBananas)
            {
                CreateBananaAt(center);
                bananaCount++;
            }

            int numColliders = (int)(radiusPercent * clusterSize);

            //create meteroids:
            for (int j = 0; j < numColliders; j++)
            {
                Vector2 colliderDir = GetRandDir(Random.Range(clusterRadiusMin, clusterRadiusMax) * (1.0f + radiusPercent));

                GameObject collider = Instantiate(RandomColliderPrefab());

                createdObjects.Add(collider);

                collider.transform.position = center + colliderDir;

                float scaleX = Random.Range(minScaleCollider, maxScaleCollider);
                float scaleY = Random.Range(minScaleCollider, maxScaleCollider);

                collider.transform.localScale = new Vector3(scaleX, scaleY, 1.0f);
                collider.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

                Rigidbody2D body = collider.GetComponent<Rigidbody2D>();

                body.mass = body.mass * scaleX * scaleY;

                yield return new WaitForEndOfFrame();
            }
        }


        int missingBananas = Mathf.Max(numBananas - bananaCount, 0);

        for (int i = 0; i < missingBananas; i++)
        {
            Debug.Log("created banana " + i);
            CreateBananaAt(GetRandDir(levelRadius));
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

        createdObjects.Add(p.gameObject);

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

    private IEnumerator GameWonRoutine()
    {
        StartCoroutine(player.ResetVelocity());

        for (int i = 0; i < createdObjects.Count; i++)
        {
            player.GetRigidBody().AddForce(-player.GetRigidBody().velocity * Time.deltaTime);

            Destroy(createdObjects[i]);
            yield return new WaitForEndOfFrame();
        }


        player.Reset();

        createdObjects.Clear();

        StartCoroutine(CreateMeteoriteCluster());

        yield return new WaitForSeconds(1.0f);

        UiManager.Instance.ShowInGame(false);
    }
}
