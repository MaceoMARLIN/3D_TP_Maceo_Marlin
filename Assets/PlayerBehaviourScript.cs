using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviourScript : MonoBehaviour
{
    private float speed;
    public float speedOnGround = 5f;
    public float speedInAir = 2f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    { 
        if (isGrounded != true)
        {
            speed = speedInAir;
        }
        else
        {
            speed = speedOnGround;
        }

        if (Mouse.current.rightButton.isPressed)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }

    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(transform.position + move * speed * Time.fixedDeltaTime);
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
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

