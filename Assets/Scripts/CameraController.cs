using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] cameraTransforms;
    private int currentPosition = 0;
    private Vector3 cameraForward;

    public float moveSpeed = 50.0f;


    private void CameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Passage à la caméra suivante.
            currentPosition = (currentPosition + 1) % cameraTransforms.Length;
            Transform targetTransform = cameraTransforms[currentPosition];

            // Remove the camera as a child of the previous targetTransform
            if (currentPosition > 0)
            {
                transform.SetParent(null);
            }

            // Set the camera's position and rotation
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Retour à la caméra précédente.
            currentPosition = (currentPosition - 1 + cameraTransforms.Length) % cameraTransforms.Length;
            Transform targetTransform = cameraTransforms[currentPosition];

            // Remove the camera as a child of the previous targetTransform
            if (currentPosition > 0)
            {
                transform.SetParent(null);
            }

            // Set the camera's position and rotation
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }

        // Check if currentPosition == 0
        if (currentPosition == 0)
        {
            // Set the camera as a child of cameraTransforms[0]
            transform.SetParent(cameraTransforms[0]);
        }

        // Check if currentPosition == 1
        if (currentPosition == 1)
        {
            // orthographic cam
            // size is 50
            Camera.main.orthographic = true;
            Camera.main.orthographicSize = 50;

        }
        else
        {
            // perspective cam
            Camera.main.orthographic = false;
        }
    }


    private void CameraMovement()
    {
        // Check if currentPosition == 0
        if (currentPosition == 0)
        {
            // Get the camera's forward, right, and up vectors
            Vector3 cameraForward = transform.forward;
            Vector3 cameraRight = transform.right;
            Vector3 cameraUp = transform.up;
            cameraForward.Normalize();
            cameraRight.Normalize();
            cameraUp.Normalize();

            // Calculate the movement vectors
            Vector3 movement = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movement += cameraForward;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += cameraRight;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement -= cameraForward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movement -= cameraRight;
            }

            // Normalize the combined movement vector
            movement.Normalize();

            // Modify the parent's position (cameraTransforms[0])
            cameraTransforms[0].position += movement * Time.deltaTime * moveSpeed;
        }
    }

    private void CameraMouse()
    {

        
    }

    private void Start()
    {
        currentPosition = 0;
        CameraPosition();
    }

    void Update()
    {
        // Appel de la fonction CameraPosition.
        CameraPosition();
        CameraMovement();

        
    }
}