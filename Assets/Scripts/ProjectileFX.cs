using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para hacer uso de los eventos de Unity
using UnityEngine.Events;

public class ProjectileFX : MonoBehaviour
{
    //creamos un evento de unity que recibira un vector 3 como parametro
    public UnityEvent<Vector3> onImpact;
    //evento de unity para cuando el proyectil sea inicializado
    public UnityEvent onInitialize;
    //proyectil al que suscribiremos lo eventos
    public Projectile projectile;

    private void OnEnable()
    {
        //nos sucribimos al action de impacto con el invoke del unity event
        //ambos coinciden con el parametro requerido de  vector 3
        projectile.onImpact += onImpact.Invoke;
        projectile.onInitialize += onInitialize.Invoke;
    }

    private void OnDestroy()
    {
        projectile.onImpact -= onImpact.Invoke;
        projectile.onInitialize -= onInitialize.Invoke;
    }
}
