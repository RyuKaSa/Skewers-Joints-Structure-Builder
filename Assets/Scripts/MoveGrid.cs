using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{
    public float moveIncrement = 0.025f; // Distance to move in one click

    void Update()
    {
        // Gestion du déplacement vers le haut avec la flèche du haut
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveObject(Vector3.up);
        }

        // Gestion du déplacement vers le bas avec la flèche du bas
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveObject(Vector3.down);
        }
    }

    void MoveObject(Vector3 direction)
    {
        Vector3 newPosition = transform.position + (direction * moveIncrement);
        transform.position = newPosition;
    }
}
