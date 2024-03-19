using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PARA PODER SERIALIZAR Y DESERIALIZAR EN BINARIO
using System.Runtime.Serialization.Formatters.Binary;
//PARA  PODER HACER LECTURA Y ESCRITURA DE ARCHIVOS
using System.IO;

public class DataManager : MonoBehaviour
{
    //variable que contendra toda la información de estadisticas y logros
    public Data data;
    //nombre en el que almacenaremos la infromacion 
    public string fileName = "data.dat";
    //ruta + el nombre de archivo
    private string dataPath;

    public static DataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //indicamos que el gameobject no sera destruido entre escenas
            DontDestroyOnLoad(gameObject);

        }
        //si se trata de una instancia distinta a la cual
        else if (instance != this)
        {
            //la destruimos
            Destroy(gameObject);
        }
        //conformamos el dataPath combinando el persitente datapath con el nombre de archivo
        dataPath = Application.persistentDataPath + "/" + fileName;
        //para poder localizar más facilmente la carpeta de data
        Debug.Log(Application.persistentDataPath);

        //cargamos el data previo si este existe
        Load();
    }

    /// <summary>
    /// Guardar la informacion de data, de forma persistente
    /// </summary>
    [ContextMenu("Save")]
    public void Save()
    {
        dataPath = Application.persistentDataPath + "/" + fileName;
        //objeto utilizado para serializar / deserializar
        BinaryFormatter bf = new BinaryFormatter();
        // crea /  sobreescribe el fichero con los datos en binario
        FileStream file = File.Create(dataPath);
        //serializamos el contenido de nuetsro objetod de datos volcado al archivo
        bf.Serialize(file, data);
        // cerramos el stream una vez termiando el proceso
        file.Close();
    }

    /// <summary>
    /// Recupera la informacion almacenada en el disco
    /// </summary>
    [ContextMenu("Load")]
    public void Load()
    {
        dataPath = Application.persistentDataPath + "/" + fileName;
        //si no existe el archivo no hacemos nada
        if (!File.Exists(dataPath)) return;

        //objeto para serializar / deserializar
        BinaryFormatter bf = new BinaryFormatter();
        //apertura del fichero para su lectura
        FileStream file = File.Open(dataPath, FileMode.Open);
        //Deserializamos el fichero utilizando la estructura de la clase data
        data = (Data)bf.Deserialize(file);
        //una vez terminado cerramos el archivo
        file.Close();

    }
    /// <summary>
    /// Borra el fichero de guardado
    /// </summary>
    [ContextMenu("DelateData")]
    public void DeleteSaveFile()
    {
        dataPath = Application.persistentDataPath + "/" + fileName;
        try
        {
            //borra fisicamente el archivo
            File.Delete(dataPath);
        }
        catch (System.Exception)
        {
            Debug.Log("No existe el archivo");
        }

    }

}
