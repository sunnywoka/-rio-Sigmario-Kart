using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge_Basic_PlayerController : MonoBehaviour
{
    
    //Reference to the CharacterController component
    public CharacterController controller;

    //Reference to the Animator component
    public Animator animator;

    //Current Player Speed
    public float m_speed = 10;

    public float m_walkSpeed = 2;

    public float m_runSpeed = 6;

    //Vertical speed
    public float m_speedY;

    //Jump height
    public float m_jumpHeight = 10;

    //Used for more controlled jump
    public float m_jumpControlAmount = 2f;

    //Current Speed at which player smoothly rotates
    public float m_turnSpeed;

    //Movement direction of the player
    private Vector3 movement, movementJump;

    //Player input
    private Vector3 inputDirection;

    //Gravity
    private const float GRAVITY = 9.87f;

    //Gravity stick constant
    private const float GRAVITY_STICK = 0.3f;


    void Start()
    {
        //Fetch the CharacterController component 
        controller = GetComponent<CharacterController>();

        //Fetch the Animator component
        animator = GetComponent<Animator>();


    }

    void Update()
    {   


        //Left-Right, Forward-Backward movement
        UpdateHorizontalMovement();

        //Jump 
        UpdateVerticalMovement();

        //Rotate Face in the movement direction
        UpdateRotation();

        //Move in the given input direction and in the jump direction if there is any
        controller.Move(movement * Time.deltaTime + movementJump * Time.deltaTime);
    }


   

    //Updates the rotation of player to face in the movement direction
    private void UpdateRotation()
    {
        //If we have some input, then only rotate other wise preserve the rotation
        if (inputDirection != Vector3.zero)            
        {
           
            //Calcuate the angle required to rotate based on the given direction
            float turnAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            //Convert the angle into a Quaternion (Data Type used by many game engines for rotation)
            //It consists of 4 dimensions: 3 imaginary numbers (x , y , z) and a real number (w) *Notations can differ
            Quaternion targetRotation = Quaternion.Euler(0 , turnAngle, 0);

            //Smoothly Interpolate from current rotation to target rotation with the given turn speed amount 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_turnSpeed * Time.deltaTime);      
                    
        }
    }

    //Updates the L-R-F-B movement
    private void UpdateHorizontalMovement()
    {
       
        //Get the input and normalize
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;
   
        //We want to preserve and modify the movement.y value when there is jump input, 
        //hence we only assign individual xz components
        //movement.x = inputDirection.x * m_speed;
        //movement.z = inputDirection.z * m_speed;

        //If on ground (so speed stays same when jumping) 
        //And there is some input,
        if(controller.isGrounded && inputDirection != Vector3.zero)
        {
            //check if we have walk input
            if(Input.GetKey(KeyCode.LeftShift))
            {
                m_speed = m_walkSpeed;
                animator.SetBool("walk", true);
       
            }

            //Run 
            else
            {
                animator.SetBool("walk", false);
                m_speed = m_runSpeed;
            }
        }

        if(inputDirection == Vector3.zero)
        {
            m_speed = 0;
        }

        //With this previous code, our movement.y will always be 0
        movement = inputDirection.normalized * m_speed;

        

        //Play the movement animation
        //This will take care of both the idle and run animations since for idle speed will 0 when there is no input
        animator.SetFloat("speed", m_speed);
       
    }


    private void UpdateVerticalMovement()
    {
        //Player is on ground, then only check for jump input
        if(controller.isGrounded)
        {
            //Common hack used to make sure the player is always sticked to the ground when on ground
            //Sometimes, isgrounded does not work correctly and the player keeps floating in air
           m_speedY = -GRAVITY * GRAVITY_STICK;
        
           animator.SetBool("jump", false);

            //If we press space and not already jumping, then jump
            if(Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("jump")) 
            {
                //Simple jump
                m_speedY = m_jumpHeight;

                //Speed stays same during all jumps
                m_speed = m_runSpeed;
                
                //More Controlled jump (proportionate to our defined gravity value)
                //movement.y += Mathf.Sqrt(m_jumpHeight * m_jumpControlAmount * GRAVITY);

                animator.SetBool("jump", true);

       
            }

             
        }
        //Player is airborne
        else
        {   
            //Apply gravity so that the player falls back to the ground
           m_speedY -= GRAVITY * Time.deltaTime;
           
        }      

        movementJump.y = m_speedY;
        
    }



}
