using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPersonAndRecord : MonoBehaviour
{

    public KeypointsExtractor keypointExtract;
    public NatSuite.Examples.RecordTexture2 recorder;
    public bool recordingBody = false;

    public bool forceNoRecording = false;

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
