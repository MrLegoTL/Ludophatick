using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//para hacer uso de los actions
using System;

public class BossManager : MonoBehaviour
{
    //public bool fightBoss = false;
    public GameObject boss;
    public UnityEvent onEntryInZone;
    public UnityEvent onStartFightBoss;

    public static Action onFightBoss;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EntryInZone();
            Invoke("FightBoss", 1f);
        }
    }

    public void EntryInZone()
    {
        onEntryInZone?.Invoke();
        onFightBoss?.Invoke();
        GameManager.instance.blockWave = true;
    }
    public void FightBoss()
    {
        //boss.SetActive(true); 
        onStartFightBoss?.Invoke();
    }
}
