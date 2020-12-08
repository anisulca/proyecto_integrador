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


public class SeOc_Boton_HV : MonoBehaviour
{
    //Variables relacionadas al movimiento de la pelotita
    [SerializeField]
    Transform[] waypoints; //defino puntos de referencia
    private float targetTime = 40f; //contador de 40
    private float speed = 2f; //defino velocidad fija
    int mov = 0; //defino bandera 1
    int inicial = 0;//variable de índice que realice un seguimiento del 
    //punto de ruta hacia el que la bola está yendo actualmente

    int fij = 0; // bandera 2, para fijacion
    private float cont = 15; // tiempo aprox de retardo inicial en segundos
    int cambio_escena = 0; // bandera para cambio de escena

    //Variable relacionada a la captura de datos con Tobii
    private GazePoint lastGazePoint = GazePoint.Invalid;

    //Variables relacionadas a la escritura del csv
    StringBuilder csvcontent = new StringBuilder();//crear archivo
    string csvpath = @"C:\Users\Gabriela\Documents\PROYECTO INTEGRADOR\CSV_Pruebas\Prueba1.csv";//direccion del archivo

    void Start()
    {

        transform.position = waypoints[inicial].transform.position; //defino la posicion inicial en 0,0
        //Escribir encabezado del archivo csv
        csvcontent.AppendLine("PRUEBA DE SEGUIMIENTO SUAVE - MOVIMIENTO HORIZONTAL-VERTICAL");
        csvcontent.AppendLine("TiemporReal; Coord_Estim; Coord_GazePoint; TimeStamp_GP");
        File.WriteAllText(csvpath, csvcontent.ToString());
    }

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
            Tiempo();

        }

        //Obtener tiempo de maquina
        String timeStampReal = DateTime.Now.ToString("HHmmssffff"); // tiempo de maquina
        var coordEstimulo = transform.position;
        var coordGazePoint = gazeData.Screen;
        var timeStampGazePoint = gazeData.Timestamp;

        //Imprimir por consola los datos
        print("Tiempo real: " + timeStampReal); //imprime tiempo
        print("Coordenadas estimulo: " + coordEstimulo);//imprime posicion
        Debug.Log("Coordenadas gaze point: " + coordGazePoint);
        Debug.Log("Timestamp gaze point: " + timeStampGazePoint);


        //escribir csv        
        csvcontent.Append(timeStampReal);
        csvcontent.Append(",");
        csvcontent.Append(coordEstimulo);
        csvcontent.Append(",");
        csvcontent.Append(coordGazePoint);
        csvcontent.Append(",");
        ////csvcontent.Append(transform.position.y);
        //csvcontent.Append(",");
        csvcontent.AppendLine(timeStampGazePoint.ToString());
        File.WriteAllText(csvpath, csvcontent.ToString());
    }


    void Fijacion()
    { // retardo de 15 segundos al iniciar la prueba y de 10 seg antes de volver a pantalla inicial
        cont -= Time.deltaTime;
        if (cont <= 0.0f)
        {
            cont = 10f;
            fij = 1;
            cambio_escena += 1;
        }
        if (cambio_escena == 2)
        {
            //close
            SceneManager.LoadScene("EscenaInicio");
        }
    }


    void Tiempo()
    {
        if (mov == 0)
        {
            Move_1();
        }
        else
        {
            Move_2();
        }
    }

    void Move_1()
    {//movimiento horizontal

        targetTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
                                                waypoints[inicial].transform.position,
                                                speed * Time.deltaTime);

        if (transform.position == waypoints[inicial].transform.position)
        {
            inicial += 1;
        }

        if (inicial == waypoints.Length - 2)
            inicial = 0;

        if (targetTime <= 0.0f)
        { //reestablesco valores
            targetTime = 40f;
            mov = 1;
            inicial = 3;
        }
    }

    void Move_2()
    {//movimiento vertical

        targetTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,
                                                waypoints[inicial].transform.position,
                                                speed * Time.deltaTime);

        if (transform.position == waypoints[inicial].transform.position)
        {
            inicial += 1;
        }

        if (inicial == waypoints.Length)
        {
            inicial = 3;
        }

        if (targetTime <= 0.0f)
        {
            targetTime = 40f;
            mov = 0;
            inicial = 0;
            fij = 0;
        }
    }


    //Funcion que toma gaze points
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
