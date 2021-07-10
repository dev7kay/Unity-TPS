using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private PlayerShooter playerShooter;
    private Animator animator;

    private Camera followCam;

    public float speed = 6f;
    public float jumpVelocity = 20f;
    [Range(0.01f, 1f)] public float airControlPercent;

    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.1f;

    private float speedSmoothVelocity;
    private float turnSmoothVelocity;

    private float currentVelocity;

    public float currentSpeed =>
        new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerShooter = GetComponent<PlayerShooter>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        followCam = Camera.main;
    }

    private void FixedUpdate()
    {
        if(currentSpeed > 0.2f || playerInput.fire || playerShooter.aimState == PlayerShooter.AimState.HipFire) Rotate();

        Move(playerInput.moveInput);

        if(playerInput.jump) Jump();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation(playerInput.moveInput);
    }

    public void Move(Vector2 moveInput)
    {
         var targetSpeed = speed * moveInput.magnitude;
         var moveDriection = Vector3.Normalize(transform.forward * moveInput.y + transform.right * moveInput.x);

         var smoothTime = characterController.isGrounded? speedSmoothTime : speedSmoothTime / airControlPercent;

         targetSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

         currentVelocity += Time.deltaTime * Physics.gravity.y;

         var veclocity = moveDriection * targetSpeed + Vector3.up * currentVelocity;

         characterController.Move(veclocity * Time.deltaTime);

         if(characterController.isGrounded) currentVelocity = 0f;
    }

    public void Rotate()
    {
        var targetRotation = followCam.transform.eulerAngles.y;

        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.eulerAngles = Vector3.up * targetRotation;
    }

    public void UpdateAnimation(Vector3 moveInput)
    {
        var animationSpeedPercent = currentSpeed / speed;
        animator.SetFloat("Vertical Move", moveInput.y * animationSpeedPercent, 0.05f, Time.deltaTime);
        animator.SetFloat("Horizontal Move", moveInput.x * animationSpeedPercent, 0.05f, Time.deltaTime);
    }

    public void Jump()
    {
        if(!characterController.isGrounded) return;
        currentVelocity = jumpVelocity;
    }
}
