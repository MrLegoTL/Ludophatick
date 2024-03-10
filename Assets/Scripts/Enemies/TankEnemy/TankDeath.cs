using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankDeath : EnemyHealth
{
    //variable para controlar la fuerza de despedida
    public float launchForce = 500f;
    public bool angryMode = false;
    //unity event para lanzar efectos al morir
    public UnityEvent OnAngryEvent;

    private void Update()
    {
        TankAngry();
       
    }

    public void TankDead()
    {
        Invoke("LaunchEnemy", 1f);
    }
    public void LaunchEnemy()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }
    }
   
    public void TankAngry()
    {
        if (currentHealth <= 30 && !angryMode)
        {
            Enemy.instance.enemyShoot = false;
            OnAngryEvent?.Invoke();
            angryMode = true;
            

        }
       
        
    }
}

   
