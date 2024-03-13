using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform target; // El objeto alrededor del cual la c�mara va a rotar
    public float rotationSpeed = 5.0f; // Velocidad de rotaci�n de la c�mara

    private Vector3 offset; // Distancia inicial entre la c�mara y el objetivo

    void Start()
    {
        offset = transform.position - target.position; // Calculamos la distancia inicial
    }

    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Capturamos la entrada de movimiento horizontal

        float rotation = horizontalInput * rotationSpeed * Time.deltaTime; // Calculamos la rotaci�n basada en la entrada de movimiento horizontal

        Quaternion rotationQuaternion = Quaternion.Euler(0, rotation, 0); // Convertimos la rotaci�n a un Quaternion

        offset = rotationQuaternion * offset; // Aplicamos la rotaci�n al offset de la c�mara

        Vector3 newPosition = target.position + offset; // Calculamos la nueva posici�n de la c�mara

        transform.position = newPosition; // Movemos la c�mara a la nueva posici�n
        transform.LookAt(target.position); // Hacemos que la c�mara mire hacia el objetivo
    }
}

