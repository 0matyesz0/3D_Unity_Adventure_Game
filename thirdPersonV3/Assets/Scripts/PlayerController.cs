using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 6;
    public float gravity = -13;
    public float turnSmoothTime = 0.2f;
    public float speedSmoothTime = 0.1f;
    public float jumpHeight = 1;
    [Range(0,1)]
    public float airControlPercent = 0.25f;

    float turnSmoothVelocity;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Animator animator;
    Transform cameraTransform;
    Vector3 originScale;
    CharacterController charController;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        charController = GetComponent<CharacterController>();

        // set the scale of our gameobject
        originScale = transform.localScale;
        originScale.y = 1.7f;   
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        float targetSpeed = runSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
        Debug.LogWarning(targetSpeed);

        // movement
        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        charController.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(charController.velocity.x, charController.velocity.z).magnitude; // our speed will depend on the vectors of the controller, meaning if it hits a wall then the magnitude of the vector will be zero, therefore the speed will be zero.

        if (charController.isGrounded)
            velocityY = 0;

        float animationSpeedPercent = currentSpeed / runSpeed;
        animator.SetFloat("speedpercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void Jump()
    {
        if(charController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if(charController.isGrounded)
            return smoothTime;
        if (airControlPercent == 0)
            return float.MaxValue;

        // the less the airControlPercentage the less control we have over the character while jumping.
        return smoothTime / airControlPercent;
    }
}
