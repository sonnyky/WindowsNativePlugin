using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CalcHomography : MonoBehaviour
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
        Vector3[] testSrc = new Vector3[4];
        Vector3[] testDst = new Vector3[4];

        testSrc[0].x = 12f; testSrc[0].y = 15f; testSrc[0].z = 23f;
        testSrc[1].x = 120f; testSrc[1].y = 15f; testSrc[1].z = 23f;
        testSrc[2].x = 120f; testSrc[2].y = 15f; testSrc[2].z = 100f;
        testSrc[3].x = 12f; testSrc[3].y = 15f; testSrc[3].z = 100f;

        testDst[0].x = 12f; testDst[0].y = 15f; testDst[0].z = 23f;
        testDst[1].x = 120f; testDst[1].y = 15f; testDst[1].z = 23f;
        testDst[2].x = 120f; testDst[2].y = 15f; testDst[2].z = 100f;
        testDst[3].x = 12f; testDst[3].y = 15f; testDst[3].z = 100f;

        testing(testSrc, testDst);
    }

    private void Update()
    {

    }

    void testing(Vector3[] src, Vector3[] dst)
    {
        // Homography needs at least four points for both point sets
        if (src.Length != dst.Length
            || src.Length < 4) return;

        // Pin array of source points
        GCHandle pinnedArray = GCHandle.Alloc(src, GCHandleType.Pinned);
        IntPtr ptrSrc = pinnedArray.AddrOfPinnedObject();

        // Pin array of destination points
        GCHandle pinnedDst = GCHandle.Alloc(dst, GCHandleType.Pinned);
        IntPtr ptrDst = pinnedDst.AddrOfPinnedObject();

        List<float> homography = new List<float>();
        for (int i = 0; i < 9; i++)
        {
            homography.Add(0);
        }
        IntPtr resPtr = IntPtr.Zero;
        IntPtr res = _CalcHomography(instance, ptrSrc, ptrDst, src.Length );

        homography = MarshalHomographyValues(res, homography.Count);

        pinnedArray.Free();
        pinnedDst.Free();
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
