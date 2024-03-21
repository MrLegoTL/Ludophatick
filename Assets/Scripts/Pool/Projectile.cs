using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Projectile : PoolEntity
{
    [Header("Components")]
    public Collider collider;
    public Rigidbody rigidBody;
    public ParticleSystem trail;

    [Header("Projectile")]
    //daño de projectil
    public float damage = 10f;
    
                              
    //velocidad de desplazamiento
    public float speed = 10f;
    //tiempo de vidas antes de autodestruise
    public float lifeTime = 5f;
    //para gestionar el tiempo de vida
    private float lifeTimeStamp;
    //layers contra los que colisionara
    public LayerMask shootableLayer;

    //action que informara de la posicion de impacto
    public Action<Vector3> onImpact;
    public Action<Vector3> onImpactEnemy;
    //action que se invocara cuando se inicialice el proyectil
    public Action onInitialize;

    //public static Projectile instance;

    private void Awake()
    {
        //if (instance == null) instance = this;
    }
    private void Start()
    {
        // currentDamage = damage;
        damage = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        // si ha pasado su teimpo de vida , devolvemos el proyectil a la pool
        if (lifeTimeStamp < Time.time && active) ReturnPool();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if ((shootableLayer & (1 << other.gameObject.layer)) != 0)
        {
            IDamageable<float> damageable;

            //tratamos de recueprar el componente IDamageable del objeto impactado
            if(other.TryGetComponent<IDamageable<float>>(out damageable))
            {
                //si es posible recuperarlo, significara que el objeto es dañable
                //por tanto, le aplico el daño correspondiente
                damageable.TakeDamage(damage, transform.position);
                onImpactEnemy?.Invoke(transform.position);
            }
            //invocamos el action informando de la posicion actual del proyectil en el momento de impactar
            onImpact?.Invoke(transform.position);
            
            
                
            
            ReturnPool();
        }
    }

    /// <summary>
    /// Recupera los componentes necesarios
    /// </summary>
    [ContextMenu("GetComponents")]
    public void GetComponents()
    {
        collider = GetComponent<Collider>();
        rigidBody = GetComponent<Rigidbody>();
        trail = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Inicializa los componentes especificos del proyectil
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        //si hay metoddos suscritos al action, lo invocamos
        onInitialize?.Invoke();
        collider.enabled = true;
        rigidBody.isKinematic = false;
        trail.Play();
        //lanzamos el proyectil segun sus propios valores
        rigidBody.velocity = transform.forward * speed;
        //calculamos la marce de tiempo a revisar para autodestruir el proyectil
        lifeTimeStamp = Time.time + lifeTime;


       
    }

    public override void Deactivate()
    {
        base.Deactivate();

        collider.enabled = false;
        rigidBody.isKinematic = true;
        trail.Stop();

    }

    

   
   
}
