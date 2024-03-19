using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class AchievementDisplay : MonoBehaviour
{
    public string achievementName;
    public Image image;
    public TMP_Text name;
    public TMP_Text description;
    public GameObject shadow;
    // Start is called before the first frame update
    void Start()
    {
        Achievement result = DataManager.instance.data.achievements.Where(a => a.name == achievementName).FirstOrDefault();

        image.sprite = Resources.Load<Sprite>("AchievementSprites/" + result.imageName);
        name.text = result.name;
        description.text = result.description;
        //si esta desbloqueado dessctivamos la sombra
        shadow.SetActive(!result.unlocked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
