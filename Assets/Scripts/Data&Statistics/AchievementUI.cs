using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para manipular imagenes
using UnityEngine.UI;
//para manipular los textos
using TMPro;

public class AchievementUI : MonoBehaviour
{
    public Image imageUI;
    public TMP_Text nameUI;
    public Animator animator;
    public float delayAchievement = 3f;
    private float lastAchievementTime;

    private void OnEnable()
    {
        AchievementManager.OnAchievementUnlock += SetAndShow;
    }

    private void OnDisable()
    {
        AchievementManager.OnAchievementUnlock -= SetAndShow;
    }

    /// <summary>
    /// Asigna la informacion del logro al panel y activa la animacion que los muestra
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageName"></param>
    public void SetAndShow(string name, string imageName)
    {
        if (lastAchievementTime > Time.time)
        {
            lastAchievementTime += delayAchievement;
        }
        else
        {
            lastAchievementTime = Time.time + delayAchievement;
        }

        StartCoroutine(DelayShow(name, imageName, lastAchievementTime));
    }

    /// <summary>
    /// Lanza el mostrado del logro en un tiempo programadado
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageName"></param>
    /// <param name="showTime"></param>
    /// <returns></returns>
    private IEnumerator DelayShow(string name, string imageName, float showTime)
    {
        while (Time.time < showTime)
        {
            yield return null;
        }

        nameUI.text = name;
        imageUI.sprite = Resources.Load<Sprite>("AchievementSprites/" + imageName);
        animator.SetTrigger("Show");
    }



}
