using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public float jumpPower = 15;
    // Start is called before the first frame update
    void Start()
    {
        rb = rb.GetComponent<Rigidbody>();
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
        
        
        if (Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            rb.AddForce(direction + new Vector3(0, jumpPower, 0), ForceMode.Impulse);

        } else if (Input.GetButtonDown("Jump") && doubleJumpAvailable && !canJump && !IsGrounded)
        {
            rb.AddForce(direction + new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }

        if (Input.GetButton("Run"))
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
        }
    }

    public bool IsGrounded => 
        Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, groundCheckDistance);
}
