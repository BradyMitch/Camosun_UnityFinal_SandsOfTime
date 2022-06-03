using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] private float speed = 9f;
    [SerializeField] private CharacterController cc;

    private float gravity;
    private float yVelocity = 0f;
    private float yVelocityOnGround = -4f;

    private float jumpHeight = 2f;
    private float jumpTime = 0.5f;
    private float initialJumpVelocity;

    private float pushForce = 5.0f;

    void Start()
    {
        float timeToApex = jumpTime / 2f;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);

        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //global to local coordinated
        movement = transform.TransformDirection(movement);
        //fix diagonal movement
        movement = Vector3.ClampMagnitude(movement, 1f);
        //Add speed before gravity
        movement *= speed;
        //add gravity
        yVelocity += gravity * Time.deltaTime;

        if (cc.isGrounded && yVelocity < 0.0)
        {
            yVelocity = yVelocityOnGround;
        }

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            yVelocity = initialJumpVelocity;
        }

        movement.y = yVelocity;
        cc.Move(movement * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        // does it have a rigidbody and is Physics enabled?
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}
