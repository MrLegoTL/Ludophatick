using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;

public class SlotMachine : MonoBehaviour
{
   

   

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
    private bool isSpining;


    private void Start()
    {
        // Asigna el componente Button y el controlador de clic a cada imagen de resultado
        for (int i = 0; i < resultImage.Length; i++)
        {
            Button button = resultImage[i].GetComponent<Button>();
            int optionIndex = i; // Almacenamos el índice de la opción para usarlo dentro del listener
            button.onClick.AddListener(() => OnOptionClick(optionIndex));
        }
    }

    /// <summary>
    /// metodo que se ejecuta al pulsar el boton girar
    /// </summary>
    public void Spin()
    {
        if (!isSpining)
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

        // Imprime en la consola el contenido del arreglo resultText
        string result = "Contenido de resultText:\n";
        for (int i = 0; i < resultText.Length; i++)
        {
            result += "Elemento " + i + ": " + resultText[i].text + "\n";
        }
        Debug.Log(result);
    }


}
