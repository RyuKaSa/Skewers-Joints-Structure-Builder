using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Camera sceneCamera;

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position on the screen
        Vector3 screenMousePosition = Input.mousePosition;

        // Create a ray from the camera through the mouse position
        Ray ray = sceneCamera.ScreenPointToRay(screenMousePosition);

        // Create a RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // A hit occurred, you can access hit information here
            Debug.Log("Hit object name: " + hit.transform.name);
        }
        else
        {
            // No hit occurred
        }

        // Optionally, you can draw the ray for debugging
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
    }
}
