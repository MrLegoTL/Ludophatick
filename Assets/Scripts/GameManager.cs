using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Array de SpawnPoints
    public GameObject[] spawnPoints;
    //Array de oleadas
    //public Wave[] waves;ç
    // Lista de oleadas generadas automáticamente
    public List<Wave> waves = new List<Wave>();

    private int currentWaveIndex = 0;

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


    public void StartWave()
    {
        //completeWaves++;
        //if (completeWaves % wavesBetweenDifficultyIncrease == 0)
        //{
        //    IncreaseDiffuculty();
        //}
        // Activar los SpawnPoints necesarios para la oleada actual
        ActivateSpawnPoint(waves[currentWaveIndex].spawnPoints);
        // Hacer aparecer los enemigos correspondientes
        SpawnEnemies(waves[currentWaveIndex].enemyTypes);
    }

    public void ActivateSpawnPoint(int[] spawnPointIndices)
    {
        // Debug para verificar la longitud del array spawnPoints
        Debug.Log("Longitud de spawnPoints: " + spawnPoints.Length);
        //Activar los SpawnPoints segun los indices proporcionados
        foreach (int index in spawnPointIndices)
        {
            // Debug para verificar los índices utilizados
            Debug.Log("Índice utilizado: " + index);
            // Asegurar que el índice esté dentro del rango válido
            if (index >= 0 && index < spawnPoints.Length)
            {
                spawnPoints[index].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Índice fuera del rango válido: " + index);
            }
        }
    }

    public void SpawnEnemies(GameObject[] enemyPrefabs)
    {

        foreach (GameObject spawnPoint in spawnPoints)
        {
            spawnPoint.GetComponent<SpawnPoint>().SpawnEnemy(waves[currentWaveIndex].enemyTypes);
        }
    }

    private void GenerateWaves()
    {
        // Lógica para generar las oleadas automáticamente
        for (int i = 0; i < wavesBetweenDifficultyIncrease; i++)
        {
            Wave newWave = new Wave();
            newWave.enemyTypes = GenerateEnemyTypesForWave(); // Implementa este método para generar tipos de enemigos
            newWave.spawnPoints = GenerateSpawnPointsForWave(); // Implementa este método para generar puntos de spawn
            newWave.enemiesPerWaves = CalculateEnemiesPerWave(i); // Implementa este método para calcular la cantidad de enemigos por oleada
            waves.Add(newWave);
        }
    }

    private GameObject[] GenerateEnemyTypesForWave()
    {
        // Implementa la lógica para generar tipos de enemigos para cada oleada
        // Puedes utilizar listas, arrays u otros métodos para generar enemigos dinámicamente
        // Por ejemplo, puedes seleccionar aleatoriamente tipos de enemigos de una lista predefinida
        // o ajustar la selección de enemigos según la dificultad actual del juego
        // En este ejemplo, simplemente devolvemos una lista vacía
        return new GameObject[0];
    }

    private int[] GenerateSpawnPointsForWave()
    {
        // Implementa la lógica para generar puntos de spawn para cada oleada
        // Puedes seleccionar aleatoriamente puntos de spawn de una lista predefinida
        // o ajustar la selección de puntos de spawn según la dificultad actual del juego
        // En este ejemplo, simplemente devolvemos una lista vacía
        return new int[0];
    }

    private int CalculateEnemiesPerWave(int waveIndex)
    {
        // Implementa la lógica para calcular la cantidad de enemigos por oleada
        // Puedes ajustar la cantidad de enemigos según la dificultad actual del juego
        // En este ejemplo, simplemente devolvemos un valor fijo
        return 10 + waveIndex * enemiesPerWavesIncrease;
    }

    private void IncreaseDiffuculty()
    {

        foreach (Wave wave in waves)
        {
            //Aumentar el numero de enemigos por oleadas
            wave.enemiesPerWaves += enemiesPerWavesIncrease;

            //Aumentar la salud d elos enemigos
            foreach (GameObject enemyPrefab in wave.enemyTypes)
            {
                EnemyHealth enemyHealth = enemyPrefab.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.maxHealth *= enemyHealtIncreaseFactor;
                }
            }

            //aumentar el daño de los enemigos
            foreach (GameObject enemyPrefab in wave.enemyTypes)
            {
                if (projectilePrefab != null)
                {
                    projectilePrefab.damage *= enemyDamageIncreaseFactor;
                }
            }

        }
    }

    //Estructura de datos para representar una oleada
    [System.Serializable]
    public class Wave
    {
        //Indices de SpawnPoints activados para esta oleada
        public int[] spawnPoints;
        //Tipos de enemigos que apareceran en esta oleada
        public GameObject[] enemyTypes;
        //Variable para el nuemro de enemigos por olead
        public int enemiesPerWaves;
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
