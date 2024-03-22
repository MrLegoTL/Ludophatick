using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para poder hacer uso de los unityevents
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable<float>
{
    //vida maxima del jugador
    public float maxHealth = 100;
    //vida actual del jugador
    public float currentHealth;
    public Animator anim;

    [Header("HUD")]
    //referencia a la imagen que muestra la vida actual
    public Image healthBar;

    [Header("OnDead")]
    //acciones y efectos a realizar cuando el jugador muera.
    public UnityEvent onDead;
    //action static para informar a los observadores de la muerte del jugador
    public static Action OnPlayerDead;

    //referencia a distintos componentes de control, para desactivarlos al morir
    public PlayerController playerController;
    //para controlar si esta muerto el jugador
    private bool isDead;
    //para controlar si el jugador ha sufrido daño
    private bool damaged;

    [Header("Cheats")]
    [SerializeField]
    private bool isImmune = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Devuelve true si el jugador esta muerto
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        //devuelve true si la vida actual es menos o igual a cero
        return currentHealth <= 0;
    }

    [ContextMenu("TestTakeDamage")]
    /// <summary>
    /// Para probar el take damage
    /// </summary>
    public void TestTakeDamage()
    {
        TakeDamage(30);
    }

    /// <summary>
    /// Aplica el daño recibido como parametro
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="impactPosition"></param>
    public void TakeDamage(float amount, Vector3 impactPosition = default(Vector3))
    {
        //si ya esta muerto no hacemos nada
        if (isDead) return;
        if (!isImmune)
        {
            //indicamos que le jugador ha sido dañado
            damaged = true;
            //aplicamos el daño recibido
            currentHealth -= amount;
            //actualizamos el estado de la barra de vida
            healthBar.fillAmount = currentHealth / maxHealth;
        }
        

        //si la vida del jugador llega a 0, realiza las acciones de muerte
        if(IsDead()) Death() ;
    }

    /// <summary>
    /// Metodo que se encargara de realizar las acciones para la muerte del jugador
    /// </summary>
    private void Death()
    {
        //indicamos que el juagdor esta muerto
        isDead = false;
        //ejecutamos la animacion de muerte
        anim.SetTrigger("Dead");
        //verificamos si hay alguien suscrito al evento dead, si es asi lo invocamos
        OnPlayerDead?.Invoke();
        //unity event para gestion de acciones y efectos
        onDead?.Invoke();
        //desactivamos el script que gestiona el movimiento del jugador
        //esto mismo lo podriamos hacer directamente en los unity events
        playerController.enabled = false;
        Invoke("EndGame", 2f);
    }
    void EndGame()
    {
        GameManager.instance.EndGame();
    }

    /// <summary>
    /// Metodo que te hace inmune
    /// </summary>
    public void SetImmunity()
    {
        isImmune = !isImmune;
    }
}
