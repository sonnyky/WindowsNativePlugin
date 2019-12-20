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
    private static extern IntPtr _CalcHomography(IntPtr instance, IntPtr src, IntPtr dst, int length);

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

    public List<float> CalculateHomography(Vector3[] src, Vector3[] dst)
    {
        List<float> homography = new List<float>();

        // Homography needs at least four points for both point sets
        if (src.Length != dst.Length
            || src.Length < 4) return homography;

        // Pin array of source points
        GCHandle pinnedArray = GCHandle.Alloc(src, GCHandleType.Pinned);
        IntPtr ptrSrc = pinnedArray.AddrOfPinnedObject();

        // Pin array of destination points
        GCHandle pinnedDst = GCHandle.Alloc(dst, GCHandleType.Pinned);
        IntPtr ptrDst = pinnedDst.AddrOfPinnedObject();

        for (int i = 0; i < 9; i++)
        {
            homography.Add(0);
        }
        IntPtr resPtr = IntPtr.Zero;
        IntPtr res = _CalcHomography(instance, ptrSrc, ptrDst, src.Length );

        homography = MarshalHomographyValues(res, homography.Count);

        pinnedArray.Free();
        pinnedDst.Free();

        return homography;
    }

    private static List<float> MarshalHomographyValues(IntPtr hMatPtr, int listSize)
    {
        var homographyValueList = new List<float>();
        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(float));

        for (int i = 0; i < listSize; i++)
        {
            IntPtr thisDataPtr = new IntPtr(hMatPtr.ToInt32() + offset);
            float oneData = (float)Marshal.PtrToStructure(thisDataPtr, typeof(float));
            homographyValueList.Add(oneData);
            offset += pointSize;
        }
        return homographyValueList;
    }
}
