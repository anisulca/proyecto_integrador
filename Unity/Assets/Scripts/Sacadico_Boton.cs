using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tobii.Gaming;
using System.Xml.Linq;
using System.IO;
using System.Text;

public class Sacadico_Boton : MonoBehaviour
{
    //Variables relacionadas al movimiento del estimulo
    private float targetTime = 20.0f; //Tiempo límite de fijación
    public int i = 1;
    float[] coordenada;
    Rigidbody boton_rojo;
    private GazePoint lastGazePoint = GazePoint.Invalid;
    private Vector3 coordEstimulo;

    //Variables relacionadas a la escritura del csv
    StringBuilder csvcontent = new StringBuilder();//crear archivo
    string csvpath = @"C:\Users\Gabriela\Documents\PROYECTO INTEGRADOR\CSV_Pruebas\Prueba1.csv";//direccion del archivo

    // Start is called before the first frame update
    void Start()
    {
        //coordenada = 
        boton_rojo = GetComponent<Rigidbody>();
        boton_rojo.position = new Vector3(0.0f, 0.0f, 0.0f);

        //Escribir encabezado del archivo csv
        csvcontent.AppendLine("PRUEBA DE MOVIMIENTOS SACADICOS");
        csvcontent.AppendLine("TiemporReal; Coord_Estim; Coord_GazePoint; TimeStamp_GP");
        File.WriteAllText(csvpath, csvcontent.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        GazePoint gazeData = GetGazeData();
        

        if (targetTime <= 0.0f)
        {
            if (i < 9)
            {
                coordEstimulo = TimerEnded(i);
                i += 1;
            }
            else
            {
                SceneManager.LoadScene("EscenaInicio");
            }


        }

        //Obtener tiempo de maquina
        String timeStampReal = DateTime.Now.ToString("HHmmssffff"); // tiempo de maquina
        var coordGazePoint = gazeData.Screen;
        var timeStampGazePoint = gazeData.Timestamp;

        //Imprimir por consola los datos
        Debug.Log("Coordenadas gaze point: " + gazeData.Screen);
        Debug.Log("Timestamp gaze point: " + gazeData.Timestamp);
        Debug.Log("Tiempo real:" + timeStampReal);
        Debug.Log("Posicion estimulo: " + coordEstimulo);

        //escribir csv        
        csvcontent.Append(timeStampReal);
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo);
        csvcontent.Append(";");
        csvcontent.Append(coordGazePoint);
        csvcontent.Append(";");
        csvcontent.AppendLine(timeStampGazePoint.ToString());
        File.WriteAllText(csvpath, csvcontent.ToString());
    }

    private Vector3 TimerEnded(int i)
    {
        switch (i)
        {
            case 1:
                boton_rojo.position = new Vector3(6.5f, -3.5f, 0.0f);
                break;
            case 2:
                boton_rojo.position = new Vector3(-6.5f, 0.0f, 0.0f);
                break;
            case 3:
                boton_rojo.position = new Vector3(6.5f, 3.5f, 0.0f);
                break;
            case 4:
                boton_rojo.position = new Vector3(0.0f, 3.5f, 0.0f);
                break;
            case 5:
                boton_rojo.position = new Vector3(0.0f, -3.5f, 0.0f);
                break;
            case 6:
                boton_rojo.position = new Vector3(-6.5f, 3.5f, 0.0f);
                break;
            case 7:
                boton_rojo.position = new Vector3(0.0f, 3.5f, 0.0f);
                break;
            case 8:
                boton_rojo.position = new Vector3(-6.5f, -3.5f, 0.0f);
                break;
        }
        
        targetTime = 20.0f;
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