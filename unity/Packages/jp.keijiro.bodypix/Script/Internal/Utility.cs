using Unity.Barracuda;
using UnityEngine;

namespace BodyPix {

#region Object construction/destruction helpers

static class ObjectUtil
{
    public static void Destroy(Object o)
    {
        if (o == null) return;
        if (Application.isPlaying)
            Object.Destroy(o);
        else
            Object.DestroyImmediate(o);
    }
}

static class RTUtil
{
    public static RenderTexture NewFloat(int w, int h)
      => new RenderTexture(w, h, 0, RenderTextureFormat.RFloat);

    public static RenderTexture NewArgbUav(int w, int h)
    {
        var rt = new RenderTexture
          (w, h, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }
}

static class ColorUtil
{
    public static bool IsLinear
      => QualitySettings.activeColorSpace == ColorSpace.Linear;
}

#endregion

#region Extension methods

static class ComputeShaderExtensions
{
    public static void DispatchThreads
      (this ComputeShader compute, int kernel, int x, int y, int z)
    {
        uint xc, yc, zc;
        compute.GetKernelThreadGroupSizes(kernel, out xc, out yc, out zc);

        x = (x + (int)xc - 1) / (int)xc;
        y = (y + (int)yc - 1) / (int)yc;
        z = (z + (int)zc - 1) / (int)zc;

        compute.Dispatch(kernel, x, y, z);
    }
}

static class IWorkerExtensions
{
    public static void CopyOutput
      (this IWorker worker, string tensorName, RenderTexture rt)
    {
        var output = worker.PeekOutput(tensorName);
        var shape = new TensorShape(1, rt.height, rt.width, 1);
        using var tensor = output.Reshape(shape);
        tensor.ToRenderTexture(rt);
    }
}

#endregion

} // namespace BodyPix
