using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform target; // El objeto alrededor del cual la cámara va a rotar
    public float rotationSpeed = 5.0f; // Velocidad de rotación de la cámara

    public float minX = -10f; // Límite mínimo en el eje X
    public float maxX = 10f; // Límite máximo en el eje X
    public float minZ = -10f; // Límite mínimo en el eje Z
    public float maxZ = 10f; // Límite máximo en el eje Z
    public float maxDistance = 20f; // Distancia máxima de la cámara


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

        // Limitar la posición de la cámara dentro de los límites del mapa
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        // Limitar la distancia de la cámara al objetivo
        newPosition = Vector3.ClampMagnitude(newPosition - target.position, maxDistance) + target.position;

        transform.position = newPosition; // Movemos la cámara a la nueva posición
        transform.LookAt(target.position); // Hacemos que la cámara mire hacia el objetivo
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar los límites del mapa
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, 0, minZ), new Vector3(minX, 0, maxZ));
        Gizmos.DrawLine(new Vector3(minX, 0, maxZ), new Vector3(maxX, 0, maxZ));
        Gizmos.DrawLine(new Vector3(maxX, 0, maxZ), new Vector3(maxX, 0, minZ));
        Gizmos.DrawLine(new Vector3(maxX, 0, minZ), new Vector3(minX, 0, minZ));

        // Dibujar la distancia máxima de la cámara
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(target.position, maxDistance);
    }
}

