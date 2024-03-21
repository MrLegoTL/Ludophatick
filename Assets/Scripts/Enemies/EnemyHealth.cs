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
    public UnityEvent OnAfterDead;
    //referencia al rigidbody
    public Rigidbody rb;
    public int moneyDropped = 10;
    public GameObject moneyPrefab;
    public Transform moneyPoint;
    public string moveAnimation;
    public static EnemyHealth instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        BossManager.onFightBoss += Death;
    }

    private void OnDisable()
    {
        BossManager.onFightBoss -= Death;
    }
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
        Instantiate(moneyPrefab, moneyPoint.transform.position, Quaternion.identity);
        //ejecutamos la animacion de muerte
        animator.SetTrigger("Dead");
        Debug.Log("Enemigo muerto");      

        //llamada al action para los observadores suscritos
        OnDead?.Invoke();
        Invoke("AfterDead", 10f);
        
    }   

    public void AfterDead()
    {
        OnAfterDead?.Invoke();
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
        animator.Play(moveAnimation);

    }



}
