using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour
{
    //referencia a la imagen que muestra el cooldown
    public Image shadow;
    //referencia a la corrutina para poder gestionarla
    private Coroutine timer;

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //inicializamos sin cooldown
        shadow.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Corrutina que contara el tiempo e ira reduciendo la sombra de cooldown
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Timer(float time)
    {
        float counter = time;

        while (counter > 0)
        {
            counter -= Time.deltaTime;

            shadow.fillAmount = counter / time;

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Inicia la corrutina deteniendo la anterior si ya existiera
    /// </summary>
    /// <param name="timer"></param>
    public void StartTimer(float t)
    {
        if (timer != null) StopCoroutine(timer);

        timer = StartCoroutine(Timer(t));
    }
}
