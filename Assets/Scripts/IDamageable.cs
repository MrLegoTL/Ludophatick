//para indicar el tipo de vector 3 que sera utilizado
using UnityEngine;

//Esta es una interfaz, que nos permite obligar a incorporar ciertos métodos a las clases que la implementen
//Esta interfaz es una interfaz generica donde T es un placeholder que sera proporcionado por la clase que implemente la interfaz
public interface IDamageable<T>
{
    bool IsDead();

    //La calse que implemente esta interfaz estara obligada a definir este metodo
    //que usara el tipo indicado y una variable Vector3
    //se usa default para obtener el valor predeterminado de un tipo concreto, para hacer en este caso el parametro
    //opcional
    void TakeDamage(T damage, Vector3 impactPosition = default(Vector3));
}
