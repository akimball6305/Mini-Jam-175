using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Shooting shootingScript;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;
    public float airResistance;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public float maxVelocity;
    [SerializeField] private float boostCooldown = 2f; // Cooldown time in seconds
    private float lastBoostTime = -Mathf.Infinity;

    Animator playerAnimator;

    [SerializeField] public float walkSpeed;
    [SerializeField] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject shootingObject;
    Animator movingAnimator;

    private void Start()
    {
        if (shootingObject != null)
        {
            shootingScript = shootingObject.GetComponent<Shooting>();
        }

        if (shootingScript == null)
        {
            Debug.LogError("Shooting script not found on the assigned shootingObject!");
        }

        movingAnimator = Player.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
        Vector3 airResistanceForce = -rb.linearVelocity * airResistance;
        rb.AddForce(airResistanceForce);

        if (Input.GetKeyDown(jumpKey) && rb.linearVelocity.y < 0 && Time.time >= lastBoostTime + boostCooldown)
        {
            float airSpeed = moveSpeed * airMultiplier;
            Debug.Log("boosted");
            Vector3 boostDirection = moveDirection.normalized * airSpeed * 10f; //+ Vector3.down; // Add slight downward force to emphasize falling
            rb.AddForce(boostDirection * jumpForce * 2f, ForceMode.Acceleration);

            lastBoostTime = Time.time;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        bool isShooting = shootingScript.IsShooting;
        bool isReloading = shootingScript.IsReloading;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
          /*  if ((horizontalInput != 0 || verticalInput != 0) && !isShooting && !isReloading)
            {
                movingAnimator.SetBool("isWalking", true);
            }
            else
            {
                movingAnimator.SetBool("isWalking", false);
            }
            */
        }

        // in air
        else if (!grounded)
        {
            float airSpeed = moveSpeed * airMultiplier;

            rb.AddForce(moveDirection.normalized * airSpeed * 10f, ForceMode.Force);
            /*  // Check if the player is moving in air
              if ((horizontalInput != 0 || verticalInput != 0) && !isShooting && !isReloading)
              {
                  movingAnimator.SetBool("isFalling", true);
              }
              else
              {
                  movingAnimator.SetBool("isFalling", false);
              } */
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}