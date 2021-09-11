using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;
using Klak.TestTools;
using BodyPix;
using System.Linq;

public class CenterBody : MonoBehaviour
{

    public KeypointsExtractor extractor;
    public Camera cameraToAdjust;

    public string BodypartToFocus = "Nose";
    public float smoothTime = 0.1F;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Marker update
        for (var i = 0; i < extractor._markers.Length; i++)
        {
            var (xform, label) = extractor._markers[i];

            if (label.text == BodypartToFocus)
            {
                float x = (xform.anchoredPosition.x / 1920f) * 2.0f;
                float y = (xform.anchoredPosition.y / 1920f) * 2.0f;
                // -0.8 centralized o body when using Nose
                // -0.57 centralize on body part

                // Define a target position above and behind the target transform
                Vector3 targetPosition = new Vector3(x - 1.0f, y - 0.57f, cameraToAdjust.transform.position.z);


                cameraToAdjust.transform.position = Vector3.SmoothDamp(cameraToAdjust.transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}
