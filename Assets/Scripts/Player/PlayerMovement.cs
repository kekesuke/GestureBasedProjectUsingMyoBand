using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private float groundDrag;

    [Header("Ground Check")]
    public float playerHight;
    public LayerMask whatIsGround;
    private bool grounded;

    float horizontalInput;
    float verticalInput;
    private Rigidbody rb;
    private float radius = 10f;

    public Transform orientation;
    private Vector3 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        Inputs();
        SpeedLimit();
        grounded = Physics.CheckCapsule(transform.position, transform.position - new Vector3(0, (playerHight * 0.25f + 0.2f), 0), radius, whatIsGround);
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void Inputs()
    {
        horizontalInput = Input.GetAxis("Horizontal") * moveSpeed;
        verticalInput = Input.GetAxis("Vertical") * moveSpeed;

    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f);
    }

    private void SpeedLimit()
    {
        Vector3 flatVal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVal.magnitude > moveSpeed)
        {
            Vector3 limit = flatVal.normalized * moveSpeed;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }
    }

}
