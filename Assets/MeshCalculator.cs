using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using Parabox.Stl;

public class MeshCalculator : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> cylindersInContact;
    //private List<GameObject> CubeSTLList;
    private List<GameObject> altCube;
    private Vector3[,] PosLocal = new Vector3[5, 5];
    private Vector3[] flatPosLocal = new Vector3[25];
    private int index;

    void Start()
    {
        cylindersInContact = new List<GameObject>();
        //CubeSTLList = new List<GameObject>();
        altCube = new List<GameObject>();
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                PosLocal[i, j] = new Vector3(i * 0.0375f, 1000f, j * 0.0375f);
            }
        }
        // Debug.Log(PosLocal[1, 1]);

        // flatten the array
        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                flatPosLocal[index] = PosLocal[i, j];
                index++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // KeydCode = f 
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key was pressed.");

            // find all the game objects with the tag "Cube", and for each cube, find all the gameobjects with the tag cylinder in contact with the cube
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
            // find all the gameobjects with the tag "Cylinder"
            GameObject[] cylinders = GameObject.FindGameObjectsWithTag("Cylinder");
            Debug.Log("cubes.Length: " + cubes.Length);
            Debug.Log("cylinders.Length: " + cylinders.Length);
            foreach (GameObject cube in cubes)
            {
                
                Debug.Log("cylinders.Length: " + cylinders.Length);
                foreach (GameObject cylinder in cylinders)
                {
                    
                    
                    // if the cylinder is in contact with the cube
                    if (cube.GetComponent<Collider>().bounds.Intersects(cylinder.GetComponent<Collider>().bounds))
                    {
                        
                        // subtract the cylinder from the cube
                        // Model result = CSG.Subtract(cube, cylinder);

                        // var composite = new GameObject();
                        // composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
                        // composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

                        // add this cylinder to list
                        cylindersInContact.Add(cylinder);
                        Debug.Log("cylinders in contact with cube: " + cube + " are: " + cylindersInContact.Count);
                        Debug.Log("cylinder: " + cylinder);

                        // draw gizmo bounds of the cylinder
                        // Gizmos.DrawWireCube(cylinder.GetComponent<Collider>().bounds.center, cylinder.GetComponent<Collider>().bounds.size);
                    }
                }

                // inventory, we have a list of cylinders in contact with the cube

                // create a single gameobject from the list of cylinders in contact with the cube
                CombineInstance[] combine = new CombineInstance[cylindersInContact.Count];

                for (int i = 0; i < cylindersInContact.Count; i++)
                {
                    combine[i].mesh = cylindersInContact[i].GetComponent<MeshFilter>().sharedMesh;
                    combine[i].transform = cylindersInContact[i].transform.localToWorldMatrix;
                    
                }

                // Combine the meshes into a single mesh.
                GameObject combinedCylinders = new GameObject("combinedCylinders");
                MeshFilter combinedMeshFilter = combinedCylinders.AddComponent<MeshFilter>();
                MeshRenderer combinedMeshRenderer = combinedCylinders.AddComponent<MeshRenderer>();
                combinedMeshFilter.sharedMesh = new Mesh();
                combinedMeshFilter.sharedMesh.CombineMeshes(combine);

                // Add a collider to the combined cylinders if needed.
                combinedCylinders.AddComponent<MeshCollider>();

                // Set the material of the combined cylinders to the material of the first cylinder.
                combinedMeshRenderer.sharedMaterial = cylindersInContact[0].GetComponent<MeshRenderer>().sharedMaterial;

                /**
                // List<GameObject> altCube = new List<GameObject>();
                for (int i = 0; i < cylindersInContact.Count; i++)
                {
                    Debug.Log("cylindersInContact[" + i + "]: " + cylindersInContact[i]);
                    // loop through the list of cylinders in contact with the cube
                    // for each cylinder, subtract it from the original cube and store the result in a new list altCube
                    Model result = CSG.Subtract(cube, cylindersInContact[i]);

                    var composite = new GameObject();
                    composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
                    composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

                    altCube.Add(composite);

                    // then intersect each of the results in the list altCube to get the final result
                }

                // intersect each of the results in the list altCube to get the final result
                // call function to intersect all the gameobjects in the list altCube
                
                GameObject finalResult = IntersectCubes(altCube);
                */
                // reset the list altCube
                altCube.Clear();

                // reset the list cylindersInContact
                cylindersInContact.Clear();


                Model result = CSG.Subtract(cube, combinedCylinders);

                var composite = new GameObject();
                composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
                composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
                composite.tag = "CubeSTL";
            }
            //All Cube are created, we just need to rescale and extract them all
            GameObject[] CubeSTLList = GameObject.FindGameObjectsWithTag("CubeSTL");
            Debug.Log(CubeSTLList.Length);
            index = 0;
            foreach (GameObject CubeSTL in CubeSTLList)
            {
                // Rescale each cube individualy
                CubeSTL.transform.localScale = new Vector3(0.025f,0.025f,0.025f);
                // attribute position Vector3 from flatPosLocal to CubeSTL
                // CubeSTL.transform.position = flatPosLocal[index];
                index++;
                //Export(string path, GameObject[] gameObjects, FileType type)
                string filePath = "Assets/STL";
                GameObject[] CubeExport = new GameObject[] {CubeSTL};
                bool success = Exporter.Export(filePath, CubeExport, FileType.Binary);
                if (success) { Debug.Log("Exported + " + index); }
                else { Debug.Log("Failed + " + index); }
            }

        }
    }

    // function to intersect all the gameobjects in the list altCube
    /*
    public GameObject IntersectCubes(List<GameObject> altCube)
    {
        // recursively intersect all the gameobjects in the list altCube
        // if there are only two gameobjects in the list, intersect them and return the result
        // check list size
        if (altCube.Count == 0)
        {
            // return null
            return null;
        }
        if (altCube.Count == 1)
        {
            // return the result
            return altCube[0];
        }
        if (altCube.Count == 2)
        {
            // intersect the two gameobjects in the list
            Model result = CSG.Intersect(altCube[0], altCube[1]);

            var composite = new GameObject();
            composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
            composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

            // return the result
            return composite;
        }
        else
        {
            // intersect the first two gameobjects in the list
            Model result = CSG.Intersect(altCube[0], altCube[1]);

            var composite = new GameObject();
            composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
            composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

            // remove the first two gameobjects from the list
            altCube.RemoveAt(0);
            altCube.RemoveAt(1);

            // add the result to the list
            altCube.Add(composite);

            // recursively intersect all the gameobjects in the list altCube
            return IntersectCubes(altCube);
        }


    }
    */
}
