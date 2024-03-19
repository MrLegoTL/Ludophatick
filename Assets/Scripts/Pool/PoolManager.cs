using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para hacer busquedas en arrays
using System.Linq;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    //array con todas las pools disponible
    public Pool[] pools;

    private void Awake()
    {
        if (!instance) instance = this;

    }
   

    private void OnEnable()
    {
        // nos suscribimos al Action static de la clase PoolEntity mediante el metodo Push
        // que sera invocado cada vex que un PoolEntity quiera volver a la pool
        PoolEntity.OnReturnToPool += Push;
    }

    private void OnDisable()
    {
        //nos de-suscribimos delAction en el caso en el que dejemos de estar activos
        // para que el action no nos invoque sin estar presentes (lo que daria lugar a errores)
        PoolEntity.OnReturnToPool -= Push;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializePools();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Vuelve a meter un entity en su pool correspondiente
    /// Este metodo sera el que utilizaremos para sucribirnos al Action de los PoolEntity
    /// </summary>
    /// <param name="entity"></param>
    public void Push(PoolEntity entity)
    {
        //Busco la pool cuyo ID coincide con el Id del entity recibido como parametro
        foreach (Pool pool in pools.Where(a => a.id == entity.poolID).AsEnumerable())
        {
            // si encuentra la pool, agrega el entity de vuelta a su pool
            pool.pool.Enqueue(entity);
        }
    }

    /// <summary>
    /// Este metodo crea un nuevo PoolEntity del pool indicado
    /// </summary>
    /// <param name="poolID"></param>
    /// <returns></returns>
    private PoolEntity CreatePoolEntity(string poolID)
    {
        //variable para almacenar el nuevo entity generado
        PoolEntity entity = null;
        //buscamos la pool con el ID indicado como parametro
        Pool pool = pools.Where(a => a.id == poolID).FirstOrDefault();
        //si encontramos la pool
        if (pool != null)
        {
            //instaciamos el entity con el prefab de la pool
            entity = Instantiate(pool.prefab, transform);
            //asignamos al nuevo entity el ID de la pool
            entity.poolID = pool.id;
        }
        //devolvemos el nuevo entity generado
        return entity;
    }

    /// <summary>
    /// Inicializa todas las pools
    /// </summary>
    private void InitializePools()
    {
        //recorremos todas las pools
        foreach (Pool pool in pools)
        {
            //repetimos esta operacion tantas veces como prewarm se haya especificado
            for (int i = 0; i < pool.prewarm; i++)
            {
                //instanciamos una nueva entidad
                PoolEntity temp = CreatePoolEntity(pool.id);
                //la dejamos desactivada
                temp.Deactivate();
                //la ponemos en cola
                pool.pool.Enqueue(temp);
            }
        }
    }
    /// <summary>
    /// Extrae un entity de la pool indicada y lo posiciona, rota y activa con lo sparametros indicados
    /// </summary>
    /// <param name="poolID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public PoolEntity Pull(string poolID, Vector3 position, Quaternion rotation)
    {
        //variable para contener el entity resultante
        PoolEntity entity = null;
        //busacmos el pool que tenga el ID indicado
        Pool pool = pools.Where(a => a.id == poolID).FirstOrDefault();
        //si existe la pool
        if (pool != null)
        {
            //si no ha sido posible realizar un Dequeue( si no quedan elementos en la pool)
            //en el caso de que el resultado sea true, entity pasara a contener un elemneto
            //extraido de la Queue
            if (!pool.pool.TryDequeue(out entity))
            {
                //creamos un nuevo entity en su lugar y lo entregamos
                entity = CreatePoolEntity(poolID);
            }
        }

        //si el entity es null
        if (entity != null)
        {
            entity.transform.position = position;
            entity.transform.rotation = rotation;
            entity.Initialize();
        }

        return entity;


    }

    public void UpdatePulledObject(string poolID, Vector3 position, Quaternion rotation)
    {
        // Buscamos el pool que tenga el ID indicado
        Pool pool = pools.Where(a => a.id == poolID).FirstOrDefault();
        // Si existe la pool
        if (pool != null)
        {
            // Obtenemos el último entity extraído de la pool
            PoolEntity entity = pool.pool.LastOrDefault();
            // Si encontramos un entity
            if (entity != null)
            {
                // Actualizamos su posición y rotación
                entity.transform.position = position;
                entity.transform.rotation = rotation;
            }
        }
    }
}
