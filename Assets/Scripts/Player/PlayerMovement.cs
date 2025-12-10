using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private CharacterController charController;

    [Header("Variables")]
    [SerializeField] private Vector3 groundMovementInput;
    [SerializeField] private Vector3 yVelocity;
    [SerializeField] private bool isGrounded;
    public bool isMovementLocked = false;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        if (controller == null)
        {
            Debug.LogError("PlayerController don't have been found!");
        }

        charController = controller.characterController;
    }

    private void Update()
    {
        if (!controller.isAlive) return;

        HandleMovement();
        HandleHeadBob();
    }

    private void HandleMovement()
    {
        if(isMovementLocked)
        {
            charController.Move(Vector3.zero);
            return;
        }

        float gravity = controller.playerData.gravity;
        float speed = controller.playerData.movementSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && yVelocity.y < 0) yVelocity.y = -2f;

        float xMovimentInput = Input.GetAxis("Horizontal");
        float zMovimentInput = Input.GetAxis("Vertical");

        groundMovementInput = transform.right * xMovimentInput + transform.forward * zMovimentInput;

        yVelocity.y += gravity * Time.deltaTime;

        charController.Move(groundMovementInput * speed * Time.deltaTime);
        charController.Move(yVelocity * Time.deltaTime);
    }

    private void HandleHeadBob()
    {
        if (controller.cameraAnimator == null) return;

        if(!isMovementLocked)
        {
            bool moving = groundMovementInput.magnitude > 0.1f;
            controller.cameraAnimator.SetBool("isWalking", moving);
        }
        else
        {
            controller.cameraAnimator.SetBool("isWalking", false);
        }

        
    }
}
