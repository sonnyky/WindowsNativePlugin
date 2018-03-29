using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class XtionInterface : MonoBehaviour
{
    // This is the pointer to the xtion_capture class.
    private IntPtr captureInstance;

    private UInt16 variable;
    private short[] returnedData;

    // This method creates the xtion_capture instance.
    [DllImport("XtionCapture", EntryPoint = "com_tinker_xtion_capture_create")]
    private static extern IntPtr _Create();

    [DllImport("XtionCapture", EntryPoint = "com_tinker_init_openni")]
    private static extern void _InitOpenNI(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_open_device")]
    public static extern IntPtr _OpenDevice(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_close_device")]
    public static extern void _CloseDevice(IntPtr instance);

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "XtionUnity";

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_error_message")]
    private static extern IntPtr _GetErrorMessage(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_device_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetDeviceName(IntPtr instance);
   
    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_vendor_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetVendorName(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_start_depth_stream", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _StartDepthStream(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_depth_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetDepthData(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_test_depth_data", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern int _GetTestDepthData(IntPtr instance);


    private void Awake()
    {
        returnedData = new short[307200];
    }

    private void Start()
    {
    }

    public void InitiateDevice()
    {
        captureInstance = _Create();
        _InitOpenNI(captureInstance);
        _OpenDevice(captureInstance);
        logger.Log(kTAG, "Loaded the plugin : " + Marshal.PtrToStringAnsi(_GetPluginName(captureInstance)));
    }

    public void CloseDevice()
    {
        _CloseDevice(captureInstance);
    }

    public void GetDeviceName()
    {
        logger.Log(kTAG, "get device name : " + Marshal.PtrToStringAnsi(_GetDeviceName(captureInstance)));
    }
    public void GetVendorname()
    {
        logger.Log(kTAG, "get vendor name : " + Marshal.PtrToStringAnsi(_GetVendorName(captureInstance)));
    }

    public void StartDepthStream()
    {
        logger.Log(kTAG, "get vendor name : " + Marshal.PtrToStringAnsi(_StartDepthStream(captureInstance)));
        Debug.Log("Data buffer length : " + returnedData.Length);
    }
    public void GetDepthData()
    {        
        IntPtr p = _GetDepthData(captureInstance);
        //Marshal.PtrToStructure(_GetDepthData(captureInstance), depthData); //unused
       
        Marshal.Copy(p, returnedData, 0, returnedData.Length);
        logger.Log(kTAG, "depth : " + returnedData[153920]);
    }

    public void GetReturnedDepthData(ref short[] dataRequest)
    {
        dataRequest = returnedData;
    }

    public void GetErrorMessage()
    {
        logger.Log(kTAG, "get error message : " + Marshal.PtrToStringAnsi(_GetErrorMessage(captureInstance)));
    }
}
