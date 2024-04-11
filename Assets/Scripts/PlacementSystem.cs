using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField] private Camera sceneCamera;

    [SerializeField] private GameObject mouseIndicator, cellIndicator;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private Grid grid;

    [SerializeField] private ObjectsDatabaseSO database;

    [SerializeField] private GameObject cylinderPrefab;

    [SerializeField] private DeletionToggleButton deletionToggleButton;
    private int selectedObjectIndex = -1;
    private GameObject firstCube, closestCube, secondCube;
    private bool firstCubeSelected = false;
    private int currentCubeIndex;
    private int currentCylinderIndex;

    private Vector3 firstCubePosition, secondCubePosition;

    [SerializeField] private GameObject gridVisualization;

    public static int nbCube = 0;
    public static int nbSkewer = 0;

    private void Start()
    {
        StopPlacement();
        currentCubeIndex = 1;
        currentCylinderIndex = 1;
    }

    public void StartPlacement(int index)
    {
        // reset
        StopPlacement();

        deletionToggleButton.DeleteMode = false;

        // check if index is valid
        // selectedObjectIndex = database.objectDatabase.FindIndex(data => data.ID == index);
        selectedObjectIndex = index;
        if (selectedObjectIndex == -1)
        {
            Debug.LogError("Object with index " + index + " not found!");
            return;
        }


        // visual tweaks
        if (selectedObjectIndex == 0)
        {
            gridVisualization.SetActive(true);
            cellIndicator.SetActive(true);
        }
        else
        {
            gridVisualization.SetActive(false);
            cellIndicator.SetActive(false);
        }
        

        // set events
        inputManager.OnExit += StopPlacement;

        // if index = 0, call PlaceStructure
        if (selectedObjectIndex == 0)
        {
            inputManager.OnClicked += PlaceStructure;
        }
        else
        {
            inputManager.OnClicked += PlaceSkewer;
        }
        
    }

    public void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI() || deletionToggleButton.DeleteMode)
        {
            return;
        }
        if (nbCube >= 40)
        {
            Debug.Log("Too many cubes : " +  nbCube);
            return;
        }

        // get mouse position in world and cell
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        GameObject structure = Instantiate(database.objectDatabase[selectedObjectIndex].Prefab);
        structure.name = "cube" + currentCubeIndex;
        currentCubeIndex++;

        // give tag "Cube" to structure
        structure.tag = "Cube";
        // for index1, add 0.0125 in all directions
        if (selectedObjectIndex == 1)
        {
            // check if there is a structure tag cube already under the mousePosition
            structure.transform.position = grid.GetCellCenterWorld(gridPosition) + new Vector3(0, 0.0125f, 0);
        }
        else
        {
            structure.transform.position = grid.GetCellCenterWorld(gridPosition);
        }

        PlacementSystem.nbCube += 1;
        Debug.Log(PlacementSystem.nbCube);

    }

    // for index 1
    public void PlaceSkewer()
    {

        
        if (inputManager.IsPointerOverUI() || deletionToggleButton.DeleteMode)
        {
            return;
        }
        if (nbSkewer >= 20)
        {
            Debug.Log("Too many skewers : " + nbSkewer);
            return;
        }
        
        // check if we can run an if else statement to separate firstcube and secondcube
        if (firstCubeSelected == false)
        {
            // we do not have a first cube yet, so we need to create one
            firstCube = closestCubeFct();
            firstCubeSelected = true;

            // Debug.Log("firstCube: " + firstCube);
        }
        else
        {
            
            secondCube = closestCubeFct();
            
            firstCubeSelected = false;

            Debug.Log("firstCube: " + firstCube);
            Debug.Log("secondCube: " + secondCube);

            
            firstCubePosition = firstCube.transform.position;
            secondCubePosition = secondCube.transform.position;

            // Calculate the height of the cylinder.
            float cylinderHeight = Vector3.Distance(firstCubePosition, secondCubePosition) - 1.3f;

            // Calculate the midpoint between base and top positions.
            Vector3 midpoint = (firstCubePosition + secondCubePosition) / 2f;

            // Instantiate the cylinder with the calculated parameters.
            GameObject cylinder = Instantiate(cylinderPrefab, midpoint, Quaternion.identity);

            cylinder.name = "cylinder" + currentCylinderIndex;
            currentCylinderIndex++;

            // Set the scale of the cylinder to fit the height.
            cylinder.transform.localScale = new Vector3(0.5f, cylinderHeight / 2f, 0.5f);

            // Calculate the direction vector from the base to the top.
            Vector3 direction = secondCubePosition - firstCubePosition;

            // Rotate the cylinder to align with the direction.
            cylinder.transform.rotation = Quaternion.LookRotation(direction);

            // add 90 degrees to the rotation X to align the cylinder with the direction
            cylinder.transform.Rotate(90f, 0f, 0f);

            cylinder.transform.position = midpoint;
            PlacementSystem.nbSkewer += 1;

            //We check if scale > 40

            if (cylinderHeight > 40)
            {
                Destroy(cylinder);
                Debug.Log("Cylinder Height is above 40 : " + cylinderHeight);
                PlacementSystem.nbSkewer -= 1;
            }
            Debug.Log(PlacementSystem.nbSkewer);
        }

    }


    public GameObject closestCubeFct()
    {
        
        Vector3 screenMousePosition = Input.mousePosition;

        Ray ray = sceneCamera.ScreenPointToRay(screenMousePosition);

        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // A hit occurred, you can access hit information here
            // Debug.Log("Hit object name: " + hit.transform.name);
            
            // check if the hit object is tagged as cube
            if (hit.transform.tag == "Cube")
            {
                return hit.transform.gameObject;
            }
            else
            {
                // No hit occurred
                return null;
            }
        }
        else
        {
            // No hit occurred
            return null;
        }

    }

    public void StopPlacement()
    {

        // reset
        selectedObjectIndex = -1;
        firstCube = null;
        closestCube = null;
        secondCube = null;
        

        // visual tweaks
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);

        // reset events
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnClicked -= PlaceSkewer;
        inputManager.OnExit -= StopPlacement;
    }



    private void Update()
    {
        // check if index is valid, else, dont run rest of code
        if (selectedObjectIndex == -1)
        {
            return;
        }

        // Debug mouse posit
        // Debug.Log("Mouse position: " + inputManager.GetSelectedMapPosition());
        // if deletion mode is on, grid set to false and cellindicator as well
        if (deletionToggleButton.DeleteMode)
        {
            gridVisualization.SetActive(false);
            cellIndicator.SetActive(false);
        }

        
        // get mouse position in world and cell
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        // update indicators in wolrd and cell
        mouseIndicator.transform.position = mousePosition;
        // add 0.02 in vector Y direction
        cellIndicator.transform.position = grid.GetCellCenterWorld(gridPosition) + new Vector3(0, 0.0126f, 0);


        // 
    }
}