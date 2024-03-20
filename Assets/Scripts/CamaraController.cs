using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform target; // El objeto alrededor del cual la c�mara va a rotar
    public float rotationSpeed = 5.0f; // Velocidad de rotaci�n de la c�mara

    public float minX = -10f; // L�mite m�nimo en el eje X
    public float maxX = 10f; // L�mite m�ximo en el eje X
    public float minZ = -10f; // L�mite m�nimo en el eje Z
    public float maxZ = 10f; // L�mite m�ximo en el eje Z
    public float maxDistance = 20f; // Distancia m�xima de la c�mara


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

        // Limitar la posici�n de la c�mara dentro de los l�mites del mapa
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        // Limitar la distancia de la c�mara al objetivo
        newPosition = Vector3.ClampMagnitude(newPosition - target.position, maxDistance) + target.position;

        transform.position = newPosition; // Movemos la c�mara a la nueva posici�n
        transform.LookAt(target.position); // Hacemos que la c�mara mire hacia el objetivo
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar los l�mites del mapa
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, 0, minZ), new Vector3(minX, 0, maxZ));
        Gizmos.DrawLine(new Vector3(minX, 0, maxZ), new Vector3(maxX, 0, maxZ));
        Gizmos.DrawLine(new Vector3(maxX, 0, maxZ), new Vector3(maxX, 0, minZ));
        Gizmos.DrawLine(new Vector3(maxX, 0, minZ), new Vector3(minX, 0, minZ));

        // Dibujar la distancia m�xima de la c�mara
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(target.position, maxDistance);
    }
}

