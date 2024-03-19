using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement 
{
    //nombre del logro
    public string name;
    //identificador de estadistica a verificar
    public string statCode;
    //nombre del sprite para el logo
    public string imageName;
    //descripcion del logro
    public string description;
    //canitdad a conseguir de la estadistica para desbloquear el logro
    public int targetAmount;
    //bool para controlar si el logro ya ha sido desbloqueado
    public bool unlocked = false;
}
