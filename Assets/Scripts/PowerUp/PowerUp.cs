using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp 
{
    //Nombre del PowerUp
    public string name;
    //Cantidad de aumento de velocidad
    public float speedBoostAmount;
    //Cantidad de aumento de daño del proyectil
    public float damageBoostAmount;

    //Constructor para inicializar el PowerUp con sus atributos
    public PowerUp(string _name, float _speedBoostAmount, float _damageBoostAmount)
    {
        name = _name;
        speedBoostAmount = _speedBoostAmount;
        damageBoostAmount = _damageBoostAmount;
    }

    // Método para aplicar el PowerUp al jugador
    public void ApplyToPlayer(PlayerController player)
    {
        player.ApplySpeedBoost(speedBoostAmount);
    }

    // Método para aplicar el PowerUp al proyectil
    public void ApplyToProjectile(Projectile projectile)
    {
        projectile.ApplyDamageBoost(damageBoostAmount);
    }
    // Método para aplicar el PowerUp al proyectil
    public void ApplyToLaserVeam(LaserBeam laser)
    {
        laser.ApplyDamageBoost(damageBoostAmount);
    }
}
