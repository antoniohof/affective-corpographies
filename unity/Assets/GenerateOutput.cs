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

    public RenderTexture outputTex;
    // Start is called before the first frame update
    void Start()
    {
        outputTex = new RenderTexture(400, 400, 0);
        outputTex.enableRandomWrite = true;

    }

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

        Graphics.Blit(output.texture, outputTex);
        //Graphics.Blit(rgb.texture, renderTex, _material);
        //output.texture = renderTex;

    }
}
