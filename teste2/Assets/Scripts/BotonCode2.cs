﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonCode2 : MonoBehaviour{
    // Start is called before the first frame update
    [SerializeField]
    Transform[] waypoints;
    //para establecer puntos de referencia en el inspector
    [SerializeField]
    public float speed = 2f;
    //Velocidad a la que viaja la bolita
    int inicial = 0;
    //variable de índice que realice un seguimiento del 
    //punto de ruta hacia el que la bola está yendo actualmente
    void Start() {
       transform.position = waypoints[inicial].transform.position;
    }
    
    void Update() {
        Move() ;          
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards (transform.position,
                                                waypoints[inicial].transform.position,
                                                speed * Time.deltaTime);

        if (transform.position == waypoints[inicial].transform.position) {
            inicial += 1;

        }
        if (inicial == waypoints.Length)
        inicial = 0;
        
        
    }
}