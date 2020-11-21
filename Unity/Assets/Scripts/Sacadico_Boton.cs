using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sacadico_Boton : MonoBehaviour
{
    private float targetTime = 10.0f; //Tiempo límite de fijación
    public int i = 1;
    float[] coordenada;
    Rigidbody boton_rojo;

    // Start is called before the first frame update
    void Start()
    {
        //coordenada = 
        boton_rojo = GetComponent<Rigidbody>();
        boton_rojo.position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        Debug.Log(targetTime);

        if (targetTime <= 0.0f)
        {
            if (i < 10)
            {
                timerEnded(i);
                i += 1;
            }
            else
            {
                SceneManager.LoadScene("EscenaInicio");
            }


        }

    }

    void timerEnded(int i)
    {
        switch (i)
        {
            case 1: 
                boton_rojo.position = new Vector3(-5.0f, 4.0f, 0.0f);
                break;
            case 2:
                boton_rojo.position = new Vector3(-5.0f, 0.0f, 0.0f);
                break;
            case 3:
                boton_rojo.position = new Vector3(-5.0f, -4.0f, 0.0f);
                break;
            case 4:
                boton_rojo.position = new Vector3(0.0f, 4.0f, 0.0f);
                break;
            case 5:
                boton_rojo.position = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case 6:
                boton_rojo.position = new Vector3(0.0f, -4.0f, 0.0f);
                break;
            case 7:
                boton_rojo.position = new Vector3(5.0f, 4.0f, 0.0f);
                break;
            case 8:
                boton_rojo.position = new Vector3(5.0f, 0.0f, 0.0f);
                break;
            case 9:
                boton_rojo.position = new Vector3(5.0f, -4.0f, 0.0f);
                break;
        }
        
        targetTime = 10.0f;

    }

}