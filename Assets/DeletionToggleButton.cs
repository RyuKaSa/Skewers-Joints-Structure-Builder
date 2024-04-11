using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeletionToggleButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private bool deleteMode = false;

    public bool DeleteMode
    {
        get { return deleteMode; }
        set { deleteMode = value; }
    }

    // start
    private void Start()
    {
        buttonText.text = deleteMode ? "DELETE ON" : "DELETE OFF";
    }

    public void ToggleDeleteMode()
    {
        deleteMode = !deleteMode;
        buttonText.text = deleteMode ? "DELETE ON" : "DELETE OFF";
    }

    void Update()   
    {
        buttonText.text = deleteMode ? "DELETE ON" : "DELETE OFF";
        // Debug.Log("Update: " + deleteMode);
        if (deleteMode && Input.GetMouseButtonDown(0)) // Clic gauche de la souris
        {
            Debug.Log("Delete mode: " + deleteMode);
            
            // Lancer un rayon depuis la position de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectToDestroy = hit.collider.gameObject;

                // V�rifier si l'objet � d�truire appartient � la cat�gorie des objets plac�s par l'utilisateur
                if (objectToDestroy.CompareTag("Cube"))
                {
                    // check if is a cube, might need to remove skewers connected to it
                    GameObject[] cylinders = GameObject.FindGameObjectsWithTag("Cylinder");
                    foreach (GameObject cylinder in cylinders)
                    {
                        // Destroy cylinders in contact
                        if ((objectToDestroy.GetComponent<Collider>().bounds.Intersects(cylinder.GetComponent<Collider>().bounds)))
                        {
                            Destroy(cylinder);
                            Debug.Log("cylinder: " + cylinder + " destroyed");

                            PlacementSystem.nbSkewer -= 1;
                        }

                    }


                    // Supprimez l'objet
                    Destroy(objectToDestroy);
                    Debug.Log("OG cube: " + objectToDestroy + " destroyed");

                    PlacementSystem.nbCube -= 1;

                }
                else if (objectToDestroy.CompareTag("Cylinder"))
                {
                    Destroy(objectToDestroy);
                    Debug.Log("OG cylinder: " + objectToDestroy + " destroyed");

                    PlacementSystem.nbSkewer -= 1;
                }
            }
            Debug.Log(PlacementSystem.nbSkewer);
            Debug.Log(PlacementSystem.nbCube);
        }
    }
}
