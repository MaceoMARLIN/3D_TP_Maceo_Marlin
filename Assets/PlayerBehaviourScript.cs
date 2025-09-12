using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBehaviourScript : MonoBehaviour
{
    public float speedOnGround = 5f;
    public float speedInAir = 2f;
    public float jumpForce = 5f;
    public float rotationSpeed = 3f;

    private Rigidbody rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;
    private float currentSpeed;

    public float health = 100f;
    public Slider healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        currentSpeed = isGrounded ? speedOnGround : speedInAir;

        float speedModifier = moveInput.magnitude > 0 ? 1f : 0f;
        animator.SetFloat("Speed", speedModifier, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        healthBar.value = health;
        healthBar.maxValue = 100f;
        if (health <= 0)
        {
            Debug.Log("Player is dead!");
        }
    }

    void FixedUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        if (Mouse.current.rightButton.isPressed)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), rotationSpeed * Time.fixedDeltaTime);
        }
        else if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 10f; 
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
