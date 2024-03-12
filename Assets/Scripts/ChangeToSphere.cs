using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToSphere : MonoBehaviour
{
    public SkinnedMeshRenderer rollMeshRenderer;
    public void ChangeRoll()
    {
        rollMeshRenderer.SetBlendShapeWeight(0, 100);
    }
}
