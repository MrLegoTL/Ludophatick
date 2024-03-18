using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SlotMachine : MonoBehaviour
{


    public Projectile projectilePrefab;
    public LaserBeam laserBeamPrefab;
    public Animator animLever;
    

    //referencia al panel de Slot
    public GameObject slotPanel;
    //referencia al texto que mostrara el resultado
    public TMP_Text[] resultText;
    //referencia a la imagen que mostrara el resultado
    public Image[] resultImage;
    //Opciones disponibles en la tragaperra
    public Sprite[] options;
    //public List<SlotOption> options;
    public string[] textOptions;
    //duracion de la animcion de giro
    public float spinDuration = 3f;
    //Indica si la tragaperra esta girando
    private bool isSpining =false;
    public float waitTime = 0f;


    private void Start()
    {
        // Asigna el componente Button y el controlador de clic a cada imagen de resultado
        for (int i = 0; i < resultImage.Length; i++)
        {
            Button button = resultImage[i].GetComponent<Button>();
            int optionIndex = i; // Almacenamos el �ndice de la opci�n para usarlo dentro del listener
            button.onClick.AddListener(() => OnOptionClick(optionIndex));
        }
    }
    private void Update()
    {
       
    }

    
    public void StartSpin()
    {
        
        animLever.SetTrigger("Lever");
        Spin();
        
        //Invoke("Spin",1);
    }

    
    /// <summary>
    /// metodo que se ejecuta al pulsar el boton girar
    /// </summary>
    public void Spin()
    {
       
        if (!isSpining )
        {
            
            StartCoroutine(SpinAnimation());
        }
    }

   
    private IEnumerator SpinAnimation()
    {
        
        isSpining = true;
        int[] randomIndices = new int[resultImage.Length];
        //int randomIndex = 0;
        float elapsedTime = 0f;

        

        //Inicia la animacion de giro
        while (elapsedTime <= spinDuration)
        {
            // Obtiene �ndices aleatorios para seleccionar im�genes aleatorias
            for (int i = 0; i < resultImage.Length; i++)
            {
                randomIndices[i] = UnityEngine.Random.Range(0, options.Length);
            }

            //Actualiza las imagenes durante el giro
            for (int i = 0; i < resultImage.Length; i++)
            {
                //Muestra la opcion actual en el panel
                resultImage[i].sprite = options[randomIndices[i]];
                resultText[i].text = textOptions[randomIndices[i]];
            }


            //Incrementa el tiempo transcurrido
            elapsedTime += Time.unscaledDeltaTime;
            //Espera un frame antes de continuar la animacion
            yield return null;
        }

        // Establece las im�genes y los textos finales una vez que la animaci�n haya terminado
        for (int i = 0; i < resultImage.Length; i++)
        {
            resultImage[i].sprite = options[randomIndices[i]];
            resultText[i].text = textOptions[randomIndices[i]];
        }
        
        isSpining = false;
        
    }

    private void OnOptionClick(int index)
    {
        // Desactiva el panel al hacer clic en la opci�n
        slotPanel.SetActive(false);


        // Obtiene el texto de la opci�n seleccionada
        string selectedOptionText = resultText[index].text;

        // Determina qu� PowerUp corresponde al texto de la opci�n seleccionada
        switch (selectedOptionText)
        {
            case "More Speed":
                // Aplica el PowerUp de aumento de velocidad al jugador
                PlayerController.instance.ApplySpeedBoost(1.1f);
                break;
            case "+10% Damage":
                // Aplica el PowerUp de aumento de da�o al proyectil
                ApplyDamageBoostToProjectiles();
                break;
            case "x2 Laser Damage":
                // Aplica el PowerUp de aumento de da�o al proyectil
                ApplyDamageBoostToLaserBeam();
                break;
            // Agrega m�s casos seg�n sea necesario para otras opciones de PowerUp
            default:
                Debug.LogWarning("Texto de opci�n no reconocido: " + selectedOptionText);
                break;
        }


        // Imprime en la consola el texto de la opci�n seleccionada
        Debug.Log("Opci�n seleccionada: " + resultText[index].text);
    }
    private void ApplyDamageBoostToProjectiles()
    {
        // Si hay un prefab de proyectil y un script asociado, aplica el aumento de da�o
        if (projectilePrefab != null)
        {
            projectilePrefab.ApplyDamageBoost(1.1f); // Ajusta el valor seg�n el aumento de da�o deseado
        }
        else
        {
            Debug.LogWarning("�Prefab de proyectil no asignado!");
        }
    }

    private void ApplyDamageBoostToLaserBeam()
    {
        if(laserBeamPrefab != null)
        {
            laserBeamPrefab.ApplyDamageBoost(2f);
        }
        else
        {
            Debug.LogWarning("�Prefab del Laser no asignado!");
        }
    }
}
