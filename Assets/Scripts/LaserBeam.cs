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
    // Daño del láser
    public float damage = 10f;
    // Longitud máxima del láser
    public float maxLength = 1000f;
    // Capa de objetos a los que puede dañar el láser
    public LayerMask shootableLayer;
    // Duración del láser antes de desaparecer
    public float laserDuration = 1.0f;
    // Tiempo en el que se activó el láser
    public float startTime;
    public ParticleSystem impact;
    
    

    // Acción que informará sobre la posición de impacto
    public Action<Vector3> onImpact;
    //action que se invocara cuando se inicialice el proyectil
    public Action onInitialize;

    // Update is called once per frame
    void Update()
    {
        // Actualizamos el láser
        UpdateLaser();
        // Si ha pasado la duración del láser, lo desactivamos
        if (Time.time - startTime >= laserDuration)
        {
            Deactivate();
        }


    }

   
    
    private void UpdateLaser()
    {
        // Inicializamos el rayo láser desde la posición actual con la dirección hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Configuramos el line renderer
        lineRenderer.SetPosition(0, ray.origin);

        // Si el rayo láser golpea algo
        if (Physics.Raycast(ray, out hit, maxLength, shootableLayer))
        {
            // Configuramos el final del line renderer en el punto de impacto
            lineRenderer.SetPosition(1, hit.point);

            // Si el objeto golpeado es dañable
            IDamageable<float> damageable = hit.collider.GetComponent<IDamageable<float>>();
            if (damageable != null)
            {
                // Aplicamos daño al objeto
                damageable.TakeDamage(damage, hit.point);
            }

            // Invocamos la acción de impacto informando sobre la posición de impacto
            onImpact?.Invoke(hit.point);
            impact.transform.position = hit.point;
            
            
        }
        else
        {
            // Si el rayo láser no golpea nada, lo extendemos hasta su longitud máxima
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

        // Guardamos el tiempo de activación del láser
        startTime = Time.time;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        gameObject.SetActive(false);
        // Deshabilitamos el line renderer
        lineRenderer.enabled = false;

        
    }
}
