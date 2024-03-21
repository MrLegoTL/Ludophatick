using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    //variable para controlar la fuerza de despedida
    public float launchForce = 500f;
    public void BossDead()
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
        GameManager.instance.blockWave = false;
    }
}
