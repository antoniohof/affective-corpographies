using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPersonAndRecord : MonoBehaviour
{

    public KeypointsExtractor keypointExtract;
    public NatSuite.Examples.RecordTexture2 recorder;
    public bool recordingBody = false;

    public bool forceNoRecording = false;

    public CenterBody centerBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (forceNoRecording)
        {
            if (recorder.recording)
            {
                recorder.StopRecording();
                recordingBody = false;
            }
            return;
        }
        if (keypointExtract.detectingBody && !recorder.recording)
        {
            // TODO choose randomly body part 
            // centerBody.BodypartToFocus = 

            List<string> parts = new List<string>();
            parts.Add("Nose");
            parts.Add("LeftEye");
            parts.Add("RightEye");
            int randomInt = Random.Range(0, 3);
            Debug.Log(randomInt);
            centerBody.BodypartToFocus = parts[randomInt];


            recorder.StartRecording();
            recordingBody = true;
        }

        if (!keypointExtract.detectingBody && recorder.recording)
        {
            recorder.StopRecording();
            recordingBody = false;
        }
    }
}
