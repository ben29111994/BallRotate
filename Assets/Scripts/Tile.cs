using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstancer;

public class Tile : MonoBehaviour
{
    public int ID;
    public Color tileColor;
    private Renderer meshRenderer;
    //private MaterialPropertyBlock propBlock;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        //propBlock = new MaterialPropertyBlock();

        if (meshRenderer == null)
            meshRenderer = GetComponent<Renderer>();
    }

    public void SetTransfrom(Vector3 pos,Vector3 scale)
    {
        transform.localPosition = pos;
        transform.localScale = new Vector3(scale.x,scale.y,scale.z) * .85f;
    }

    public void SetColor(Color inputColor)
    {
        tileColor = inputColor;
        //meshRenderer.GetPropertyBlock(propBlock);
        //propBlock.SetColor("_Color", inputColor);
        meshRenderer.material.color = tileColor;
        GetComponent<Ball>().ballColor = tileColor;
        //meshRenderer.SetPropertyBlock(propBlock);
    }
}
