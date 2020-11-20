using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeOc_Boton_HV : MonoBehaviour
{
   // Start is called before the first frame update
    [SerializeField]
    Transform[] waypoints;
    private float targetTime = 60f; //defino mi cronometro de 30 seg
    private float speed = 2f; //defino velocidad fija
    int mov = 0; //defino bandera
    int inicial = 0;//variable de índice que realice un seguimiento del 
    //punto de ruta hacia el que la bola está yendo actualmente

    int fij = 0;
    private float cont = 10f;

    void Start() {
       transform.position = waypoints[inicial].transform.position; //defino la posicion inicial en 0,0
    }
    
    void Update() {
        if (fij == 0){
            Fijacion();
        }
        else{
        Tiempo() ;  
        //aca hice locuras para imprimir la posicion del boton
        print("posicion: " + transform.position);
        print("Tiempo: " + Time.deltaTime);
        //hasta aca
        }     
    }

    void Fijacion() {
        cont -= Time.deltaTime;
        if (cont <= 0.0f){
            cont = 10f;
            fij=1;
        }
    }
    void Tiempo() {
        if (mov == 0){
            Move_1();
        }
        else {
            Move_2();
        }
        
    }

    void Move_1() {//movimiento horizontal

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
    void Move_2() {//movimiento vertical

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
        }
        
    }
}
