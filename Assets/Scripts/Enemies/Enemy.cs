using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para hacer uso del nav mesh
using UnityEngine.AI;
//para hacer uso de los unity events
using UnityEngine.Events;

public class Enemy : PoolEntity
{
    //nombre del tag objetivo a buscar
    public string targetTagName = "Player";
    //trnasforma del objetivo a perseguir
    public Transform target;
    //refrencia la navmesh agent
    public NavMeshAgent nav;
    //referencia public al animator que gestionara la maquina de estado
    public Animator animator;

    //distancia  a la que iniciara el ataque
    public float attackDistance = 10f;
    //nombre de la pool de proyectile a utilizar
    public string enemyProjectile = "EnemyBullets";
    //punto d eorigen de los disparos
    public Transform shootingPoint;
    //velocidad de giro en estado de ataque
    public float enemyAttackTurnSpeed = 5;
    public bool enemyShoot = true;

    public UnityEvent OnInitialize;
    public UnityEvent OnDeactivate;

    public static Enemy instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTarget(targetTagName);
    }

    /// <summary>
    /// Busca el transform del objetivo con el tag indicado
    /// </summary>
    /// <param name="tag"></param>
    public void CheckForTarget(string tag)
    {
        //recorremos todos los objetivos de la escena con el tag indicado
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(tag);

        //recorremnos todos los objetivos posibles
        foreach(GameObject possibleTarget in possibleTargets)
        {
            //si no hay ninguno previamente seleccionado
            if(target == null)
            {
                //seleccionamos el primero
                target = possibleTarget.transform;
            }
            else if(Vector3.Distance(gameObject.transform.position,target.position) > 
                    Vector3.Distance(gameObject.transform.position, possibleTarget.transform.position))
            {
                target = possibleTarget.transform;
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        //para asegurarnos que se posiciona correctamente en el navmesh al sacarlo del pool
        nav.Warp(transform.position);
        OnInitialize?.Invoke();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        OnDeactivate?.Invoke();
    }
}
