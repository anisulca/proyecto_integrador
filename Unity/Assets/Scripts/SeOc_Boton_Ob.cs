using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Tobii.Gaming; //Tobii SDK
using System.Xml.Linq;
using System.IO;
using System.Text;

public class SeOc_Boton_Ob : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    private float targetTime = 40f; //defino mi cronometro de 30 seg
    private float speed = 2f; //defino velocidad fija
    int mov = 0; //defino bandera
    int inicial = 0;//variable de índice que realice un seguimiento del 
    //punto de ruta hacia el que la bola está yendo actualmente

    int fijar = 0; // bandera para el retardo
    private float cont = 15; // tiempo de retardo inicial
    int cambio_es = 0; // bandera para cambio de escena

    private GazePoint lastGazePoint = GazePoint.Invalid; //Se fija como valor del primer gaze point como Invalido

    //Variables relacionadas a la escritura del csv
    StringBuilder csvcontent = new StringBuilder();//crear archivo
    string csvpath = @"C:\Users\Gabriela\Documents\PROYECTO INTEGRADOR\CSV_Pruebas\Prueba1.csv";//direccion del archivo

    void Start() 
    {
       transform.position = waypoints[inicial].transform.position; //defino la posicion inicial en 0,0
       //Escribir encabezado del archivo csv
       csvcontent.AppendLine("PRUEBA DE SEGUIMIENTO SUAVE - MOVIMIENTO OBLICUO");
       csvcontent.AppendLine("TiemporReal; Coord_Estim; Coord_GazePoint; TimeStamp_GP");
       File.WriteAllText(csvpath, csvcontent.ToString());
    }
    
    void Update() 
    {
        GazePoint gazeData = GetGazeData(); //En cada actualizacion del frame se toma un gaze point

        //Movimiento del estimulo
        if (fijar == 0){
            Retardo();
        }

        else{
        Tiempo_ob();  
        }

        //Obtener datos
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
        csvcontent.Append(";");
        csvcontent.Append(coordEstimulo);
        csvcontent.Append(";");
        csvcontent.Append(coordGazePoint);
        csvcontent.Append(";");
        csvcontent.AppendLine(timeStampGazePoint.ToString());
        File.WriteAllText(csvpath, csvcontent.ToString());
    }


    void Retardo() { // retardo de 30 segundos al iniciar la prueba
        cont -= Time.deltaTime;
        if (cont <= 0.0f){
            cont = 10f;
            fijar=1;
            cambio_es += 1;
        }
        if (cambio_es == 2) {
            SceneManager.LoadScene ("EscenaInicio");
        }
    }


    void Tiempo_ob() {
        if (mov == 0){
            Moveob_1();
        }
        else {
            Moveob_2();
        }
        
    }

    void Moveob_1() {//movimiento horizontal

        targetTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards (transform.position,
                                                waypoints[inicial].transform.position,
                                                speed * Time.deltaTime);

        if (transform.position == waypoints[inicial].transform.position) {
            inicial += 1;

        }
        if (inicial == waypoints.Length - 2)
        inicial = 0;
        
        if (targetTime <= 0.0f){ //reestablesco valores
            targetTime = 40f;
            mov = 1;
            inicial = 3;
        }   
    }
    
    void Moveob_2() {//movimiento vertical

        targetTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards (transform.position,
                                                waypoints[inicial].transform.position,
                                                speed * Time.deltaTime);

        if (transform.position == waypoints[inicial].transform.position) {
            inicial += 1;

        }
        if (inicial == waypoints.Length)
        inicial = 3;

        if (targetTime <= 0.0f){
            targetTime = 40f;
            mov = 0;
            inicial=0;
            fijar = 0;

        }
        
    }

    //Función que toma gaze points
    public GazePoint GetGazeData() //Función que toma un gaze point, lo valida y lo devuelve a Update
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
