using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class JellyDead : EnemyHealth
{
    [Header("JellyDead")]
    public Transform playerTransform;
    public float jumpForce = 10f;
    

    [Header("Lunch")]
    //daño de impacto
    public float damage = 20;
    //radio de la zona de impacto
    public float damageAreaRadius = 3;
    //velocidad de desplazamiento
    public float speed = 10;
    //layers contra los que podra impactar el misil
    public LayerMask shootableLayer;
    public bool isLunch = false;

    public UnityEvent onImpact;
    //variable privada reutilizable, para identificar si el objeto impactado es dañable
    private IDamageable<float> damageable;

    public void JellyDeath()
    {
        Invoke("JellyLunch", 1f);
        Invoke("IsLunch", 2f);
    }
    public void JellyLunch()
    {


        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Vector3 jumpDirection = Vector3.up + direction;
        transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z);
        rb.isKinematic = false;
        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLunch)
        {
            //si el layer delobjeto impactado se encuentra desntro del layermask
            if ((shootableLayer & (1 << other.gameObject.layer)) != 0)
            {
                Debug.Log("Ha impactado");
                //recuperamos todos los objetos impactados dentro de los layer especificados
                Collider[] impacts = Physics.OverlapSphere(transform.position, damageAreaRadius, shootableLayer);

                //recorremos todos los objetos impactados
                foreach (Collider i in impacts)
                {
                    //inicializamos la variale que almacenará el objeto dañable
                    damageable = null;

                    //tratamos de recuperar la interfaz dañable del objeto actual
                    if (i.TryGetComponent<IDamageable<float>>(out damageable))
                    {
                        //si la conseguimos recuperar, aplicamos el daño definido
                        damageable.TakeDamage(damage, transform.position);
                    }
                }

                //invocamos el unity event para realizar las acciones adicionales de impacto
                onImpact?.Invoke();

            }
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
        
    //    if (isLunch)
    //    {
    //        //si el layer delobjeto impactado se encuentra desntro del layermask
    //        if ((shootableLayer & (1 << collision.gameObject.layer)) != 0)
    //        {
    //            Debug.Log("Ha impactado");
    //            //recuperamos todos los objetos impactados dentro de los layer especificados
    //            Collider[] impacts = Physics.OverlapSphere(transform.position, damageAreaRadius, shootableLayer);

    //            //recorremos todos los objetos impactados
    //            foreach (Collider i in impacts)
    //            {
    //                //inicializamos la variale que almacenará el objeto dañable
    //                damageable = null;

    //                //tratamos de recuperar la interfaz dañable del objeto actual
    //                if (i.TryGetComponent<IDamageable<float>>(out damageable))
    //                {
    //                    //si la conseguimos recuperar, aplicamos el daño definido
    //                    damageable.TakeDamage(damage, transform.position);
    //                }
    //            }

    //            //invocamos el unity event para realizar las acciones adicionales de impacto
    //            onImpact?.Invoke();

    //        }
    //    }
       
    //}

    public void IsLunch()
    {
        isLunch = true;
    }
}
