using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    //punto d eorigen de los disparos
    public Transform shootingPointUpRight;
    //punto d eorigen de los disparos
    public Transform shootingPointDownLeft;
    //punto d eorigen de los disparos
    public Transform shootingPointDownRight;

    public string enemyLaser = "LaserBeamEnemy";
    public Transform shootingPointLaser;

}
