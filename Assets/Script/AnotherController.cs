using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DriftDirection
{
    None,
    Left,
    Right,
}
public enum DriftLevel
{
    One,
    Two,
    Three
}

public class AnotherController : MonoBehaviour
{
    public Rigidbody rigidbody;
    GameObject player;
    Animator animator;
    public AudioSource driftAudio;// = new DriftAudio();
    public AudioSource engineRunningAudio;
    //public AudioSource engineReverseAudio;

    [Header("Input")]
    float forwardInput;
    float horizontalInput;

    [Header("Enter force")]
    public float currentForce;
    public float normalForce = 80;  //Normal force
    public float boostForce = 130;  //Boost force
    public float jumpForce = 10;    //Jump force 
    public float gravity = 40;      //gravity

    //Force direction
    Vector3 forceDirection;
    float verticalModified;         //Front and end of kart modefied coefficient

    [Header("Turning")]
    public bool isDrifting;
    public DriftDirection driftDirection = DriftDirection.None;
    [Tooltip("horizontalInput effection")]
    public Quaternion finalRotation;   //Final rotation
    public float turnSpeed = 60;

    //Drift()
    Quaternion driftOffset = Quaternion.identity;
    public DriftLevel driftLevel;

    [Header("Detect ground")]
    public Transform frontHitTrans;
    public Transform rearHitTrans;
    public bool isGround;
    public bool isGroundLastFrame;
    public float groundDistance = 0.7f;//Depend on different car model

    [Header("Particle")]
    public Transform driftParticle;
    public ParticleSystem[] wheelsParticeles;
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;
    [Header("Partical colour")]
    public Color[] driftColors;
    public float driftPower = 0;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        forceDirection = transform.forward;
        finalRotation = rigidbody.rotation;
        player = GameObject.Find("Player");
        animator = player.GetComponent<Animator>();
        //driftAudio = new DriftAudio();

        //The partical system of wheels when the kart is drifting
        wheelsParticeles = driftParticle.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        //Input
        forwardInput = Input.GetAxisRaw("Vertical");     //Forward
        horizontalInput = Input.GetAxisRaw("Horizontal");   //Vertical
        //Enter space key to jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)   //check if the kart is on ground
            {
                Jump();
            }
        }

        //Press space key and give a vertical input, the kart will drift
        if (Input.GetKey(KeyCode.Space) && horizontalInput != 0)
        {
            //The kart will drift at the moment when it touchs ground and is not drifting and its speed is bigger than certain amount
            if (isGround && !isGroundLastFrame && !isDrifting && rigidbody.velocity.sqrMagnitude > 10)
            {
                StartDrift();   //start drift
                
            }
        }

        //Release space key, the kart stop drifting
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isDrifting)
            {
                Boost(boostForce);//accelerate
                StopDrift();//Stop drifting
            }
        }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                animator.SetBool("TurningLeft", true);
                animator.SetBool("TurningRight", false);
                animator.SetBool("NotTurning", false);
            } else if (Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("TurningLeft", false);
                animator.SetBool("TurningRight", true);
                animator.SetBool("NotTurning", false);
            } else
            {
                animator.SetBool("TurningLeft", false);
                animator.SetBool("TurningRight", false);
                animator.SetBool("NotTurning", true);

            }
            

        
        if (!engineRunningAudio.isPlaying && currentForce > 0)
        {
            engineRunningAudio.Play();
        }

        //UpdateAudio();
    }

    

    private void FixedUpdate()
    {
        //The kart turing
        CheckGroundNormal();        //Detect if the kart is on ground and it is parallel with ground
        Turn();                     //Enter different keys to control the direction

        //Increasing the force when the kart starts running
        IncreaseForce();
        //when release the forward key or after the drifting accelerating, the force will reduce
        ReduceForce();


        //if the kart is drifting
        if (isDrifting)
        {
            CalculateDriftingLevel();   //calculate the drifting level
            ChangeDriftColor();         //change the colour depend on the drifting level
            if (!driftAudio.isPlaying)
            {
                driftAudio.Play();
            }
 
        }

            //Give the kart final rotation and accelerate base of above situation
            rigidbody.MoveRotation(finalRotation);
            //calculate the direction of force
            CalculateForceDir();
            //moving
            AddForceToMove();
        


    }

    //Calculate the accelerating force direction
    public void CalculateForceDir()
        {
            //Forward force
            if (forwardInput > 0)
            {
                verticalModified = 1;
            }
            else if (forwardInput < 0)//Reverse force
            {
                verticalModified = -1;
            }

            forceDirection = driftOffset * transform.forward;
        }

        //Accelerating 
        public void AddForceToMove()
        {
            //Calculate total force
            Vector3 tempForce = verticalModified * currentForce * forceDirection;

            if (!isGround)  //if the kart is not on ground, add the gravity on it.
            {
                tempForce = tempForce + gravity * Vector3.down;
            }

            rigidbody.AddForce(tempForce, ForceMode.Force);
        }

    //Detect if the kart is on ground and it is parallel with ground
        public void CheckGroundNormal()
        {
            //produce a raycast from the head of the kart to the ground
            RaycastHit frontHit;
            bool hasFrontHit = Physics.Raycast(frontHitTrans.position, -transform.up, out frontHit, groundDistance, LayerMask.GetMask("Ground"));
            if (hasFrontHit)
            {
                Debug.DrawLine(frontHitTrans.position, frontHitTrans.position - transform.up * groundDistance, Color.red);
            }
            //produce a raycast from the tail of the kart to the ground
            RaycastHit rearHit;
            bool hasRearHit = Physics.Raycast(rearHitTrans.position, -transform.up, out rearHit, groundDistance, LayerMask.GetMask("Ground"));
            if (hasRearHit)
            {
                Debug.DrawLine(rearHitTrans.position, rearHitTrans.position - transform.up * groundDistance, Color.red);
            }
            isGroundLastFrame = isGround;
            if (hasFrontHit || hasRearHit)//detect if the kart is on ground
            {
                isGround = true;
                animator.SetBool("Grounded", true);
            }
            else
            {
                isGround = false;
                animator.SetBool("Grounded", false);
            }

            //Make sure the kart is parallel with ground
            Vector3 tempNormal = (frontHit.normal + rearHit.normal).normalized;
            Quaternion quaternion = Quaternion.FromToRotation(transform.up, tempNormal);
            finalRotation = quaternion * finalRotation;
        }

        //Reduce the force
        public void ReduceForce()
        {
            float targetForce = currentForce;
            if (isGround && forwardInput == 0)
            {
                targetForce = 0;
            }
            else if (currentForce > normalForce)    //back to normal situation after accelerating
            {
                targetForce = normalForce;
            }

            if (currentForce <= normalForce)
            {
                DisableTrail();
            }
            currentForce = Mathf.MoveTowards(currentForce, targetForce, 60 * Time.fixedDeltaTime);//reduce 60 force per second
        }

        //Increase force
        public void IncreaseForce()
        {
            float targetForce = currentForce;
            if (forwardInput != 0 && currentForce < normalForce)
            {
                currentForce = Mathf.MoveTowards(currentForce, normalForce, 80 * Time.fixedDeltaTime);//increase 80 force per second
            }
        }

        public void Turn()
        {
            //the kart only can trun when it is moving
            if (rigidbody.velocity.sqrMagnitude <= 0.1)
            {
                return;
            }

            //the drifting will provide turning
            if (driftDirection == DriftDirection.Left)
            {
                finalRotation = finalRotation * Quaternion.Euler(0, -40 * Time.fixedDeltaTime, 0);
            }
            else if (driftDirection == DriftDirection.Right)
            {
                finalRotation = finalRotation * Quaternion.Euler(0, 40 * Time.fixedDeltaTime, 0);
            }

            //backward with opposite turing
            float modifiedSteering = Vector3.Dot(rigidbody.velocity, transform.forward) >= 0 ? horizontalInput : -horizontalInput;

            //the normal turing speed is 120, and will change to 60 when it is drifting
            turnSpeed = driftDirection != DriftDirection.None ? 60 : 120;
            float turnAngle = modifiedSteering * turnSpeed * Time.fixedDeltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0, turnAngle, 0);

            finalRotation = finalRotation * deltaRotation;//the kart can rotate in some coordinates
        }

        public void Jump()
        {
            rigidbody.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }


        //start drifting and decide the turning direction
        public void StartDrift()
        {
            Debug.Log("Start Drift");
            isDrifting = true;

            //decide the turning direction base on the vertical input
            if (horizontalInput < 0)
            {
                driftDirection = DriftDirection.Left;
                driftOffset = Quaternion.Euler(0f, 30, 0f);
            }
            else if (horizontalInput > 0)
            {
                driftDirection = DriftDirection.Right;
                driftOffset = Quaternion.Euler(0f, -30, 0f);
            }

            //display the partical
            PlayDriftParticle();
        }

        //calculate drifting level
        public void CalculateDriftingLevel()
        {
            driftPower += Time.fixedDeltaTime;
            //increase the drifting level in every 0.7 second
            if (driftPower < 0.7)
            {
                driftLevel = DriftLevel.One;
            }
            else if (driftPower < 1.4)
            {
                driftLevel = DriftLevel.Two;
            }
            else
            {
                driftLevel = DriftLevel.Three;
            }
        }


        //Stop drifting
        public void StopDrift()
        {
            isDrifting = false;
            driftDirection = DriftDirection.None;
            driftPower = 0;
            driftOffset = Quaternion.identity;
            StopDriftParticle();
        }

        //Accelerating
        public void Boost(float boostForce)
        {
            //Depend on the drifting level: 1, 1.1, 1.2
            currentForce = (1 + (int)driftLevel / 10) * boostForce;
            EnableTrail();
        }

        //play the partical
        public void PlayDriftParticle()
        {
            foreach (var tempParticle in wheelsParticeles)
            {
                tempParticle.Play();
            }
        }

        //change the partical colour when the level of drifting increasing
        public void ChangeDriftColor()
        {
            foreach (var tempParticle in wheelsParticeles)
            {
                var t = tempParticle.main;
                t.startColor = driftColors[(int)driftLevel];
            }
        }

        //stop playing partical
        public void StopDriftParticle()
        {
            foreach (var tempParticle in wheelsParticeles)
            {
                tempParticle.Stop();
            }
        }

        public void EnableTrail()
        {
            leftTrail.enabled = true;
            rightTrail.enabled = true;
        }

    public void DisableTrail()
    {
        leftTrail.enabled = false;
        rightTrail.enabled = false;
    }
}
