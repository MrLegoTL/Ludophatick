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
    public Wave[] waves;

    private int currentWaveIndex = 0;

    [Header("Increase Difficulty")]
    public int wavesBetweenDifficultyIncrease = 5;
    public float enemyHealtIncreaseFactor = 1.2f;
    public float enemyDamageIncreaseFactor = 1.2f;
    public int enemiesPerWavesIncrease = 2;
    public Projectile projectilePrefab;

    private int completeWaves = 0;

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
        StartWave();

        completeWaves++;
        if(completeWaves % wavesBetweenDifficultyIncrease == 0)
        {
            IncreaseDiffuculty();
        }
    }

   public void StartWave()
    {
        // Activar los SpawnPoints necesarios para la oleada actual
        ActivateSpawnPoint(waves[currentWaveIndex].spawnPoints);
        // Hacer aparecer los enemigos correspondientes
        SpawnEnemies(waves[currentWaveIndex].enemyTypes);
    }

    public void ActivateSpawnPoint(int[] spawnPointIndices)
    {
        //Activar los SpawnPoints segun los indices proporcionados
        foreach (int index in spawnPointIndices)
        {
            spawnPoints[index].SetActive(true);
        }
    }

    public void SpawnEnemies(GameObject[] enemyPrefabs)
    {

        foreach(GameObject spawnPoint in spawnPoints)
        {
            spawnPoint.GetComponent<SpawnPoint>().SpawnEnemy(waves[currentWaveIndex].enemyTypes);
        }
    }

    private void IncreaseDiffuculty()
    {
        foreach(Wave wave in waves)
        {
            //Aumentar el numero de enemigos por oleadas
            wave.enemiesPerWaves += enemiesPerWavesIncrease;

            //Aumentar la salud d elos enemigos
            foreach(GameObject enemyPrefab in wave.enemyTypes)
            {
                EnemyHealth enemyHealth = enemyPrefab.GetComponent<EnemyHealth>();
                if(enemyHealth != null)
                {
                    enemyHealth.maxHealth *= enemyHealtIncreaseFactor;
                }
            }

            //aumentar el daño de los enemigos
            foreach (GameObject enemyPrefab in wave.enemyTypes)
            {
                if(projectilePrefab != null)
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
}
