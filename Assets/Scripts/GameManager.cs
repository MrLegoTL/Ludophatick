using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Array de puntos de spawn disponibles en tu juego
    public SpawnPoint[] spawnPoints;

    // Array de prefabs de enemigos disponibles en tu juego
    public GameObject[] enemyPrefabs;
    
    // Lista de oleadas generadas automáticamente
    public List<Wave> waves = new List<Wave>();

    //public int enemiesPerWave = 5; // Asigna un valor inicial a enemiesPerWave

    private int currentWaveIndex = 0;

    private int activeEnemyCount = 0;

    [Header("Increase Difficulty")]
    public int wavesBetweenDifficultyIncrease = 5;
    public float enemyHealtIncreaseFactor = 1.2f;
    public float enemyDamageIncreaseFactor = 1.2f;
    public int enemiesPerWavesIncrease = 2;
    public Projectile projectilePrefab;

    public int completeWaves = 0;

    [Header("Money Manager")]
    //Dinero del jugador
    public int moneyCount = 0;
    //Referencia al texto del dinero
    public TMP_Text moneyText;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateWaves();
        StartFirstWave();
        //completeWaves++;
        //if(completeWaves % wavesBetweenDifficultyIncrease == 0)
        //{
        //    IncreaseDiffuculty();
        //}
    }
    private void Update()
    {
        UpdateMoneyUI();
    }
    public void StartFirstWave()
    {
        StartWave();
    }
    public void EnemySpawned()
    {
        activeEnemyCount++;
    }
    public void EnemyDied()
    {
        activeEnemyCount--;

        if (activeEnemyCount <= 0)
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            StartWave();
        }
        else
        {
            // No hay más oleadas, el juego ha terminado
            Debug.Log("¡Fin del juego!");
        }
    }
    public void StartWave()
    {
        //completeWaves++;
        //if (completeWaves % wavesBetweenDifficultyIncrease == 0)
        //{
        //    IncreaseDiffuculty();
        //}
        // Activar los SpawnPoints necesarios para la oleada actual
        activeEnemyCount = CalculateTotalEnemiesForCurrentWave();
        ActivateSpawnPointsForCurrentWave();
        SpawnEnemiesForCurrentWave();
    }

    private int CalculateTotalEnemiesForCurrentWave()
    {
        int totalEnemies = 0;
        foreach (GameObject enemyPrefab in waves[currentWaveIndex].enemyTypes)
        {
            totalEnemies += waves[currentWaveIndex].enemiesPerWave;
        }
        return totalEnemies;
    }

    private void ActivateSpawnPointsForCurrentWave()
    {
        // Obtener la oleada actual
        Wave currentWave = waves[currentWaveIndex];

        // Verificar si la oleada actual tiene puntos de spawn
        if (currentWave != null && currentWave.spawnPoints != null)
        {
            // Iterar sobre los índices de los puntos de spawn de la oleada actual
            foreach (int spawnPointIndex in currentWave.spawnPoints)
            {
                if (spawnPoints[spawnPointIndex] != null)
                {
                    spawnPoints[spawnPointIndex].Activate();
                }
                else
                {
                    Debug.LogWarning("El punto de spawn en el índice " + spawnPointIndex + " es nulo.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No se han definido puntos de spawn para la oleada actual.");
        }
    }

    private void SpawnEnemiesForCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];
        foreach (GameObject enemyPrefab in currentWave.enemyTypes)
        {
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.isActive)
                {
                    spawnPoint.SpawnEnemy(enemyPrefabs);
                }
            }
        }
    }


    private void GenerateWaves()
    {
        // Lógica para generar las oleadas automáticamente
        for (int i = 0; i < 5; i++)
        {
            Wave newWave = new Wave();
            newWave.enemyTypes = GenerateEnemyTypesForWave(); // Implementa este método para generar tipos de enemigos
            newWave.spawnPoints = GenerateSpawnPointsForWave(); // Implementa este método para generar puntos de spawn
            newWave.enemiesPerWave = 5 + i * enemiesPerWavesIncrease; // Ajusta la cantidad de enemigos por oleada
            waves.Add(newWave);
        }
    }

    private GameObject[] GenerateEnemyTypesForWave()
    {
        List<GameObject> selectedEnemies = new List<GameObject>();
        // Obtener una cantidad específica de prefabs de enemigos para la oleada actual
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            // Seleccionar aleatoriamente un prefab de enemigo de la lista
            GameObject enemyPrefab = enemyPrefabs[i];
            selectedEnemies.Add(enemyPrefab);
        }

        return selectedEnemies.ToArray();
        
    }

    private int[] GenerateSpawnPointsForWave()
    {
        List<int> selectedSpawnPoints = new List<int>();
        // Lógica para seleccionar los puntos de spawn disponibles en cada oleada
        // En este ejemplo, seleccionamos aleatoriamente algunos puntos de spawn
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Random.value < 0.5f) // Probabilidad del 50%
            {
                selectedSpawnPoints.Add(i);
            }
        }
        return selectedSpawnPoints.ToArray();
    }

    //private int CalculateEnemiesPerWave(int waveIndex)
    //{
    //    // Calculamos la cantidad base de enemigos por oleada
    //    int baseEnemiesPerWave = 10;

    //    // Aumentamos la cantidad de enemigos por oleada en función del número de oleadas completadas
    //    int totalEnemiesPerWave = baseEnemiesPerWave + waves.Count * enemiesPerWavesIncrease;

    //    return totalEnemiesPerWave;
    //}

    

    //private void IncreaseDiffuculty()
    //{

    //    foreach (Wave wave in waves)
    //    {
    //        //Aumentar el numero de enemigos por oleadas
    //        wave.enemiesPerWave += enemiesPerWavesIncrease;

    //        //Aumentar la salud d elos enemigos
    //        foreach (GameObject enemyPrefab in wave.enemyTypes)
    //        {
    //            EnemyHealth enemyHealth = enemyPrefab.GetComponent<EnemyHealth>();
    //            if (enemyHealth != null)
    //            {
    //                enemyHealth.maxHealth *= enemyHealtIncreaseFactor;
    //            }
    //        }

    //        //aumentar el daño de los enemigos
    //        foreach (GameObject enemyPrefab in wave.enemyTypes)
    //        {
    //            if (projectilePrefab != null)
    //            {
    //                projectilePrefab.damage *= enemyDamageIncreaseFactor;
    //            }
    //        }

    //    }
    //}

    //Estructura de datos para representar una oleada
    [System.Serializable]
    public class Wave
    {
        //Indices de SpawnPoints activados para esta oleada
        public int[] spawnPoints;
        //Tipos de enemigos que apareceran en esta oleada
        public GameObject[] enemyTypes;
        //Variable para el nuemro de enemigos por olead
        public int enemiesPerWave;
    }

    public void CollectMoney(int amount)
    {
        //Aumenta el dinero del jugador
        moneyCount += amount;

        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        //Actualiza el exto del dinero del jugador
        if(moneyText != null) 
        {
            moneyText.text = moneyCount.ToString();
        }
    }
    // Método para verificar si el jugador tiene suficiente dinero para desbloquear
    public bool PlayerHasEnoughMoney(int amount)
    {
        return moneyCount >= amount;
    }

    // Método para gastar dinero del jugador
    public void SpendPlayerMoney(int amount)
    {
        if (PlayerHasEnoughMoney(amount))
        {
            moneyCount -= amount;
            // Aquí podrías agregar lógica adicional, como sonidos de dinero gastado o actualizaciones de UI.
        }
        else
        {
            Debug.LogWarning("El jugador no tiene suficiente dinero para gastar " + amount.ToString() + " monedas.");
        }
    }
}
