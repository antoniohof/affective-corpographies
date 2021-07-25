BodyPixBarracuda
================

![gif](https://user-images.githubusercontent.com/343936/126066328-9bb01b01-d16f-4a38-8b7e-fb463bd0aac2.gif)
![gif](https://user-images.githubusercontent.com/343936/126066334-c8d7ea3f-a1b2-49c0-b094-cf55d8f80610.gif)

**BodyPixBarracuda** is an implementation of the [BodyPix] person segmentation and pose estimation model
that runs on the [Unity Barracuda] neural network inference library.

[BodyPix]: https://blog.tensorflow.org/2019/11/updated-bodypix-2.html
[Unity Barracuda]: https://docs.unity3d.com/Packages/com.unity.barracuda@latest

System requirements
-------------------

- Unity 2020.3 LTS or later

About the ONNX file
-------------------

I converted the original BodyPix model (provided as tfjs) into ONNX using tfjs-to-tf and tf2onnx.
See [the Colab notebook] for further details.

[tfjs-to-tf]: https://github.com/patlevin/tfjs-to-tf
[tf2onnx]: https://github.com/onnx/tensorflow-onnx
[the Colab notebook]:
  https://colab.research.google.com/drive/1ikOMoqOX7TSBNId0lGaQ_kIyDF2GV3M3?usp=sharing
