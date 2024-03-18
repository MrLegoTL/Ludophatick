using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //Array de prefabs de enemigos
    public GameObject[] enemyPrefabs;
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
}
