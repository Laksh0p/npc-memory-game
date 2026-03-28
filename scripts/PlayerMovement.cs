using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
public float speed = 5f;
public float mouseSensitivity = 100f;
public Transform cameraTransform;
private CharacterController controller;
private float xRotation = 0f;

private float gravity = -9.8f;
private float yVelocity = 0f;

void Start()
{
    controller = GetComponent<CharacterController>();
    Cursor.lockState = CursorLockMode.Locked;
    controller = GetComponent<CharacterController>();

if (controller == null)
{
    Debug.LogError("NO CONTROLLER FOUND");
}
else
{
    Debug.Log("Controller OK");
}
}

void Update()
{
    // ================= MOUSE LOOK =================
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -80f, 80f);

    cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    transform.Rotate(Vector3.up * mouseX);

    // ================= MOVEMENT (RAW INPUT FIX) =================
    float x = 0f;
    float z = 0f;

    if (Input.GetKey(KeyCode.W)) z = 1f;
    if (Input.GetKey(KeyCode.S)) z = -1f;
    if (Input.GetKey(KeyCode.A)) x = -1f;
    if (Input.GetKey(KeyCode.D)) x = 1f;

    Vector3 move = transform.right * x + transform.forward * z;

    // ================= GRAVITY =================
    if (controller.isGrounded && yVelocity < 0)
    {
        yVelocity = -2f;
    }

    yVelocity += gravity * Time.deltaTime;

    Vector3 velocity = move * speed;
    velocity.y = yVelocity;

    controller.Move(velocity * Time.deltaTime);

    // ================= INTERACTION =================
    if (Input.GetKeyDown(KeyCode.E))
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            Door door = hit.transform.GetComponentInParent<Door>();
            if (door != null)
            {
                door.ToggleDoor();
            }
        }
    }
}
}
