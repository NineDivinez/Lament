using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeToHouse : MonoBehaviour
{
    //Variables
    public float distance;
    public Vector3 size;
    public bool inArea = false;

    //Game Objects
    public MeshFilter target;
    public GameObject[] enterances;
    public MeshFilter houseMesh;

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < target.mesh.vertices.Length; i++)
            {
                if (houseMesh.mesh.bounds.Contains(transform.TransformPoint(target.mesh.vertices[i])))
                {
                    print("Is player inside?");
                }
            }
        }
    }
}
