using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LaserBeam : PoolEntity
{
    //public GameObject firePoint;

    [Header("Components")]
    public LineRenderer lineRenderer;

    [Header("Laser")]
    // Da�o del l�ser
    public float damage = 0.1f;
    public float currentDamage;
    // Longitud m�xima del l�ser
    public float maxLength = 1000f;
    // Capa de objetos a los que puede da�ar el l�ser
    public LayerMask shootableLayer;
    // Duraci�n del l�ser antes de desaparecer
    public float laserDuration = 1.0f;
    // Tiempo en el que se activ� el l�ser
    public float startTime;
    public ParticleSystem impact;
    public ParticleSystem laserImpact;



    // Acci�n que informar� sobre la posici�n de impacto
    public Action<Vector3> onImpact;
    public Action<Vector3> onImpactEnemy;
    //action que se invocara cuando se inicialice el proyectil
    public Action onInitialize;

    public static LaserBeam instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        // Actualizamos el l�ser
        UpdateLaser();
        // Si ha pasado la duraci�n del l�ser, lo desactivamos
        if (Time.time - startTime >= laserDuration)
        {
            Deactivate();
        }


    }

   
    
    private void UpdateLaser()
    {
        // Inicializamos el rayo l�ser desde la posici�n actual con la direcci�n hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Configuramos el line renderer
        lineRenderer.SetPosition(0, ray.origin);

        // Si el rayo l�ser golpea algo
        if (Physics.Raycast(ray, out hit, maxLength, shootableLayer))
        {
            // Configuramos el final del line renderer en el punto de impacto
            lineRenderer.SetPosition(1, hit.point);

            // Si el objeto golpeado es da�able
            IDamageable<float> damageable = hit.collider.GetComponent<IDamageable<float>>();
            if (damageable != null)
            {
                // Aplicamos da�o al objeto
                damageable.TakeDamage(currentDamage, hit.point);
                onImpactEnemy?.Invoke(transform.position);
            }

            // Invocamos la acci�n de impacto informando sobre la posici�n de impacto
            onImpact?.Invoke(hit.point);

            if(impact != null)
            {
                impact.transform.position = hit.point;
            }
            if(laserImpact != null)
            {
                laserImpact.transform.position = hit.point;
            }
            
            
            
        }
        else
        {
            // Si el rayo l�ser no golpea nada, lo extendemos hasta su longitud m�xima
            lineRenderer.SetPosition(1, ray.GetPoint(maxLength));
            
        }



        transform.position = PlayerController.instance.shootingLaserPoint.transform.position;
        transform.forward = PlayerController.instance.transform.forward;

    }

   

    public override void Initialize()
    {
        base.Initialize();
        //si hay metoddos suscritos al action, lo invocamos
        onInitialize?.Invoke();
        // Habilitamos el line renderer
        lineRenderer.enabled = true;

        // Guardamos el tiempo de activaci�n del l�ser
        startTime = Time.time;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.SetActive(false);
        // Deshabilitamos el line renderer
        lineRenderer.enabled = false;

        
    }
    public void RestartDamageLaser()
    {
        currentDamage = damage;
    }
    public void ApplyDamageBoost(float damageBoostAmount)
    {
        currentDamage *= damageBoostAmount;
    }
}
