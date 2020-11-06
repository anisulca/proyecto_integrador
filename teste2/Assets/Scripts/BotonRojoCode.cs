using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonRojoCode : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 30; //La variable velocidad es publica para que no 
    //sea necesario modificarla en el Script
    // Use this for initialization
    void Start () {

        GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;

    }

    // Update is called once per frame
    //void Update () {
   
        
    
}
