using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

public class PathBlocker : MonoBehaviour
{
    //Costo de desbloquear camino
    public int unlockCost = 20;
    //Texto que indica al juagdor como desbloquear el camino
    public TMP_Text unlockText;

    //Unity Event para cuandos se desbloquee
    public UnityEvent onUnlock;
    public UnityEvent onDisable;
    //Indica si  el jugador esta dentro del area de interaccion
    public bool playerInRange = false;

    public static Action onUnlockZone;


    private void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AttempToUnlock();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            unlockText.gameObject.SetActive(true);
            UpdateUnlockText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            unlockText.gameObject.SetActive(false);
        }
    }

    public void UpdateUnlockText()
    {
        if (playerInRange)
        {
            if (GameManager.instance.PlayerHasEnoughMoney(unlockCost))
            {
                unlockText.color = Color.green;
                unlockText.text = "Pulse E para desbloquear por "+ unlockCost.ToString() + " $";
            }

            else
            {
                unlockText.color= Color.red;
                unlockText.text = "Pulse E para desbloquear por " + unlockCost.ToString() + " $";
            }
        }
    }

    void AttempToUnlock()
    {
        if (GameManager.instance.PlayerHasEnoughMoney(unlockCost))
        {
            // Descontar el dinero del jugador
            GameManager.instance.SpendPlayerMoney(unlockCost);

            onUnlock?.Invoke();

            Invoke("DisableObject", 1f);
            playerInRange = false;
        }
    }
    public void DisableObject()
    {
        onDisable?.Invoke();
        onUnlockZone?.Invoke();
    }


}
