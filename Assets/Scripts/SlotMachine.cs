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

    [Header("PowerUp")]
    private float damageBoost;

    public static Action onClickPowerUp;
    public static SlotMachine instance;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    private void Start()
    {
        damageBoost = 1;
        // Asigna el componente Button y el controlador de clic a cada imagen de resultado
        for (int i = 0; i < resultImage.Length; i++)
        {
            Button button = resultImage[i].GetComponent<Button>();
            int optionIndex = i; // Almacenamos el índice de la opción para usarlo dentro del listener
            button.onClick.AddListener(() => OnOptionClick(optionIndex));
        }
    }
    private void Update()
    {
        
    }

    
    public void StartSpin()
    {
        
        
            //animLever.SetTrigger("Lever");
            Spin();
            // Descontar el dinero del jugador
            
        
        
        
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

        yield return new WaitForSecondsRealtime(1);

        //Inicia la animacion de giro
        while (elapsedTime <= spinDuration)
        {
            // Obtiene índices aleatorios para seleccionar imágenes aleatorias
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

        // Establece las imágenes y los textos finales una vez que la animación haya terminado
        for (int i = 0; i < resultImage.Length; i++)
        {
            resultImage[i].sprite = options[randomIndices[i]];
            resultText[i].text = textOptions[randomIndices[i]];
        }
        
        isSpining = false;
        
    }
    

    private void OnOptionClick(int index)
    {
        // Desactiva el panel al hacer clic en la opción
        slotPanel.SetActive(false);


        // Obtiene el texto de la opción seleccionada
        string selectedOptionText = resultText[index].text;

        // Determina qué PowerUp corresponde al texto de la opción seleccionada
        switch (selectedOptionText)
        {
            case "More Speed":
                // Aplica el PowerUp de aumento de velocidad al jugador
                PlayerController.instance.ApplySpeedBoost(1.1f);
                break;
            case "+10% Damage":
                // Aplica el PowerUp de aumento de daño al proyectil
                PlayerController.instance.ApplyDamageBoost(1.2f);
                break;
            case "x2 Laser Damage":
                // Aplica el PowerUp de aumento de daño al proyectil
                ApplyDamageBoostToLaserBeam();
                break;
            // Agrega más casos según sea necesario para otras opciones de PowerUp
            default:
                Debug.LogWarning("Texto de opción no reconocido: " + selectedOptionText);
                break;
        }
        onClickPowerUp?.Invoke();

        // Imprime en la consola el texto de la opción seleccionada
        Debug.Log("Opción seleccionada: " + resultText[index].text);
    }
 

    private void ApplyDamageBoostToLaserBeam()
    {
        if(laserBeamPrefab != null)
        {
            laserBeamPrefab.ApplyDamageBoost(2f);
        }
        else
        {
            Debug.LogWarning("¡Prefab del Laser no asignado!");
        }
    }
}
