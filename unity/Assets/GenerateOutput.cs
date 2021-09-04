using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateOutput : MonoBehaviour
{

    [SerializeField] public Shader _shader = null;
    [SerializeField] public RawImage output = null;


    [SerializeField] public RawImage rgb = null;
    [SerializeField] public RawImage alpha = null;


    // Update is called once per frame
    void LateUpdate()
    {
        BlendTextures();
    }

    public void BlendTextures()
    {
        output.texture = rgb.texture;

        output.material.SetTexture("_Alpha", alpha.texture);
        output.material.SetTexture("_MainTex", rgb.texture);

        //Graphics.Blit(rgb.texture, renderTex, _material);
        //output.texture = renderTex;

    }
}
