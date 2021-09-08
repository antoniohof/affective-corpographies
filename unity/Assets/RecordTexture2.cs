/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Recorders;
    using Recorders.Clocks;

    public class RecordTexture2 : MonoBehaviour {

       //  public RenderTexture inputTexture;
        private MP4Recorder recorder;
        private IClock clock;
        private bool recording;
        private Color32[] pixelBuffer;
        //public RawImage previewImage;


        public Camera cam;

        private Texture2D readbackTexture;
        #region --Recording State--

        public int width = 1920;
        public int height = 1080;

        void Start () {
        readbackTexture = new Texture2D(width, height);

        }

        public void StartRecording () {
            // Start recording
            clock = new RealtimeClock();
            recorder = new MP4Recorder(width, height, 30);
            recording = true;
        }

        public async void StopRecording () {
            // Stop recording
            recording = false;
            var path = await recorder.FinishWriting();
            // Playback recording
            Debug.Log($"Saved recording to: {path}");
            Handheld.PlayFullScreenMovie($"file://{path}");
        }
        #endregion


        #region --Operations--



        void Update () {
                // Record frames from the webcam
                // Say we have some `RenderTexture`
                /*
                var width = inputTexture.width;
                var height = inputTexture.height;
                // We can perform a synchronous readback using a `Texture2D`
                Texture2D readbackTexture = new Texture2D(width, height);
                RenderTexture.active = inputTexture;
                readbackTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                previewImage.texture = inputTexture;

                RenderTexture.active = null;
                */


            //Get temporary RenderTexture
            RenderTexture tempRT = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = tempRT;
            cam.Render();

            RenderTexture.active = tempRT;
            readbackTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            // previewImage.texture = tempRT;

            //Release temporary RenderTexture
            RenderTexture.ReleaseTemporary(tempRT);

            if (recording) {
                recorder.CommitFrame(readbackTexture.GetPixels32(), clock.timestamp);
            }
            cam.targetTexture = null;
        }
        #endregion
    }
}