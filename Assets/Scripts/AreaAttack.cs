using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public float damage = 20;
    public float damageAreaRadius = 3;
    public float knockBackForce = 10f;
    public Rigidbody rb;
    //layers contra los que podra impactar el misil
    public LayerMask shootableLayer;

    //variable privada reutilizable, para identificar si el objeto impactado es dañable
    private IDamageable<float> damageable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //si el layer delobjeto impactado se encuentra desntro del layermask
        if ((shootableLayer & (1 << other.gameObject.layer)) != 0)
        {
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
                 
                if (rb != null)
                {
                    Vector3 direction = (other.transform.position - transform.position).normalized;
                    rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
                }
            }
        }
    }
}
