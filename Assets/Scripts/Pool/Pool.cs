using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool 
{
    //id de la pool
    public string id;
    // prefab del elemento que será almacenado en la pool
    public PoolEntity prefab;
    // numero de poolentity inicial a generar
    public int prewarm;
    //Cola de elementos disponible en la pool
    public Queue<PoolEntity> pool = new Queue<PoolEntity>();
}
