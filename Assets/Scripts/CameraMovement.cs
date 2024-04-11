using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    public float positionSpeed = 10.0f;
    public float zoomSpeed = 10.0f;

    private float maxPositionDelta = 30.0f;
    private float maxRotationDelta = 45.0f;

    private float currentRotationX = 0.0f;
    private float currentRotationY = 0.0f;

    private float currentPositionX = 0.0f;
    private float currentPositionY = 0.0f;

    private float maxZoomDelta = 30.0f;
    private float currentZoom = 0.0f;

    private void Start()
    {
        currentRotationX = transform.rotation.eulerAngles.x;
        currentRotationY = transform.rotation.eulerAngles.y;
        currentPositionX = transform.position.x;
        currentPositionY = transform.position.y;
        currentZoom = transform.position.z;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            float targetRotationX = currentRotationX + rotationSpeed * Time.deltaTime;

            targetRotationX = Mathf.Clamp(targetRotationX, currentRotationX - maxRotationDelta, currentRotationX + maxRotationDelta);

            currentRotationX = targetRotationX;
            transform.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            float targetRotationY = currentRotationY + rotationSpeed * Time.deltaTime;

            targetRotationY = Mathf.Clamp(targetRotationY, currentRotationY - maxRotationDelta, currentRotationY + maxRotationDelta);

            currentRotationY = targetRotationY;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotationY, transform.rotation.eulerAngles.z);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            float targetRotationX = currentRotationX - rotationSpeed * Time.deltaTime;

            targetRotationX = Mathf.Clamp(targetRotationX, currentRotationX - maxRotationDelta, currentRotationX + maxRotationDelta);

            currentRotationX = targetRotationX;
            transform.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            float targetRotationY = currentRotationY - rotationSpeed * Time.deltaTime;

            targetRotationY = Mathf.Clamp(targetRotationY, currentRotationY - maxRotationDelta, currentRotationY + maxRotationDelta);

            currentRotationY = targetRotationY;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotationY, transform.rotation.eulerAngles.z);
        }

        //--------------------------------------------------------------------

        if (Input.GetKey(KeyCode.L))
        {
            float targetPositionX = currentPositionX + positionSpeed*Time.deltaTime;

            targetPositionX = Mathf.Clamp(targetPositionX, currentPositionX - maxPositionDelta, currentPositionX +  maxPositionDelta);
            currentPositionX = targetPositionX;
            transform.position = new Vector3(currentPositionX, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.J))
        {
            float targetPositionX = currentPositionX - positionSpeed * Time.deltaTime;

            targetPositionX = Mathf.Clamp(targetPositionX, currentPositionX - maxPositionDelta, currentPositionX + maxPositionDelta);
            currentPositionX = targetPositionX;
            transform.position = new Vector3(currentPositionX, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.K))
        {
            float targetPositionY = currentPositionY - positionSpeed * Time.deltaTime;

            targetPositionY = Mathf.Clamp(targetPositionY, currentPositionY - maxPositionDelta, currentPositionY + maxPositionDelta);
            currentPositionY = targetPositionY;
            transform.position = new Vector3(transform.position.x, currentPositionY, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            float targetPositionY = currentPositionY + positionSpeed * Time.deltaTime;

            targetPositionY = Mathf.Clamp(targetPositionY, currentPositionY - maxPositionDelta, currentPositionY + maxPositionDelta);
            currentPositionY = targetPositionY;
            transform.position = new Vector3(transform.position.x, currentPositionY, transform.position.z);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

            float newZoomDistance = transform.position.z - (zoomDelta * zoomSpeed);

            newZoomDistance = Mathf.Clamp(newZoomDistance, currentZoom - maxZoomDelta, currentZoom + maxZoomDelta);

            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, newZoomDistance);
            transform.position = newPosition;
        }
    }
}
