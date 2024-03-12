using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToSphere : MonoBehaviour
{
    //public SkinnedMeshRenderer rollMeshRenderer;
    public GameObject Player;
    public GameObject rollPlayer;
    [ContextMenu("Test")]
    public void ChangeRoll()
    {
        Player.SetActive(false);
        rollPlayer.SetActive(true);
        //rollMeshRenderer.SetBlendShapeWeight(0, 100);
    }

    public void EndRoll()
    {
        Player.SetActive(true);
        rollPlayer.SetActive(false);
    }
}
