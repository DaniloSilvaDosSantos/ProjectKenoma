using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform playerBody;

    [Header("Variables")]
    [SerializeField] private float xRotation = 0f;

    private void Start()
    {
        controller = GetComponentInParent<PlayerController>();

        if (controller == null)
        {
            Debug.Log("PlayerController don't have been found in a parent object!");
        }
        if (playerBody == null)
        {
            Debug.Log("Player Transform Reference don't have been assigned!");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!controller.isAlive) return;

        float mouseSensitivity = controller.playerData.mouseSensitivity;
        float minYAngle = controller.playerData.minLookAngle;
        float maxYAngle = controller.playerData.maxLookAngle;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (playerBody != null) playerBody.Rotate(Vector3.up * mouseX);
    }
}

