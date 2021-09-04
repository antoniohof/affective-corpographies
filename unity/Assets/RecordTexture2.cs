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

        public RenderTexture inputTexture;
        private MP4Recorder recorder;
        private IClock clock;
        private bool recording;
        private Color32[] pixelBuffer;
        //public RawImage previewImage;


        #region --Recording State--

        void Start () {
            //previewImage.texture = inputTexture;
        }

        public void StartRecording () {
            // Start recording
            clock = new RealtimeClock();
            recorder = new MP4Recorder(inputTexture.width, inputTexture.height, 30);
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
            if (recording) {
                // Say we have some `RenderTexture`
                var width = inputTexture.width;
                var height = inputTexture.height;
                // We can perform a synchronous readback using a `Texture2D`
                var readbackTexture = new Texture2D(width, height);
                RenderTexture.active = inputTexture;
                readbackTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);

                RenderTexture.active = null;
                recorder.CommitFrame(readbackTexture.GetPixels32(), clock.timestamp);
            }
        }
        #endregion
    }
}