using UnityEngine;
using UI = UnityEngine.UI;
using BodyPix;
using Klak.TestTools;
using System.Collections.Generic;
using Unity.Barracuda;

public class MaskExtractor : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Vector2Int _resolution = new Vector2Int(512, 384);
    [SerializeField] UI.RawImage backgroundRenderImage = null;
    [SerializeField] UI.RawImage maskRenderImage = null;
    [SerializeField] bool _drawSkeleton = false;
    [SerializeField] Shader _shader = null;


    BodyPixRuntime _bodypix;
    Material _material;
    public RenderTexture _mask;
    void Start()
    {
        _bodypix = new BodyPixRuntime(_resources, _resolution.x, _resolution.y);

        _material = new Material(_shader);
        
        var reso = _source.OutputResolution;
        _mask = new RenderTexture(reso.x, reso.y, 0);
        maskRenderImage.texture = _mask;

    }

    void OnDestroy()
    {
        _bodypix.Dispose();
        Destroy(_material);
        Destroy(_mask);
    }

    void LateUpdate()
    {
        _bodypix.ProcessImage(_source.Texture);

        backgroundRenderImage.texture = _source.Texture;
        Graphics.Blit(_bodypix.Mask, _mask, _material, 0);


        if (Time.frameCount % 30 == 0)
        {
            // Debug.Log("collect");
            System.GC.Collect();
        // Application.GarbageCollectUnusedAssets();
        }
        
    }

    void OnRenderObject()
    {
        if (!_drawSkeleton) return;
        
        _material.SetBuffer("_Keypoints", _bodypix.Keypoints);
        _material.SetFloat("_Aspect", (float)_resolution.x / _resolution.y);

        _material.SetPass(1);
        Graphics.DrawProceduralNow
          (MeshTopology.Triangles, 6, BodyPixRuntime.KeypointCount);

        _material.SetPass(2);
        Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 12);
    }
}
