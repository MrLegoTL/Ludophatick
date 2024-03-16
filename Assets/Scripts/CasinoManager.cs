using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CasinoManager : MonoBehaviour
{
    [Header("Casino Interaction")]    
    //Referencia la Panel del Casino
    public GameObject casinoPanel;
    //Referencia al panel de slots
    public GameObject slotPanel;
    public bool canInteract = false;
    //Indica si el juego esta pausado
    private bool isGamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            InteractWithPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract=false;
        }
    }

    private void InteractWithPlayer()
    {
        if(casinoPanel != null)
        {
            casinoPanel.SetActive(true);
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        canInteract = false;
    }

    /// <summary>
    /// Método para reanudar el juego
    /// </summary>
    private void ResumeGame()
    {
        Time.timeScale = 1f; // Reanuda el tiempo en el juego
        canInteract = true;
    }

    public void ExitCasino()
    {
        casinoPanel.SetActive(false);
        slotPanel.SetActive(false);
        ResumeGame();
    }

    public void SpendMoney()
    {
        casinoPanel.SetActive(false);
        slotPanel.SetActive(true);
    }
}
