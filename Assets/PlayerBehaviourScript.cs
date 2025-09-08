using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviourScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speedOnGround = 5f;
    public float speedInAir = 2f;
    public float jumpForce = 5f;
    public float rotationSpeed = 3f;

    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Mettre à jour le speed selon le sol
        currentSpeed = isGrounded ? speedOnGround : speedInAir;

        // Mise à jour des paramètres Animator
        float speedModifier = moveInput.magnitude > 0 ? 1f : 0f;
        animator.SetFloat("Speed", speedModifier, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
    }

    void FixedUpdate()
    {
        // Calculer la direction relative à la caméra
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        // Rotation du personnage
        if (Mouse.current.rightButton.isPressed)
        {
            // Regarder la caméra
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), rotationSpeed * Time.fixedDeltaTime);
        }
        else if (moveDir.sqrMagnitude > 0.01f)
        {
            // Regarder dans la direction du mouvement
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Déplacement
        rb.MovePosition(rb.position + moveDir * currentSpeed * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("JumpTrigger");
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
