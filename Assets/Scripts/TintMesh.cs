using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintMesh : MonoBehaviour
{
    static MaterialPropertyBlock mpb;
    [SerializeField] private Color color;
    private void Awake()
    {
        if (mpb == null)
            mpb = new MaterialPropertyBlock();

        Renderer r = GetComponent<Renderer>();

        r.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", color);
        r.SetPropertyBlock(mpb);
    }
}
