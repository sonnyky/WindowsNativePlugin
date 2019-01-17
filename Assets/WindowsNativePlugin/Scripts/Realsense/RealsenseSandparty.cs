using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RealsenseSandparty : MonoBehaviour
{
    // This is the pointer to the realsense_capture class.
    private IntPtr captureInstance = IntPtr.Zero;

    private short[] returnedData;

    // This method creates the xtion_capture instance.
    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_realsense_capture_create")]
    private static extern IntPtr _Create();

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_list_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _ListDevices(IntPtr instance);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_sandbox_depth", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetSandboxDepth(IntPtr instance);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_debug_message", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern string _GetDebugMessage(IntPtr instance);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_error_message", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetErrorMessage(IntPtr instance);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_thresholded_image", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _GetThresholdedImage(IntPtr instance, IntPtr data, ref int width, ref int height);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_color_image", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _GetColorImage(IntPtr instance, IntPtr data, ref int width, ref int height);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_get_homography", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetHomography(IntPtr instance, float proj_tl_x, float proj_tl_y, float proj_tr_x, float proj_tr_y, float proj_bl_x, float proj_bl_y, float proj_br_x, float proj_br_y,
        float image_tl_x, float image_tl_y, float image_tr_x, float image_tr_y, float image_bl_x, float image_bl_y, float image_br_x, float image_br_y, ref int listSize);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_remove_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _RemoveDevices(IntPtr instance);

    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_start_thread", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _StartThread(IntPtr instance);


    // This must be called before _ListDevices
    [DllImport("uplugin_sandparty_realsense", EntryPoint = "com_tinker_setup_detection_params", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _SetupCaptureParameters(IntPtr instance, int lowDistMin, int lowDistMax, int highDistMin, int highDistMax, int minBlobArea, int maxBlobArea, int erosionSize);

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "RealsenseUnity";

    // Variables to hold data in Unity readable format
    private String[] deviceNames;
    private string test = "";

    public void InitiateDevice()
    {
        captureInstance = _Create();
        logger.Log(kTAG, "Loaded the plugin : " + Marshal.PtrToStringAnsi(_GetPluginName(captureInstance)));
    }

    private void Awake()
    {
        // We only support up to 4 devices. Should be enough for now. TODO : add logic to handle when more than 10 are present.
        deviceNames = new string[4];
        returnedData = new short[921600];
    }

    public void ListDevices()
    {
        IntPtr p = _ListDevices(captureInstance);
        deviceNames = MarshalStringArray(p, 4);
        print("strArray[" + 4 + "] = [" + String.Join(",", deviceNames) + "]");
    }

    public void GetDepthData()
    {
        IntPtr depthDataPtr = IntPtr.Zero;
        depthDataPtr = _GetSandboxDepth(captureInstance);
        int pointSize = Marshal.SizeOf(typeof(short));
        if (depthDataPtr != IntPtr.Zero)
        {
            Marshal.Copy(depthDataPtr, returnedData, 0, returnedData.Length);
            // at center the index is col * (row/2) + (col/2) = 1280 * (360) + 640
            //IntPtr thisDataPtr = new IntPtr(depthDataPtr.ToInt64() + (pointSize*461440));
            //short oneData = (short)Marshal.PtrToStructure(thisDataPtr, typeof(short));
            //logger.Log(kTAG, "depth at center : " + returnedData[461440]);
        }
    }

    public void GetReturnedDepthData(ref short[] dataRequest)
    {
        dataRequest = returnedData;
    }


    public void GetThresholdedImage(ref IntPtr pixPtr, ref int width_, ref int height_)
    {
        int width = 0, height = 0;
        _GetThresholdedImage(captureInstance, pixPtr, ref width, ref height);
        width_ = width;
        height_ = height;
    }

    public void GetColorImage(ref IntPtr pixPtr, ref int width_, ref int height_)
    {
        int width = 0, height = 0;
        _GetColorImage(captureInstance, pixPtr, ref width, ref height);
        width_ = width;
        height_ = height;
    }

    public void GetHomography(ref List<float> homography, float proj_tl_x, float proj_tl_y, float proj_tr_x, float proj_tr_y, float proj_bl_x, float proj_bl_y, float proj_br_x, float proj_br_y,
        float image_tl_x, float image_tl_y, float image_tr_x, float image_tr_y, float image_bl_x, float image_bl_y, float image_br_x, float image_br_y, ref int size)
    {
        int listSize = 0;
        IntPtr hMatDataPtr = _GetHomography(captureInstance, proj_tl_x, proj_tl_y, proj_tr_x, proj_tr_y, proj_bl_x, proj_bl_y, proj_br_x, proj_br_y,
            image_tl_x, image_tl_y, image_tr_x, image_tr_y, image_bl_x, image_bl_y, image_br_x, image_br_y, ref listSize);

        // here listSize value is modified according to the number of homography matrix values

        homography = MarshalHomographyValues(hMatDataPtr, listSize);
        size = listSize;
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


    // Decodes string array from raw pointer
    private static string[] MarshalStringArray(IntPtr dataPtr, int arraySize)
    {
        var dataPtrArray = new IntPtr[arraySize];
        var strArray = new String[arraySize];
        Marshal.Copy(dataPtr, dataPtrArray, 0, arraySize);

        for (int i = 0; i < arraySize; i++)
        {
            strArray[i] = Marshal.PtrToStringAnsi(dataPtrArray[i]);
            Marshal.FreeCoTaskMem(dataPtrArray[i]);
        }
        Marshal.FreeCoTaskMem(dataPtr);

        return strArray;
    }

    private void OnApplicationQuit()
    {
        if (captureInstance != IntPtr.Zero)
        {
            _RemoveDevices(captureInstance);
        }
    }

    /// <summary>
    /// キャプチャパラメータの設定。とりあえず有効距離の閾値、Blob（塊り）の最小面積。ListDevices より前に実行しないといけない。
    /// </summary>
    /// <param name="dist">有効距離。これ以上遠い点は無視される。㎜</param>
    /// <param name="area">Blob の最小面積。小さすぎるとノイズが誤検知される。大きすぎると対象物体が検出されない。500～1500ぐらいがよさそう</param>
    public void SetDetectionParams(int lowDistMin, int lowDistMax, int highDistMin, int highDistMax, int minArea, int maxArea, int size)
    {
        _SetupCaptureParameters(captureInstance, lowDistMin, lowDistMax, highDistMin, highDistMax, minArea, maxArea, size);
    }

    public void GetDebugMessage()
    {
        logger.Log(kTAG, "get debug message : " + _GetDebugMessage(captureInstance));
    }

    public void GetErrorMessage()
    {
        logger.Log(kTAG, "get error message : " + Marshal.PtrToStringAnsi(_GetErrorMessage(captureInstance)));
    }

    public void StartThread()
    {
        _StartThread(captureInstance);
    }
}
