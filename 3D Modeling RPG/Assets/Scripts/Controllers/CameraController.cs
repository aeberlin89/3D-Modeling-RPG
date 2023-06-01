using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public Vector3 offset;

    public float pitch = 2f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float yawSpeed = 100f;

    private float currentZoom = 10f;
    private float currentYaw = 0f;

    private void Update()
    {
        currentZoom -= (float)Mouse.current.scroll.ReadValue().y * zoomSpeed;
        //currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        float yawValue = 0;
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            yawValue = -1f;
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            yawValue = 1f;
        }

        //currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        currentYaw -= yawValue * yawSpeed * Time.deltaTime;

    }

    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}
