using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PARA EL USO DE LOS ACTIONS
using System;
//para el uso de los unity events
using UnityEngine.Events;
using TMPro;

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

    //referencia al rigidbody
    public Rigidbody rb;
    //variable para controlar la fuerza de despedida
    public float launchForce = 500f;

    [Header("JellyDead")]
    public Transform playerTransform;
    public float jumpForce = 10f;


    // Start is called before the first frame update
    void Start()
    {
        Revive();
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
    /// Aplica el da�o recibido como parametro
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="hitPoint"></param>
    public void TakeDamage(float amount, Vector3 hitPoint)
    {
        if (IsDead()) return;

        //aplicamos da�o
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

    public void BobDeath()
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

    /// <summary>
    /// Restaura la vida del enemigo
    /// </summary>
    public void Revive()
    {
        currentHealth = maxHealth;
        //reseteamos el trigger para evitar que se reinicie
        animator.ResetTrigger("Dead");
        //forzamos al aniamciond de Move
        //animator.Play("Bob_Move");

    }

    public void JellyDead()
    {


        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Vector3 jumpDirection = Vector3.up + direction;        
        transform.rotation =  Quaternion.Euler(90f,transform.eulerAngles.y, transform.eulerAngles.z);
        rb.isKinematic = false;
        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
    }

}
