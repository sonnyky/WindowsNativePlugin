using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shape : MonoBehaviour {

    private IntPtr captureInstance;

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "Unity Shape Detection For Webcam";

    [DllImport("uplugin_shape", EntryPoint = "com_tinker_recognition_create")]
    private static extern IntPtr _Create();

    [DllImport("uplugin_shape", EntryPoint = "com_tinker_recognition_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    [DllImport("uplugin_shape", EntryPoint = "com_tinker_recognition_setup_camera", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _SetupCamera(IntPtr instance, int camId);

    [DllImport("uplugin_shape", EntryPoint = "com_tinker_recognition_release_camera", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _ReleaseCamera(IntPtr instance);

    [DllImport("uplugin_shape", EntryPoint = "com_tinker_recognition_get_color_image", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _GetColorImage(IntPtr instance, IntPtr data, ref int width, ref int height);


    public void InitiateDevice()
    {
        captureInstance = _Create();
        logger.Log(kTAG, "Loaded the plugin : " + Marshal.PtrToStringAnsi(_GetPluginName(captureInstance)));
    }

    public void GetColorImage(ref IntPtr pixPtr, ref int width_, ref int height_)
    {
        int width = 0, height = 0;
        _GetColorImage(captureInstance, pixPtr, ref width, ref height);
        width_ = width;
        height_ = height;
    }

    public void SetupCamera(int camId)
    {
        _SetupCamera(captureInstance, camId);
    }

    public void ReleaseCamera()
    {
        _ReleaseCamera(captureInstance);
    }

}
