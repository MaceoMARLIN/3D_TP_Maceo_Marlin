using UnityEngine;

public class EnnemyBehaviourScript : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float rotationSpeed = 5f;
    private Rigidbody rb;
    private Animator animator;
    private bool isChasing;

    public float wanderRadius = 5f;
    public float wanderTimer = 3f;
    private float timer;
    private Vector3 targetPosition;

    public float health = 100f;
    public UnityEngine.UI.Slider healthBar;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();

        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        targetPosition = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
    }

    void Update()
    {
        healthBar.value = health;
        healthBar.maxValue = 100f;
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        else
        {
            Wander();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        transform.position += direction * speed * Time.deltaTime;

        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
            targetPosition = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);

            timer = 0;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction.magnitude > 0.9f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            transform.position += direction * (speed * 0.5f) * Time.deltaTime;
            animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
        }
    }
}
