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

    private string currentLabelToFind = "";

    // Start is called before the first frame update
    void Start()
    {
        currentLabelToFind = BodypartToFocus;
    }

    // Update is called once per frame
    void Update()
    {

        currentLabelToFind = BodypartToFocus;

        // Marker update
        for (var i = 0; i < extractor._markers.Length; i++)
        {
            var (xform, label) = extractor._markers[i];

            if (label.text == currentLabelToFind)
            {
                float x = (xform.anchoredPosition.x / 1920f) * 2.0f;
                float y = (xform.anchoredPosition.y / 1920f) * 2.0f;
                // -0.8 centralized o body when using Nose
                // -0.57 centralize on body part
                float xAdjuster = 1.0f;

                float yAdjuster = 0.57f;

                if (currentLabelToFind == "Nose")
                {
                    // actually focus on mouth
                    yAdjuster = 0.61f;
                    cameraToAdjust.orthographicSize = 0.04f;

                }
                if (currentLabelToFind == "LeftEye" || currentLabelToFind == "RightEye")
                {
                    cameraToAdjust.orthographicSize = 0.02f;
                }
                if (currentLabelToFind == "LeftAnkle") {
                    cameraToAdjust.orthographicSize = 0.06f;

                }
                if (currentLabelToFind == "RightShoulder")
                {
                    cameraToAdjust.orthographicSize = 0.2f;
                    yAdjuster = 0.75f;
                    xAdjuster = 0.95f;

                }
                if (currentLabelToFind == "LeftKnee")
                {
                    cameraToAdjust.orthographicSize = 0.06f;
                }
                // Define a target position above and behind the target transform
                Vector3 targetPosition = new Vector3(x - xAdjuster, y - yAdjuster, cameraToAdjust.transform.position.z);


                cameraToAdjust.transform.position = Vector3.SmoothDamp(cameraToAdjust.transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}
