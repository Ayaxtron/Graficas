/*
 * TC3022. Computer Graphics 
 * Sergio Ruiz-Loza, Ph.D.
 * 
 * Basic script for an animated arm.
 * Uses BasicCube.cs
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArm : MonoBehaviour
{
    public GameObject bcPrefab;
    BasicCube bc1;          // This will be a script type. An instance of BasicCube.cs
    BasicCube bc2;          // This will be a script type. An instance of BasicCube.cs
    float zRot = 0.0f;      // Starting rotation angle.
    float zDir = 1.0f;      // Starting direction for rotation.

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate a BasicCubePrefab, then get its BasicCube.cs script.
        bc1 = Instantiate(bcPrefab).GetComponent<BasicCube>();
        bc2 = Instantiate(bcPrefab).GetComponent<BasicCube>();
    }

    void Update()
    {
        // Always add DIRECTION * DELTA to the rotation
        zRot += zDir * 0.4f;
        // Change direction with the threshold (0, 60):
        if (zRot > 60.0f || zRot < 0.0f) zDir = -zDir;

        Matrix4x4 rotZ = MatrixOperations.opRotate(zRot, MatrixOperations.AXIS.AX_Z);

        Matrix4x4 right = MatrixOperations.opTranslate(1, 0, 0);

        Matrix4x4 scale = MatrixOperations.opScale(1, 0.5f, 0.5f);

        Matrix4x4 transformNoScale = rotZ * right;

        Matrix4x4 transform = rotZ * right * scale;


        Matrix4x4 rotZ2 = MatrixOperations.opRotate(zRot * 0.75f, MatrixOperations.AXIS.AX_Z);
        Matrix4x4 transform2 = transformNoScale * right * rotZ2 * right * scale;

        // Reset both links before transforming them:
        bc1.resetPoints();
        bc2.resetPoints();

        // Finally, transform both links from their original points:
        TransformMesh(bc1.getMesh(), transform);
        TransformMesh(bc2.getMesh(), transform2);
    }

    void TransformMesh(Mesh m, Matrix4x4 t)
    {
        Vector3[] points = m.vertices;
        int total = points.Length;
        for (int i = 0; i < total; i++)
        {
            Vector4 v = new Vector4(points[i].x,
                                    points[i].y,
                                    points[i].z, 1);
            v = t * v;
            points[i] = new Vector3(v.x, v.y, v.z);
        }
        m.vertices = points;
        m.RecalculateNormals();
    }

}
