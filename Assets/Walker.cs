using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Walker : MonoBehaviour
{
    public GameObject bcPrefab;     // An empty GameObject holding the BasicCube.cs script.
    static int PARTS = 16;
    BasicCube[] scripts;
    float[] rotationsX;
    float[] directions;
    enum PART_NAME
    {
        HIP, TORSO, NECK, HEAD,
        L_SHOULDER, L_ARM, L_HAND,
        R_SHOULDER, R_ARM, R_HAND,
        L_THIGH, L_LEG, L_FOOT,
        R_THIGH, R_LEG, R_FOOT
    };

    // Start is called before the first frame update
    void Start()
    {
        scripts = new BasicCube[PARTS];
        rotationsX = new float[PARTS];
        directions = new float[PARTS];
        for (int p = 0; p < PARTS; p++)
        {
            scripts[p] = Instantiate(bcPrefab).GetComponent<BasicCube>();
            rotationsX[p] = 0.0f;
            directions[p] = 1.0f;
        }

    }

    void Update()
    {
        //RESET ALL PARTS;
        for (int p = 0; p < PARTS; p++) {
            scripts[p].resetPoints();
        }

        //Rotate back and forth
        rotationsX[(int)PART_NAME.TORSO] += 0.2f * directions[(int)PART_NAME.TORSO];
        if(rotationsX[(int)PART_NAME.TORSO] > 5.0f || rotationsX[(int)PART_NAME.TORSO] < -5.0f)
        {
            directions[(int)PART_NAME.TORSO] = -directions[(int)PART_NAME.TORSO];
        }
        rotationsX[(int)PART_NAME.L_SHOULDER] += 0.3f * directions[(int)PART_NAME.L_SHOULDER];
        if (rotationsX[(int)PART_NAME.L_SHOULDER] > 15.0f || rotationsX[(int)PART_NAME.L_SHOULDER] < -5.0f)
        {
            directions[(int)PART_NAME.L_SHOULDER] = -directions[(int)PART_NAME.L_SHOULDER];
        }

        // Hip
        Matrix4x4 hipT = MatrixOperations.opTranslate(0, 3, 0);
        Matrix4x4 hipS = MatrixOperations.opScale(1, 0.2f, 0.8f);
        //scripts[(int)PART_NAME.HIP].resetPoints();
        TransformMesh(scripts[(int)PART_NAME.HIP].getMesh(), hipT * hipS);

        // TORSO
        // rotationsX[(int)PART_NAME.TORSO] = 30.0f;
        Matrix4x4 torsoTminusone = MatrixOperations.opTranslate(0, -1.1f, 0);
        Matrix4x4 torsoRX = MatrixOperations.opRotate(rotationsX[(int)PART_NAME.TORSO], MatrixOperations.AXIS.AX_X);
        Matrix4x4 torsoTplusone = MatrixOperations.opTranslate(0, 1.1f, 0);

        Matrix4x4 torsoT = MatrixOperations.opTranslate(0, 1.2f, 0);
        //scripts[(int)PART_NAME.TORSO].resetPoints();
        Matrix4x4 torsoFinal = hipT * torsoT * torsoTminusone * torsoRX * torsoTplusone;
        TransformMesh(scripts[(int)PART_NAME.TORSO].getMesh(), hipT * torsoT * torsoTminusone * torsoRX * torsoTplusone);

        //Neck
        Matrix4x4 neckT = MatrixOperations.opTranslate(0, 1.2f, 0);
        Matrix4x4 neckS = MatrixOperations.opScale(0.25f, 0.25f, 0.25f);
        //scripts[(int)PART_NAME.NECK].resetPoints();
        TransformMesh(scripts[(int)PART_NAME.NECK].getMesh(), torsoFinal * neckT * neckS);

        //Head
        Matrix4x4 headT = MatrixOperations.opTranslate(0, 0.7f, 0);
        Matrix4x4 headS = MatrixOperations.opScale(0.5f, 0.5f, 0.5f);
        //scripts[(int)PART_NAME.HEAD].resetPoints();
        TransformMesh(scripts[(int)PART_NAME.HEAD].getMesh(), torsoFinal * neckT * headT * headS);

        //L_SHOULDER
        Matrix4x4 LshoulderT = MatrixOperations.opTranslate(1.2f, 0.6f, 0);
        Matrix4x4 LshoulderS = MatrixOperations.opScale(0.4f, 0.4f, 0.4f);
        Matrix4x4 LshoulderRX = MatrixOperations.opRotate(rotationsX[(int)PART_NAME.L_SHOULDER], MatrixOperations.AXIS.AX_X);
        //scripts[(int)PART_NAME.L_SHOULDER].resetPoints();
        TransformMesh(scripts[(int)PART_NAME.L_SHOULDER].getMesh(), torsoFinal * LshoulderT * LshoulderS);

        //L_ARM
        Matrix4x4 LarmT = MatrixOperations.opTranslate(1.6f, -0.3f, 0);
        Matrix4x4 LarmS = MatrixOperations.opScale(0.35f, 1.2f, 0.35f);
        TransformMesh(scripts[(int)PART_NAME.L_ARM].getMesh(), torsoFinal * LarmS * LarmT);

        //L_HAND
        Matrix4x4 LhandT = MatrixOperations.opTranslate(0, -1.1f, 0);
        Matrix4x4 LhandS = MatrixOperations.opScale(0.4f, 0.4f, 0.4f);
        TransformMesh(scripts[(int)PART_NAME.L_ARM].getMesh(), torsoFinal * LhandS * LhandT);


    }

    void TransformMesh(Mesh m, Matrix4x4 t)
    {
        Vector3[] points = m.vertices;
        int total = points.Length;
        for (int i = 0; i < total; i++)
        {
            Vector4 v = points[i];  // Vector3 to Vector4 (w is zero)
            v.w = 1;
            points[i] = t * v;      // Vector4 to Vector3
        }
        m.vertices = points;
        m.RecalculateNormals();
    }
}
