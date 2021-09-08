using UnityEngine;
using UnityEngine.UI;
using BodyPix;
using UI = UnityEngine.UI;
using Klak.TestTools;
using System.Linq;

public class VisualizerOld : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Vector2Int _resolution = new Vector2Int(512, 384);
    [SerializeField] RawImage backgroundRenderImage = null;
    [SerializeField] RawImage maskRenderImage = null;
    [SerializeField] bool _drawSkeleton = false;
    [SerializeField] Shader _shader = null;
    [SerializeField] RectTransform _markerPrefab = null;
    const float ScoreThreshold = 0.3f;


    (RectTransform xform, UI.Text label) []
      _markers = new (RectTransform, UI.Text) [Body.KeypointCount];

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


         // BodyPix detector initialization
        //_detector = new BodyDetector(_resources, _resolution.x, _resolution.y);

        // Marker population
        for (var i = 0; i < Body.KeypointCount; i++)
        {
            var xform = Instantiate(_markerPrefab, backgroundRenderImage.transform);
            _markers[i] = (xform, xform.GetComponentInChildren<UI.Text>());
        }
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

        // Marker update
        /*
        var rectSize = backgroundRenderImage.rectTransform.rect.size;
        for (var i = 0; i < Body.KeypointCount; i++)
        {
            var key = _bodypix.Keypoints.ElementAt(i);
            var (xform, label) = _markers[i];

            // Visibility
            var visible = key.Score > ScoreThreshold;
            xform.gameObject.SetActive(visible);
            if (!visible) continue;

            // Position and label
            xform.anchoredPosition = key.Position * rectSize;
            label.text = $"{(Body.KeypointID)i}\n{key.Score:0.00}";
        }
        */

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
