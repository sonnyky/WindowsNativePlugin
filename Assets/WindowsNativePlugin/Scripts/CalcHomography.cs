using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CalcHomography : MonoBehaviour {
    [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_create")]
    private static extern IntPtr _Create();

    [DllImport("uplugin_cv_util", EntryPoint = "com_tinker_cv_util_calc_homography")]
    private static extern void _CalcHomography(IntPtr instance, IntPtr posPtr, int length, ref float result);

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
        Vector3[] testPos = new Vector3[2];

        testPos[0].x = 12f;testPos[0].y = 15f;testPos[0].z = 23f;
        testPos[1].x = 12f; testPos[1].y = 15f; testPos[1].z = 23f;
        testing(testPos);
    }

    private void Update()
    {
        
    }

    void testing(Vector3[] theList)
    {
        GCHandle pinnedArray = GCHandle.Alloc(theList, GCHandleType.Pinned);
        IntPtr ptr = pinnedArray.AddrOfPinnedObject();
        Debug.Log("Lenght of data to pass  " + theList.Length);
        float oneData = 0;
        _CalcHomography(instance, ptr, theList.Length, ref oneData);

        Debug.Log("after call " + oneData);
        pinnedArray.Free();
    }
}
