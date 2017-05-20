using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteDifference : MonoBehaviour
{
    public GameObject prefab;
    static int xGridSize = 20;
    static int yGridSize = 20;
    float deltaX = 1.0f;
    float deltaY = 1.0f;
    //creating a grid of spheres
    GameObject[,] spheres = new GameObject[xGridSize, yGridSize];
    float[,] Uprev = new float[xGridSize, yGridSize];
    float[,] Ucurr = new float[xGridSize, yGridSize];
    float[,] Unext = new float[xGridSize, yGridSize];

    public float c = 1.0f;
    float r;
    float rSqure;
    float time;

    float calculateFunctionF(float x, float y)
    {
        //float result = 0.003f * (1 - Mathf.Abs(Mathf.Sin(x)))*(1-Mathf.Abs(Mathf.Sin(2*y))) + 2*(1-Mathf.Abs(Mathf.Sin(5*x)))*(1-Mathf.Abs(Mathf.Sin(7*y)));
        float result = (x * y)/5000;

        return result;
    }

    float calculateFunctionG(float x, float y)
    {
        //float result =  0.002f* (1 - Mathf.Abs(Mathf.Sin(x))) * (1 - Mathf.Abs(Mathf.Sin(y))) + 2 * (1 - Mathf.Abs(Mathf.Sin(2 * x))) * (1 - Mathf.Abs(Mathf.Sin(2 * y)));
        float result = (x * y) / 3000;

        return result;
    }

    // Use this for initialization
    void Start ()
    {


        //creating 10 by 10 grid of spheres of radius 1
        for (int i = 0; i < xGridSize; ++i)
        {
            for (int j = 0; j < yGridSize; ++j)
            {
              spheres[i,j]=(GameObject)Instantiate(prefab,new Vector3(i*deltaX,0, j * deltaY),Quaternion.identity);
            }
        }



        for (int i = 0; i < xGridSize; ++i)
        {
            for (int j = 0; j < yGridSize; ++j)
            {
                Uprev[i, j] = 0.0f;
                Unext[i, j] = 0.0f;
                Ucurr[i, j] = calculateFunctionF(i * deltaX, j * deltaY) + calculateFunctionG(i * deltaX, j * deltaY)*Time.deltaTime;
                Debug.Log("U[" + i + "," + j + "] = " + Ucurr[i,j]);
            }
        }

        r = (c * Time.deltaTime) / deltaX;
        rSqure = Mathf.Pow(r, 2);


    }

    // Update is called once per frame
    void Update ()
    {
     
        //setting height of each sphere based on calculated values.
        for (int i = 0; i < xGridSize; ++i)
        {
            for (int j = 0; j < yGridSize; ++j)
            {
                spheres[i, j].transform.position = new Vector3(i, Ucurr[i, j], j);
            }
        }

        //calculation.
        for (int i = 0; i < xGridSize; ++i)
        {
            for (int j = 0; j < yGridSize; ++j)
            {
                if (i + 1 < xGridSize && j + 1 < yGridSize && i - 1 >= 0 && j - 1 >= 0)
                {
                    Unext[i, j] = rSqure * (Ucurr[i + 1, j] + Ucurr[i, j + 1] + Ucurr[i - 1, j] + Ucurr[i, j - 1] - 4*Ucurr[i,j]) + 2 * Ucurr[i, j] - Uprev[i, j];
                }
            }
        }


        //swapping the values in previous and current.
        for (int i = 0; i < xGridSize; ++i)
        {
            for (int j = 0; j < yGridSize; ++j)
            {
                Uprev[i, j] = Ucurr[i, j];
                Ucurr[i, j] = Unext[i, j];
            }
        }


    }
}
