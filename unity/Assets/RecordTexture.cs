using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NatSuite.Recorders;
using UnityEngine.Rendering;
using NatSuite.Recorders.Clocks;

public class RecordTexture : MonoBehaviour
{   
    public RenderTexture inputRenderTexToRecord;
    private IClock clock;

    MP4Recorder recorder;

    long counter = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("started recording");
        recorder = new MP4Recorder(400,400,15);
        clock = new RealtimeClock();

        while (true)
        {
            yield return new WaitForSeconds(1);
            yield return new WaitForEndOfFrame();

            AsyncGPUReadback.Request(inputRenderTexToRecord, 0, TextureFormat.ARGB32, OnCompleteReadback);
            counter++;
            Debug.Log(counter);
            if (counter > 20)
            {
                StopRecording();
            }
        }

    }
    public async void StopRecording()
    {
        // Stop recording
        var path = await recorder.FinishWriting();
        // Playback recording
        Debug.Log($"Saved recording to: {path}");
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            return;
        }

       // var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        //tex.LoadRawTextureData(request.GetData<uint>());
       // tex.Apply();

        var nativeArray = request.GetData<byte>();
        // And commit the pixel buffer
        recorder.CommitFrame(nativeArray.ToArray(), clock.timestamp);

    }

}
