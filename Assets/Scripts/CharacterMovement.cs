using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // rigidbody, im gonna rigid your body oooo that line was cold 🧊🧊
    [SerializeField] private Rigidbody rb;
    // for the isGrounded func
    [SerializeField] private float groundCheckDistance = 1.1f;
    // for turning the cahracter n stuff
    [SerializeField] private float turningSpeed = 6f;
    [SerializeField] private float turningTime = 0.1f;
    // speed related variables
    public float speed;
    public float acceleration = 2f;
    public float sprintSpeed;
    // jump related variables
    private bool canJump = true;
    private bool doubleJumpAvailable = true;
    private bool isJumpCharging = true;
    public float jumpPower = 50;
    public float jumpCharge = 0;
    public float maxJumpCharge = 5;

    [SerializeField] CharacterAnimator characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        rb = rb.GetComponent<Rigidbody>();
        characterAnimator = rb.GetComponent<CharacterAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, 0).normalized;
    
        if (direction.magnitude > 0.1f)
        {
            rb.freezeRotation = true;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(rb.transform.eulerAngles.y, targetAngle, ref turningSpeed, turningTime);

            rb.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        } else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, acceleration * Time.deltaTime);
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            jumpCharge = 0.0f;
            isJumpCharging = true;
        }

        if (Input.GetButton("Jump"))
        {
            jumpCharge += Time.deltaTime;
            jumpCharge = Mathf.Clamp(jumpCharge, 0, maxJumpCharge);
        }

        if (Input.GetButtonUp("Jump") && IsGrounded)
        {
            characterAnimator.TriggerAnimation("DoJump");

            float jumpImpulse = (jumpCharge / maxJumpCharge) * jumpPower + (jumpPower / 2);
            rb.AddForce(direction + new Vector3(0, jumpImpulse, 0), ForceMode.Impulse);
        }

        if (Input.GetButton("Run") && IsGrounded)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, direction * sprintSpeed, acceleration * Time.deltaTime);
        } else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, direction * speed, acceleration * Time.deltaTime);
        }

        if (IsGrounded)
        {
            canJump = true;
            doubleJumpAvailable = true;
        } else
        {
            if (doubleJumpAvailable && Input.GetButtonDown("Jump"))
            {
                characterAnimator.TriggerAnimation("DoJump");
                rb.AddForce(direction + new Vector3(0, jumpPower / 2, 0), ForceMode.Impulse);
                doubleJumpAvailable = false;
            }
        }
    }

    private void FixedUpdate() {
        if (!IsGrounded)
        {
            applyGravity();
        }
    }


    public bool IsGrounded => 
        Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, groundCheckDistance);

    public void applyGravity()
    {
        rb.velocity += Vector3.up * Physics.gravity.y * (2f - 1) * Time.fixedDeltaTime;
    }

}
