using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PARA EL USO DE LOS ACTIONS
using System;
//para el uso de los unity events
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour , IDamageable<float>
{
    //vida maxima
    public float maxHealth = 20;
    //vida actual
    public float currentHealth;
    //referencia del animator
    public Animator animator;
    //action static para patron observador de la muerte de cualquier enemigo
    public static Action OnDead;
    //unity event para lanzar efectos al morir
    public UnityEvent OnDeadEvent;
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
    /// Indica si el enemigo ha muerto
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    /// <summary>
    /// Aplica el daño recibido como parametro
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="hitPoint"></param>
    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        if (IsDead()) return;

        //aplicamos daño
        currentHealth -= amount;

        if(IsDead() )
        {
            Death();
        }
    }

    /// <summary>
    /// Realiza las acciones necesarias para gestionar la muerte del enemigo
    /// </summary>
    private void Death()
    {
        //invocamos el evento de unity
        OnDeadEvent?.Invoke();
        //ejecutamos la animacion de muerte
        animator.SetTrigger("Dead");
        Debug.Log("Enemigo muerto");
        //llamada al action para los observadores suscritos
        OnDead?.Invoke();
    }
}
