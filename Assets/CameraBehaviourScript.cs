using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviourScript : MonoBehaviour
{
    public Transform target;
    public float distance = 10f;
    public float sensitivity = 5f;
    public float maxYAngle = 80f;
    public float minYAngle = 0f;
    public float maxDistance = 20f;
    public float minDistance = 2f;
    public float minDistanceFromPlayer = 2f;
    private float yaw = 0f;
    private float pitch = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Mouse.current.scroll.y.ReadValue() != 0)
        {
            float scrollAmount = Mouse.current.scroll.y.ReadValue();
            distance -= scrollAmount;
            distance = Mathf.Clamp(distance, 2f, 20f);
        }

        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            yaw += mouseDelta.x * sensitivity * Time.deltaTime;
            pitch -= mouseDelta.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            yaw += mouseDelta.x * sensitivity * Time.deltaTime;
            pitch -= mouseDelta.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);
        }
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 position = target.position - rotation * Vector3.forward * distance;
        transform.rotation = rotation;
        transform.position = position;

        Vector3 direction = (transform.position - target.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(target.position, direction, out hit, distance))
        {
            float distance2 = Mathf.Max(hit.distance, minDistanceFromPlayer);
            transform.position = target.position + direction * distance2 + Vector3.up * 0.5f;
        }
        else
        {
            transform.position = transform.position;
        }

        transform.LookAt(target.position + Vector3.up);
    }
}

