using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVectorsAndMatricesAndPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v1 = new Vector3(1.5f,2.5f,3.5f);
        Vector3 v2 = new Vector3(1.0f,3.5f, 1.5f);

        const float k = 0.75f;

        print("v1 + v2 = " + (v1 + v2));
        print("v1 - v2 = " + (v1 - v2));

        print("k(v1) = " + (k * v1));
        print("k(v2) = " + (k * v2));

        print("v1.v2 = " + Vector3.Dot(v1,v2));
        print("v1 x v2 = " + Vector3.Cross(v1,v2));

        print("|v1| = " + Vector3.Magnitude(v1));
        print("|v2| = " + Vector3.Magnitude(v2));

        print("angle(v1,v2) = " + Vector3.Angle(v1, v2));

        print("cos(angle(v1,v2)) = " + Mathf.Cos(Vector3.Angle(v1, v2)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
