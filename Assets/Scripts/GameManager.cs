using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("Enemies")]
    //nombres  en la pool de los enemigos
    public string[] enemyList;
    //puntos de sapwn de los enemigos
    public Transform[] spawnPoints;

    [Header("Waves")]
    //retardo en la generacion de enemigos
    public float spawnDelay = 0.2f;
    //numero de enemigos a multiplicar por la oleada
    public int waveEnemyNumberMultiplier = 20;
    //enemigos en la oleada actual
    private int waveEnemies;
    //numeros de enemigos restantes de la oleada actual
    private int remainingEnemies;
    //numero de oleada actual
    public int currentWave = 0;
    //temporizador para la generacion de enemigos
    private float spawnTimer = 0f;
    //limite maximo de enemigos en escena
    public int maxEnemiesOnScene = 15;
    //contador de enemigos actuales en la escena
    private int enemiesOnScene;

    [Header("Money Manager")]
    //Dinero del jugador
    public int moneyCount = 0;
    //Referencia al texto del dinero
    public TMP_Text moneyText;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //iniciamos la primera oleada al momento de comenzar la partida
        NewWave();
    }
    private void OnEnable()
    {
        EnemyHealth.OnDead += EnemyDead;
    }
    private void OnDisable()
    {
        EnemyHealth.OnDead -= EnemyDead;
    }
    private void Update()
    {
        //solo contamos si el temporizador no ha llegado al final
        if (spawnTimer <= spawnDelay)
        {

            spawnTimer += Time.deltaTime;
        }

        //si quedan enemigos por generar y ha pasado el tiempo de retardo entre enemigos
        if (waveEnemies > 0 &&
           spawnTimer > spawnDelay &&
           enemiesOnScene < maxEnemiesOnScene)
        {
            //generamos el nuevo enemigo
            GeneratedEnemy(enemyList);
            //resetamos el contador
            spawnTimer = 0;
        }

        //si han muerto todos los enemigos de la oleada actual
        if (remainingEnemies <= 0)
        {
            //iniciamos una nueva oleada
            NewWave();
        }
        UpdateMoneyUI();
    }

    /// <summary>
    /// Genera un enemigo aleatorio a partir de las lista recibida
    /// pudiendo ser una lista de distintos tipos de enemigos o bosses
    /// </summary>
    /// <param name="enemyPool"></param>
    public void GeneratedEnemy(string[] enemyPool)
    {
        if (spawnPoints.Length == 0 || enemyPool.Length == 0)
        {
            Debug.LogError("Te has olvidado de configurar o los Spawns o los enemigos");
            return;
        }

        int randomSpawn = Random.Range(0, spawnPoints.Length);
        int randomEnemy = Random.Range(0, enemyPool.Length);

        Transform[] spawnActivate = spawnPoints.Where(s => s.gameObject.activeSelf == true).ToArray();
        int random = Random.Range(0, spawnActivate.Length);



        PoolManager.instance.Pull(enemyPool[randomEnemy],
                              spawnActivate[random].position,
                              Quaternion.identity);
        //tras generar un nuevo enemigo, decremento el contador de enemigos a generar
        waveEnemies--;
        //incrementamos el contador  de enemigos en escena
        enemiesOnScene++;




    }


    /// <summary>
    /// Metodo ejecutado cuando un enemigo muere
    /// </summary>
    public void EnemyDead()
    {
        //reducimos el numero de enemigos restantes de la oleada
        remainingEnemies--;
        //decrementamos el contador de enemigos en escena
        enemiesOnScene--;
    }

    /// <summary>
    /// Realiza todas las acciones necesarias para indicar una nueva oleada
    /// </summary>
    public void NewWave()
    {
        //incremento el numero de la oleada
        currentWave++;
        ////asignamos el valor del numero de oleada
        //waveNumber.text = currentWave.ToString();
        ////iniciamnos la aniamcion del hud de oleada
        //waveAnimator.SetTrigger("Show");
        //calculamos el numero de enemigos que seran generados en esta oleada
        waveEnemies = currentWave * waveEnemyNumberMultiplier;
        //el numero de enemigos restantes se inicializara con el numero de enemigos de la oleada
        remainingEnemies = waveEnemies;
        //inicializamos el contador de numero de enemigos presentes en la escena
        enemiesOnScene = 0;
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
        if (moneyText != null)
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
