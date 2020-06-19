using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCone : MonoBehaviour
{
    public float angle = 10;
    public float distance = 60;

    public Mesh CreateViewCone(float aAngle, float aDistance, int aConeResolution = 30)
    {
        Vector3[] verts = new Vector3[aConeResolution + 1];
        Vector3[] normals = new Vector3[verts.Length];
        int[] tris = new int[aConeResolution * 3];
        Vector3 a = Quaternion.Euler(-aAngle, 0, 0) * Vector3.forward * aDistance;
        Vector3 n = Quaternion.Euler(-aAngle, 0, 0) * Vector3.up;
        Quaternion step = Quaternion.Euler(0, 0, 360f / aConeResolution);
        verts[0] = Vector3.zero;
        normals[0] = Vector3.back;
        for (int i = 0; i < aConeResolution; i++)
        {
            normals[i + 1] = n;
            verts[i + 1] = a;
            a = step * a;
            n = step * n;
            tris[i * 3] = 0;
            tris[i * 3 + 1] = (i + 1) % aConeResolution + 1;
            tris[i * 3 + 2] = i + 1;
        }
        Mesh m = new Mesh();
        m.vertices = verts;
        m.normals = normals;
        m.triangles = tris;
        m.RecalculateBounds();
        return m;
    }

    void Start()
    {
        print("Debug");
        CreateViewCone(angle, distance);
    }
}
