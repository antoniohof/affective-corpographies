using UnityEngine;
using UI = UnityEngine.UI;
using Klak.TestTools;
using BodyPix;
using System.Linq;

public sealed class KeypointsExtractor : MonoBehaviour
{
    [SerializeField] ImageSource _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Vector2Int _resolution = new Vector2Int(512, 384);
    [SerializeField] UI.RawImage _previewUI = null;
    [SerializeField] RectTransform _markerPrefab = null;

    public float minScoreToDetect = 0.3f;

    public BodyDetector _detector;

    public bool detectingBody = false;

    // MASK
    [SerializeField] UI.RawImage backgroundRenderImage = null;
    [SerializeField] UI.RawImage maskRenderImage = null;
    [SerializeField] bool _drawSkeleton = false;
    [SerializeField] Shader _shader = null;
    Material _material;
    public RenderTexture _mask;

    public (RectTransform xform, UI.Text label) [] _markers = new (RectTransform, UI.Text) [Body.KeypointCount];

    void Start()
    {
        // BodyPix detector initialization
        _detector = new BodyDetector(_resources, _resolution.x, _resolution.y);

        _material = new Material(_shader);

        var reso = _source.OutputResolution;
        _mask = new RenderTexture(reso.x, reso.y, 0);
        maskRenderImage.texture = _mask;


        // Marker population
        for (var i = 0; i < Body.KeypointCount; i++)
        {
            var xform = Instantiate(_markerPrefab, _previewUI.transform);
            _markers[i] = (xform, xform.GetComponentInChildren<UI.Text>());
        }
    }

    void OnDestroy()
    {
        _detector.Dispose();
        Destroy(_material);
        Destroy(_mask);
    }

    void LateUpdate()
    {
        // BodyPix detector update
        _detector.ProcessImage(_source.Texture);



        backgroundRenderImage.texture = _source.Texture;
        Graphics.Blit(_detector.MaskTexture, _mask, _material, 0);


        //_previewUI.texture = _source.Texture;
        bool hasKeypoints = false;
        // Marker update
        var rectSize = _previewUI.rectTransform.rect.size;
        for (var i = 0; i < Body.KeypointCount; i++)
        {
            var key = _detector.Keypoints.ElementAt(i);
            var (xform, label) = _markers[i];

            // Visibility
            var visible = key.Score > minScoreToDetect;
            xform.gameObject.SetActive(visible);
            if (!visible) continue;
            hasKeypoints = true;
            // Position and label
            xform.anchoredPosition = key.Position * rectSize;
            label.text = $"{(Body.KeypointID)i}";
        }

        detectingBody = hasKeypoints;



        if (Time.frameCount % 5000 == 0)
        {
            // Debug.Log("collect");
            System.GC.Collect();
            // Application.GarbageCollectUnusedAssets();
        }

    }

    void OnRenderObject()
    {
        if (!_drawSkeleton) return;

        _material.SetBuffer("_Keypoints", _detector.KeypointBuffer);
        _material.SetFloat("_Aspect", (float)_resolution.x / _resolution.y);

        _material.SetPass(1);
        Graphics.DrawProceduralNow
          (MeshTopology.Triangles, 6, Body.KeypointCount);

        _material.SetPass(2);
        Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 12);
    }
}
