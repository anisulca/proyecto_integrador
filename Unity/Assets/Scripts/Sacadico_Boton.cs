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
    public float targetTimeInicial; //Tiempo límite de fijación que se fija por consola
    private float targetTime;
    private int caso = 0;
    Rigidbody boton_rojo;

    //lista de casos posibles de posiciones del estímulo
    int[] valores = { 1, 2, 3, 4, 5, 6, 7, 8};
    List<int> listaCasos = new List<int>();
    System.Random rnd = new System.Random();

    //se declara cámara para consirerar las coordenadas de screen
    public Camera cam;
    public GameObject boton;

    //Variables relacionadas a la coordenada de mirada
    private GazePoint lastGazePoint = GazePoint.Invalid;
    private Vector3 coordEstimulo;

    //Variables relacionadas a la escritura del csv estimulo
    StringBuilder csvcontent = new StringBuilder();//crear archivo
    string csvpath = @"C:\Users\Dani\Documents\PROYECTO INTEGRADOR\CSV_Pruebas\Prueba_MS.csv";//direccion del archivo

    // Start is called before the first frame update
    void Start()
    {
        //coordenada inicial del estímulo siempre al centro
        boton_rojo = GetComponent<Rigidbody>();
        boton_rojo.position = new Vector3(0.0f, 0.0f, 0.0f);
        targetTime = targetTimeInicial;

        //mezclar array con valores de posiciones del estímulo
        listaCasos.AddRange(valores);

        var count = listaCasos.Count;
        var last = count - 1;
        for(var num = 0; num < last; ++num)
        {
            var r = rnd.Next(num, count);
            var tmp = listaCasos[num];
            listaCasos[num] = listaCasos[r];
            listaCasos[r] = tmp;
        }

        foreach(var x in listaCasos)
        {
            Debug.Log("lista de posiciones" + x);
        }

        

        //Escribir encabezado del archivo csv
        csvcontent.AppendLine("PRUEBA DE MOVIMIENTOS SACADICOS");
        csvcontent.AppendLine("TiempoReal; Coord_Estim_X; Coord_Estim_Y; Coord_Estim_Z; Coord_Estim_px_X; Coord_Estim_px_Y; Coord_GazePoint_X; Coord_GazePoint_Y; TimeStamp_GP");

    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        GazePoint gazeData = GetGazeData();
        

        if (targetTime <= 0.0f) //Al terminar el tiempo de fijación se cambia la posición del estímulo
        {
            if (caso < 8)
            {
                coordEstimulo = TimerEnded(listaCasos[caso]); //Se elige el caso desde la lista con el número de caso 
                caso += 1;
            }
            else
            {
                //Se pasan todos los datos recolectados al archivo csv
                File.WriteAllText(csvpath, csvcontent.ToString());
                
                //Se cambia de escena al menú
                SceneManager.LoadScene("EscenaInicio");
            }


        }

        //Obtener tiempo de maquina
        String timeStampReal = DateTime.Now.ToString("HHmmssffff");

        //Obtener coordenadas con píxeles
        Vector2 coordEstimulo_screen = cam.WorldToScreenPoint(boton.transform.position);

        //Obtener coordenadas de mirada
        var coordGazePoint = gazeData.Screen;
        var timeStampGazePoint = gazeData.Timestamp;

        //Imprimir por consola los datos
        Debug.Log("Coordenadas gaze point: " + gazeData.Screen);
        Debug.Log("Timestamp gaze point: " + gazeData.Timestamp);
        Debug.Log("Tiempo real:" + timeStampReal);
        Debug.Log("Posicion estimulo: " + coordEstimulo);
        Debug.Log("Tiempo de fijación: "+ targetTime);

        //escribir csv        
        csvcontent.Append(timeStampReal);
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo.x);
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo.y);
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo.z);
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo_screen.x);
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo_screen.y);
        csvcontent.Append(";");
        csvcontent.Append(coordGazePoint.x);
        csvcontent.Append(";");
        csvcontent.Append(coordGazePoint.y);
        csvcontent.Append(";");
        csvcontent.AppendLine(timeStampGazePoint.ToString());

    }

    private Vector3 TimerEnded(int caso)
    {
        switch (caso)
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
        
        targetTime = targetTimeInicial;
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