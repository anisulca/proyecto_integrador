using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tobii.Gaming;

public class Sacadico_Boton : MonoBehaviour
{
    private float targetTime = 10.0f; //Tiempo límite de fijación
    public int i = 1;
    float[] coordenada;
    Rigidbody boton_rojo;
    private GazePoint lastGazePoint = GazePoint.Invalid;
    private Vector3 coordEstimulo;

    // Start is called before the first frame update
    void Start()
    {
        //coordenada = 
        boton_rojo = GetComponent<Rigidbody>();
        boton_rojo.position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        GazePoint gazeData = GetGazeData();
        

        if (targetTime <= 0.0f)
        {
            if (i < 10)
            {
                coordEstimulo = TimerEnded(i);
                i += 1;
            }
            else
            {
                SceneManager.LoadScene("EscenaInicio");
            }


        }

        Debug.Log("Coordenadas gaze point: " + gazeData.Screen);
        Debug.Log("Timestamp gaze point: " + gazeData.Timestamp);
        Debug.Log("Tiempo real:" + DateTime.Now.ToString("HHmmssffff"));
        Debug.Log("Posicion estimulo: " + coordEstimulo);

    }

    private Vector3 TimerEnded(int i)
    {
        switch (i)
        {
            case 1: 
                boton_rojo.position = new Vector3(-5.0f, 4.0f, 0.0f);
                break;
            case 2:
                boton_rojo.position = new Vector3(-5.0f, 0.0f, 0.0f);
                break;
            case 3:
                boton_rojo.position = new Vector3(-5.0f, -4.0f, 0.0f);
                break;
            case 4:
                boton_rojo.position = new Vector3(0.0f, 4.0f, 0.0f);
                break;
            case 5:
                boton_rojo.position = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case 6:
                boton_rojo.position = new Vector3(0.0f, -4.0f, 0.0f);
                break;
            case 7:
                boton_rojo.position = new Vector3(5.0f, 4.0f, 0.0f);
                break;
            case 8:
                boton_rojo.position = new Vector3(5.0f, 0.0f, 0.0f);
                break;
            case 9:
                boton_rojo.position = new Vector3(5.0f, -4.0f, 0.0f);
                break;
        }
        
        targetTime = 10.0f;
        return boton_rojo.position;

    }

    public GazePoint GetGazeData()
    {
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        if (gazePoint.IsRecent() && (gazePoint.Timestamp > (lastGazePoint.Timestamp - float.Epsilon)))
        {
            return gazePoint;
        }
        else
        {
            return GazePoint.Invalid;
        }
    }

}