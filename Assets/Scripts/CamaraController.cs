using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform target; // El objeto alrededor del cual la cámara va a rotar
    public float rotationSpeed = 5.0f; // Velocidad de rotación de la cámara

    private Vector3 offset; // Distancia inicial entre la cámara y el objetivo

    void Start()
    {
        offset = transform.position - target.position; // Calculamos la distancia inicial
    }

    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Capturamos la entrada de movimiento horizontal

        float rotation = horizontalInput * rotationSpeed * Time.deltaTime; // Calculamos la rotación basada en la entrada de movimiento horizontal

        Quaternion rotationQuaternion = Quaternion.Euler(0, rotation, 0); // Convertimos la rotación a un Quaternion

        offset = rotationQuaternion * offset; // Aplicamos la rotación al offset de la cámara

        Vector3 newPosition = target.position + offset; // Calculamos la nueva posición de la cámara

        transform.position = newPosition; // Movemos la cámara a la nueva posición
        transform.LookAt(target.position); // Hacemos que la cámara mire hacia el objetivo
    }
}

