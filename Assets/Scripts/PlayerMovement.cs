using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
 [Header("Chodzonko")]
    private float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;

    public Transform orientation;
    
    private float horizontaInput;
    private float verticalInput;

    private Vector3 moveDir;
    private Rigidbody rb;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;
    public float groundDrag;

    [Header("Skakanko")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Kucanko")]
    public float crouchSpeed;
    public float crouchScale;
    private float startScale;
   
    [Header("Ślizganko")]

    public float slideDuration;
    public float slideSpeed;
    private float slideTimer;
    public float slideCooldown;
    private bool readyToSlide = true;


    [Header("Klawisze")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.C;
    public KeyCode slideKey = KeyCode.B;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air,
        crouching,
        sliding
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startScale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MoveInput();
        SpeedControl();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MoveInput()
    {
        horizontaInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 6f, ForceMode.Impulse);
        }

        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startScale, transform.localScale.z);
        }
        if(Input.GetKey(slideKey) && readyToSlide && grounded)
        {
            readyToSlide = false;
            StartSliding();
            Invoke(nameof(ResetSlide), slideCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontaInput;

        if(grounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void StateHandler()
    {
        
        if (grounded && Input.GetKey(sprintKey))
        {
            StartSprinting();
        }

        else if (grounded && Input.GetKey(crouchKey))
        {
            StartCrouching();
        }
        else if (grounded && Input.GetKey(slideKey))
        {
            state = MovementState.sliding;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
        
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    private void StartSprinting()
    {
        state = MovementState.sprinting;
        moveSpeed = sprintSpeed;
    }
    private void StartCrouching()
    {
        state = MovementState.crouching;
        moveSpeed = crouchSpeed;
    }

    private void StartSliding()
    {
        state = MovementState.sliding;
        moveSpeed = slideSpeed;
    }

    private void ResetSlide()
    {
        readyToSlide = true;
    }
}
