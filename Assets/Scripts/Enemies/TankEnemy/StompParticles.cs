using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para el uso de los unity events
using UnityEngine.Events;

public class StompParticles : MonoBehaviour
{
    public ParticleSystem particle;
    //unity event para lanzar efectos al morir
    public UnityEvent OnStompEvent;

    public void ActivateParticles()
    {
        OnStompEvent?.Invoke();
    }
  
}
