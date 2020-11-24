using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeOc_Boton_Ob : MonoBehaviour
{
   // Start is called before the first frame update
    [SerializeField]
    Transform[] waypoints;
    private float targetTime = 60f; //defino mi cronometro de 30 seg
    private float speed = 2f; //defino velocidad fija
    int mov = 0; //defino bandera
    int inicial = 0;//variable de índice que realice un seguimiento del 
    //punto de ruta hacia el que la bola está yendo actualmente

    int fijar = 0; // bandera para el retardo
    private float cont = 30; // tiempo de retardo inicial
    int cambio_es = 0; // bandera para cambio de escena

    void Start() {
       transform.position = waypoints[inicial].transform.position; //defino la posicion inicial en 0,0
    }
    
    void Update() {
        if (fijar == 0){
            Retardo();
        }

        else{
        Tiempo_ob() ;  
        //aca hice locuras para imprimir la posicion del boton
        print("posicion: " + transform.position);
        print("Tiempo: " + Time.deltaTime);
        //hasta aca
        }     
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
            targetTime = 60f;
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
            targetTime = 60f;
            mov = 0;
            inicial=0;
            fijar = 0;

        }
        
    }
}
