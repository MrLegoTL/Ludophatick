using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //Array de prefabs de enemigos
    public GameObject[] enemyPrefabs;

    // Variable para verificar si el punto de spawn está activo
    public bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy(enemyPrefabs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(GameObject[] enemyPrefabs)
    {
        // Verificar si hay prefabs de enemigos asignados
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No se han asignado prefabs de enemigos.");
            return;
        }
        //Selelciona alaetoriamente un prefab de enemigos del array
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        //Instancia el enemigo en la posicion y rotacion del SpawnPoint
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    // Método para activar el punto de spawn
    public void Activate()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    // Método para desactivar el punto de spawn
    public void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
