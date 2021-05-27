using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Tobii.Gaming;
using System.Threading;
//bibliotecas necesarias para crear/escribir un archivo
using System.Xml.Linq;
using System.IO;
using System.Text;

public class Seguim_V : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints; //se definen puntos de referencia para movimiento horizontal

    private float targetTime = 30f; //contador de 40 segundos para duración de la prueba 
    private float speed = 2f; //se define velocidad fija para el movimiento del estímulo
    int index = 0;//índice para el orden de los waypoints en el arreglo, define el recorrido que seguirá el estímulo
    int fij = 0; // bandera 2, para fijacion
    private float cont = 5; // tiempo aprox de retardo inicial en segundos
    int cambio_escena = 0; // bandera para cambio de escena
    int ciclo = 0;

    //se declara cámara para consirerar las coordenadas de screen
    public Camera cam;
    public GameObject boton;

    //Variable relacionada a la captura de datos con Tobii
    private GazePoint lastGazePoint = GazePoint.Invalid;

    //Variables relacionadas a la escritura del csv
    private StringBuilder csvcontent = new StringBuilder();//crear archivo
    private string csvpath = @"C:\Users\Dani\Documents\PROYECTO INTEGRADOR\CSV_Pruebas\Seguim_V.csv";//direccion del archivo

    void Start()
    {
        transform.position = waypoints[index].transform.position; //se define la posicion inicial en 0,0

        //Escribir encabezado del archivo csv
        csvcontent.AppendLine("TiempoReal; Coord_Estim_X; Coord_Estim_Y; Coord_Estim_Z; Coord_Estim_px_X; Coord_Estim_px_Y; Coord_GazePoint_X; Coord_GazePoint_Y; TimeStamp_GP");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Obtener gazepoint
        GazePoint gazeData = GetGazeData();

        //comportamiento del estimulo (pelota)
        if (fij == 0)
        {
            Fijacion();
        }
        else
        {
            Desplazamiento();
        }

        //Obtener datos
        String timeStampReal = DateTime.Now.ToString("mmssff"); // tiempo de maquina
        Vector3 coordEstimulo = boton.transform.position; //coordenada del estímulo en sistema de coordenadas world space
        Vector2 coordEstimulo_screen = cam.WorldToScreenPoint(boton.transform.position); //coordenada del estímulo en sist. de coordenadas de la pantalla
        Vector2 coordGazePoint = gazeData.Screen; //coordenada de la mirada del usuario en la pantalla
        var timeStampGazePoint = gazeData.Timestamp; //tiempo de la coord. de mirada del usuario

        //mostrar en consola
        Debug.Log("Tiempo restante fijacion " + cont);
        Debug.Log("Tiempo restante desplazamiento " + targetTime);
        Debug.Log("Pasadas restantes " + ciclo);
        Debug.Log("Coordenadas en px " + coordEstimulo_screen);
        Debug.Log("Coordenadas en unidades " + coordEstimulo);
        Debug.Log("Tiempo real: " + timeStampReal);

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

    void Fijacion()
    { // retardo de 15 segundos al iniciar la prueba y de 10 seg antes de volver a pantalla inicial
        cont -= Time.deltaTime;
        if (cont <= 0.0f)
        {
            //cont = 10f;
            fij = 1;
            cambio_escena += 1;
        }
        if (cambio_escena == 2)
        {
            //Se crea un archivo csv con los datos obtenidos
            File.WriteAllText(csvpath, csvcontent.ToString());

            //cambio de escena a menú
            SceneManager.LoadScene("EscenaInicio");
        }
    }

    void Desplazamiento()
    {//movimiento horizontal

        targetTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
                                                waypoints[index].transform.position,
                                                speed * Time.deltaTime);

        if (transform.position == waypoints[index].transform.position)
        {
            index += 1;
        }

        if (index == waypoints.Length)
        {
            index = 0;
            ciclo += 1; //Cada vez que pasa por el último waypoint se considera que completó un ciclo
        }

        if (ciclo >=3) //Se define el final de la función en base a la cantidad de veces que completa un ciclo
        {
            //targetTime = 5f;
            index = 0;
            fij = 0;
            cont = 5;
        }
    }

    private GazePoint GetGazeData()
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

