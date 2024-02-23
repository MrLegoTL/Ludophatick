using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para hacer uso de unity events
using UnityEngine.Events;

public class JellyDeath : MonoBehaviour
{
    //daño de impacto
    public float damage = 20;
    // radio de  la zona de impacto
    public float damageAreaRadius = 3;
    //velocidad de desplazamiento
    public float speed = 10;
    //duraciondel misil
    public float lifeTime = 5;
    //contador de tiempo interno
    private float lifeCounter;
    //layers contra los que podra impactar el misil
    public LayerMask shootableLayer;
    //posicion de origen del misil
    public Vector3 startPosition;
    //posicion del objetivo apuntado
    public Vector3 targetPosition;
    //posicion del tirador
    public Vector3 shooterPosition;

    //eventos de unity para los momentos clave del misil
    public UnityEvent onInitialize;
    public UnityEvent onImpact;
    public UnityEvent onDeactivate;

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
    private void OnCollisionEnter(Collision collision)
    {
        //si el layer del objeto es impactado se encuentra dentro del layermask
        if((shootableLayer & (1<<collision.gameObject.layer)) != 0)
        {
            //recuperamos todos los objetos impactados dentro de los layer especificos
            Collider[] impacts = Physics.OverlapSphere(transform.position, damageAreaRadius, shootableLayer);

            //recorremos todos los objetos impactados
            foreach(Collider i in impacts)
            {
                //iniciamos la variable que almacenara el objeto dañable
                damageable = null;

                //tratamos de recuperar la interfaz dañable del objeto actual
                if(i.TryGetComponent<IDamageable<float>>(out damageable))
                {
                    //si la conseguimos recuperar, aplicamos el daño definido
                }
            }

            //fijo su destino como el punto impacto
            //para evitar que el enemigo se siga desplazando
            targetPosition = transform.position;

            //invocamos el unity event para realizar las acciones adicionales de imapcto
            onImpact?.Invoke();
        }
    }

    public void LunchDead()
    {
        startPosition = transform.position;
        //realizamos un Slerp para que realice una trayectoria curva
        // se resta la posicion del tirador, para hcer que no se hagan los calculos de posicion a nivel global
        //sino relativo a quien dispara
        transform.position = Vector3.Slerp(startPosition - shooterPosition,
                                            targetPosition - shooterPosition,
                                            1 - lifeCounter / lifeTime) + shooterPosition;
        lifeCounter -= Time.deltaTime;
    }

    public void Initialized()
    {
        lifeCounter = lifeTime;
        onInitialize?.Invoke();
    }

    public void Deactivate()
    {
        onDeactivate?.Invoke();
    }



}
