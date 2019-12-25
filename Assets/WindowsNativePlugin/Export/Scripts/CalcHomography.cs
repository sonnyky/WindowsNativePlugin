using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CalcHomography
{
    [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_create")]
    private static extern IntPtr _Create();

    [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_calc_homography")]
    private static extern void _CalcHomography(IntPtr instance, IntPtr src, IntPtr dst, int length, IntPtr data);

    [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    private IntPtr instance;
    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "cv_util";

    public void InitiateDevice()
    {
        instance = _Create();
        logger.Log(kTAG, "Loaded the plugin : " + Marshal.PtrToStringAnsi(_GetPluginName(instance)));
    }

    private void Start()
    {
        InitiateDevice();
       
    }

    public float[] CalculateHomography(Vector3[] src, Vector3[] dst)
    {
        float[] homography = new float[9];

        // Homography needs at least four points for both point sets
        if (src.Length != dst.Length
            || src.Length < 4) return homography;

        // Pin array of source points
        GCHandle pinnedArray = GCHandle.Alloc(src, GCHandleType.Pinned);
        IntPtr ptrSrc = pinnedArray.AddrOfPinnedObject();

        // Pin array of destination points
        GCHandle pinnedDst = GCHandle.Alloc(dst, GCHandleType.Pinned);
        IntPtr ptrDst = pinnedDst.AddrOfPinnedObject();

        // Pin homography array
        GCHandle pinnedHM = GCHandle.Alloc(homography, GCHandleType.Pinned);
        IntPtr ptrHM = pinnedHM.AddrOfPinnedObject();

        for (int i = 0; i < 9; i++)
        {
            homography[i] = 1.3f;
        }
        _CalcHomography(instance, ptrSrc, ptrDst, src.Length , ptrHM);

        pinnedArray.Free();
        pinnedDst.Free();
        pinnedHM.Free();

        return homography;
    }
}
