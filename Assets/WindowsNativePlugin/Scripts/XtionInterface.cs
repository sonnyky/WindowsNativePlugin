using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class XtionInterface : MonoBehaviour
{
    // This is the pointer to the xtion_capture class.
    private IntPtr captureInstance;
    private string device;

    private UInt16[] depthData;
    private UInt16 variable;

    // This method creates the xtion_capture instance.
    [DllImport("XtionCapture", EntryPoint = "com_tinker_xtion_capture_create")]
    private static extern IntPtr _Create();

    [DllImport("XtionCapture", EntryPoint = "com_tinker_init_openni")]
    private static extern void _InitOpenNI(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_open_device")]
    public static extern IntPtr _OpenDevice(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_close_device")]
    public static extern void _CloseDevice();

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "TestNativeTag";

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_capture_handle")]
    private static extern IntPtr _GetHandle(IntPtr instance);

    // Helper methods to retrieve initialization status
    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_init_flag")]
    private static extern bool _GetInitFlag(IntPtr instance);

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

    private void Start()
    {
        depthData = new UInt16[307200];
        InitiateDevice();
    }

    private void InitiateDevice()
    {
        captureInstance = _Create();
        logger.Log(kTAG, "The instance pointer : " + captureInstance);
        _InitOpenNI(captureInstance);
        device = Marshal.PtrToStringAnsi(_OpenDevice(captureInstance));
       
        logger.Log(kTAG, "get plugin name : " + Marshal.PtrToStringAnsi(_GetPluginName(captureInstance)));
    }

    public void CloseDevice()
    {
        _CloseDevice();
    }

    public void GetInitFlag()
    {
        logger.Log(kTAG, "get init flag : " + _GetInitFlag(captureInstance));
        if (!_GetInitFlag(captureInstance))
        {
            logger.Log(kTAG, "error : " + Marshal.PtrToStringAnsi(_GetErrorMessage(captureInstance)));
        }
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
    }
    public void GetDepthData()
    {
        logger.Log(kTAG, "depth data pointer: " + _GetDepthData(captureInstance));
        
        IntPtr p = _GetDepthData(captureInstance);
        //Marshal.PtrToStructure(_GetDepthData(captureInstance), depthData);
        byte[] returnedData = new byte[307200];
        Marshal.Copy(p, returnedData, 0, returnedData.Length);
        logger.Log(kTAG, "size of pointer : " + returnedData[153600]);
    }
}
