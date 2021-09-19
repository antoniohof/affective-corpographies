using OscJack;
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

    public void chooseRandomPart()
    {
        List<string> parts = new List<string>();
        parts.Add("Nose");
        parts.Add("LeftEye");
        parts.Add("RightEye");
        parts.Add("LeftAnkle");
        parts.Add("RightShoulder");
        parts.Add("LeftKnee");
        int randomInt = Random.Range(0, parts.Count);
        Debug.Log(randomInt);

           if (parts[randomInt] == centerBody.BodypartToFocus)
        {
            return;
        }
        centerBody.BodypartToFocus = parts[randomInt];


        // IP address, port number
        var client = new OscClient("127.0.0.1", 9000);
        client.Send("/td", "stop");
        Debug.Log("send stop");

        StartCoroutine(waitHalfSecondandSync());

    }

    IEnumerator waitHalfSecondandSync ()
    {
        var client = new OscClient("127.0.0.1", 9000);

        yield return new WaitForSeconds(0.5f);
        client.Send("/td", centerBody.BodypartToFocus);
        Debug.Log("send start");
        yield return null;
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


            chooseRandomPart();

            // IP address, port number
            var client = new OscClient("127.0.0.1", 9000);

            client.Send("/td", centerBody.BodypartToFocus);
            Debug.Log("send start");

            // Terminate the client.
            client.Dispose();
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
