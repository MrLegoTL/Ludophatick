using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCooldown : CoolDownUI
{
    public override void OnEnable()
    {
        base.OnEnable();
        PlayerController.onShootLaser += StartTimer;
    }

    public override void OnDisable() 
    { 
        base.OnDisable();
        PlayerController.onShootLaser -= StartTimer;
    }
}
