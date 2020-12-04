using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Tobii.Gaming;

public class SeOc_Boton_HV : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform[] waypoints; //puntos de referencia
    private float targetTime = 40f; //defino mi cronometro de 40 seg
    private float speed = 2f; //defino velocidad fija
    int mov = 0; //defino bandera
    int inicial = 0;//variable de índice que realice un seguimiento del 
    //punto de ruta hacia el que la bola está yendo actualmente

    int fij = 0; // bandera para el retardo
    private float cont = 15; // tiempo de retardo inicial en segundos
    int cambio_escena = 0; // bandera para cambio de escena

    private GazePoint lastGazePoint = GazePoint.Invalid;

    void Start()
    {
        transform.position = waypoints[inicial].transform.position; //defino la posicion inicial en 0,0
                                                                    //abrir - open
    }

    void Update()
    {
        String timeStamp = DateTime.Now.ToString("HHmmssffff"); // tiempo de maquina en ese formato hora,minuto,segundos,milisegundos
        GazePoint gazeData = GetGazeData();

        print(timeStamp); //imprime tiempo
        print("posicion: " + transform.position);//imprime posicion
        Debug.Log("Coordenadas gaze point: " + gazeData.Screen);
        Debug.Log("Timestamp gaze point: " + gazeData.Timestamp);

        //escribir
        if (fij == 0)
        {
            Fijacion();
        }

        else
        {
            Tiempo();

        }
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
            inicial = 3;

        if (targetTime <= 0.0f)
        {
            targetTime = 40f;
            mov = 0;
            inicial = 0;
            fij = 0;
        }
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
