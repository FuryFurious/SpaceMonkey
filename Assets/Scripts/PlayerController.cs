using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private const float VELOCITY_TRIGGER = 7.5f;
    private const float ANGULAR_TRIGGER = 180.0f;

    private const float MIN_NUM_PARTICLES = 0.0f;
    private const float MAX_NUM_PARTICLES = 100.0f;

    [SerializeField]
    private AudioSource jetPackSource;
    [SerializeField]
    private AudioSource monkeySource;

    [SerializeField]
    private Camera myCam;
    [SerializeField]
    private ParticleSystem[] duseSystems;
    [SerializeField]
    private Rigidbody2D myBody;
    [SerializeField]
    private Animator myAnimator;
    [SerializeField]
    private GameObject headRoot;

    [SerializeField]
    private float soundChance = 0.2f;

    [SerializeField]
    private float minPitch = 0.9f;

    [SerializeField]
    private float maxPitch = 1.1f;

    [SerializeField]
    private float soundScale = 0.5f;

    [SerializeField]
    private float yVelocity = 1.0f;
    [SerializeField]
    private float xVelocity = 1.0f;
    [SerializeField]
    private float dashPower = 10.0f;
    [SerializeField]
    private float sizeChangeFactor = 0.5f;
    [SerializeField]
    private float minSize = 4.0f;
    [SerializeField]
    private float maxSize = 5.0f;
    [SerializeField]
    private float angularDecreaseFactor = 0.15f;

    [SerializeField]
    private float maxVelocity = 10.0f;

    private int numBanana = 0;

    private float speedPercent = 0.0f;
    //private bool isPushing = false;
    //private bool isSlowingDown = false;
    private float targetSize = 0.0f;
    private float helpSize;

    public Rigidbody2D GetRigidBody()
    {
        return myBody;
    }

    private float lastNumParticles = -1.0f;

    void FixedUpdate()
    {
        if (GameManager.Instance.gameIsRunning)
        {

            float xAxis = -Input.GetAxis("Horizontal");
            myBody.AddTorque(xAxis * xVelocity * Time.fixedDeltaTime, ForceMode2D.Force);

            float yAxis = Input.GetAxis("Vertical");

            if (yAxis != 0.0f)
            {
                float scaleFactor = 1.0f;

                float curVelo = Mathf.Sqrt(myBody.velocity.magnitude);

                if (curVelo > 1.0f)
                    scaleFactor = curVelo;

                Vector2 velocity = gameObject.transform.up * yAxis * yVelocity * scaleFactor;
                myBody.AddForce(velocity);
            }

            if (Input.GetButton("Jump")){

                float changeVeloPercent = 0.15f;
                Vector2 curVelo = myBody.velocity;
                Vector2 curVeloDir = curVelo.normalized;
                Vector2 newVelo = curVelo;

                newVelo = curVelo - curVeloDir * changeVeloPercent * Time.fixedDeltaTime;
                myBody.velocity = newVelo + dashPower * (Vector2)gameObject.transform.up * Time.fixedDeltaTime;
            }

            else
            {
                float decrease = -myBody.angularVelocity * angularDecreaseFactor * Time.fixedDeltaTime;
                myBody.angularVelocity = myBody.angularVelocity + decrease;
            }

        }

        //HERE:

        speedPercent = myBody.velocity.magnitude / VELOCITY_TRIGGER;
        myAnimator.SetFloat("speed", speedPercent);

        float angularDings = Mathf.Abs(myBody.angularVelocity) / ANGULAR_TRIGGER;
        myAnimator.SetFloat("angularVelocity", angularDings);


        float lerpValue = Mathf.Clamp(speedPercent, 0.0f, 1.0f);
        targetSize = Mathf.Lerp(minSize, maxSize, lerpValue);
        myCam.orthographicSize = Mathf.SmoothDamp(myCam.orthographicSize, targetSize, ref helpSize, Time.fixedDeltaTime * sizeChangeFactor);
        

        float curParticles = Mathf.Lerp(MIN_NUM_PARTICLES, MAX_NUM_PARTICLES, lerpValue);

        if(lastNumParticles != curParticles)
        {
            lastNumParticles = curParticles;

            for (int i = 0; i < duseSystems.Length; i++)
            {
                var emission = duseSystems[i].emission;
                emission.rate = curParticles;
            }
        }

        jetPackSource.volume = lerpValue * soundScale;

        float tmpVelo = myBody.velocity.magnitude;

        if (tmpVelo > maxVelocity)
            myBody.velocity = myBody.velocity.normalized * maxVelocity;

        if (Input.GetKeyDown(KeyCode.P))
            TryMonkeySound();
    }

    public void AddPoint()
    {
        numBanana++;

        if (numBanana == GameManager.Instance.GetNumBananas())
            GameManager.Instance.EndGame();

        UiManager.Instance.UpdatePoints(GameManager.Instance.GetNumBananas() - numBanana);

        TryMonkeySound();
    }

    private void TryMonkeySound()
    {
        if (!monkeySource.isPlaying && Random.value < soundChance)
        {
            monkeySource.pitch = minPitch + Random.value * (maxPitch - minPitch);
            monkeySource.Play();
        }
    }

    internal void Reset()
    {
        Vector3 camDelta = gameObject.transform.position - myCam.transform.position;
        camDelta.z = -10.0f;

        gameObject.transform.position = Vector2.zero;
        myCam.transform.position = camDelta;

        numBanana = 0;
    }

    public IEnumerator ResetVelocity()
    {
        float normalDrag  = myBody.drag;
        float normalAngularDrag = myBody.angularDrag;

        myBody.drag = 5.0f;
        myBody.angularDrag = 5.0f;

        yield return new WaitForSeconds(2.0f);


        myBody.drag = normalDrag;
        myBody.angularDrag = normalAngularDrag;
    }

    public void ResetDuse()
    {
        lastNumParticles = -1.0f;

        for (int i = 0; i < duseSystems.Length; i++)
        {
            var emission = duseSystems[i].emission;
            emission.rate = 0.0f;
        }
    }
}
