using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private const float VELOCITY_TRIGGER = 4.0f;
    private const float ANGULAR_TRIGGER = 180.0f;

    private const float MIN_NUM_PARTICLES = 0.0f;
    private const float MAX_NUM_PARTICLES = 100.0f;
    
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
        float xAxis = -Input.GetAxis("Horizontal");
        myBody.AddTorque(xAxis * xVelocity * Time.fixedDeltaTime, ForceMode2D.Force);

        float yAxis = Input.GetAxis("Vertical");

        if (yAxis != 0.0f)
        {
            float scaleFactor = 1.0f;

            float curVelo = Mathf.Sqrt(myBody.velocity.magnitude);

            if(curVelo > 1.0f)
                scaleFactor = curVelo;

            Vector2 velocity = gameObject.transform.up * yAxis * yVelocity * scaleFactor;
            myBody.AddForce(velocity);
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

        if (Input.GetButton("Jump"))
            myBody.AddForce(dashPower * gameObject.transform.up);

        else
        {
            float decrease = - myBody.angularVelocity * angularDecreaseFactor * Time.fixedDeltaTime;
            myBody.angularVelocity = myBody.angularVelocity + decrease;
        }


        float tmpVelo = myBody.velocity.magnitude;

        if (tmpVelo > maxVelocity)
            myBody.velocity = myBody.velocity.normalized * maxVelocity;
    }

    /*
    private IEnumerator Push(float time, float power, float dir)
    {
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            myBody.AddForce(dir * power * myBody.transform.up * Time.deltaTime, ForceMode2D.Impulse);
            yield return new WaitForEndOfFrame();
        }

        if (dir >= 0.0f)
            isPushing = false;
        else
            isSlowingDown = false;
    }
    */

    public void AddPoint()
    {
        numBanana++;
        UiManager.Instance.UpdatePoints(numBanana);
    }
}