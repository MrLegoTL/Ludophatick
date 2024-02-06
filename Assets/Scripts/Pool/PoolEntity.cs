using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para hacer uso de Action
using System;


[System.Serializable]
public class PoolEntity : MonoBehaviour
{
    //almacena la pool a la que pertenece esta entidad
    public string poolID;
    //action al que se suscribira la pool, para capturar las entidades que vuelvan a la pool
    public static Action<PoolEntity> OnReturnToPool;
    //para desactivar los elementos visuales del objeto
    public Renderer[] renderers;
    //para saber si el objeto esta activo
    public bool active;


    /// <summary>
    /// Acciones a realizar al extraer el objeto de la pool
    /// Es de tipo virtual para que pueda ser reescrito mediante override en la clase que lo herede
    /// </summary>
    public virtual void Initialize()
    {
        active = true;
        //activa todos los renderers de la entidad
        EnableRenderers(true);
    }

    public virtual void Deactivate()
    {
        active = false;
        EnableRenderers(false);
    }

    public void ReturnPool()
    {
        Deactivate();
        //todos aquellos que esten suscritos al Action, recibiran la referencia de este objeto
        //que sea volver a la pool
        //if(OnReturnToPool !=null) OnReturnToPool.Invoke(this)
        OnReturnToPool?.Invoke(this);
    }

    /// <summary>
    /// Activa o desactivba los elementos visuales del objeto
    /// </summary>
    /// <param name="enable"></param>
    private void EnableRenderers(bool enable)
    {
        //recorremos la lista de renderers
        foreach (Renderer rend in renderers)
        {
            //activo o desactivo el renderer segun el parametro
            rend.enabled = enable;
        }
    }

    /// <summary>
    /// Localiza y alamcena los renderer del entity en un array
    /// </summary>
    [ContextMenu("Finde Renderers")]
    public void FindeRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

}
