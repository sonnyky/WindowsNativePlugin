using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class XtionInterface : MonoBehaviour
{
    private IntPtr captureInstance;
    private IntPtr deviceAddress;

    [DllImport("XtionCapture", EntryPoint = "com_tinker_init_openni")]
    private static extern void _InitOpenNI(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_open_device")]
    public static extern void _OpenDevice(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_close_device")]
    public static extern void _CloseDevice();

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "TestNativeTag";

    [DllImport("XtionCapture", EntryPoint = "com_tinker_xtion_capture_create")]
    private static extern IntPtr _Create();

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_capture_handle")]
    private static extern IntPtr _GetHandle(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_init_flag")]
    private static extern bool _GetInitFlag(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_error_message")]
    private static extern IntPtr _GetErrorMessage(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_is_valid", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool _IsValid(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_device_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetDeviceName(IntPtr instance);
    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_first_device_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetFirstDeviceName(IntPtr instance);

    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_vendor_name", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetVendorName(IntPtr instance);
    [DllImport("XtionCapture", EntryPoint = "com_tinker_get_usb_product_id", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern int _GetUsbProdId(IntPtr instance);

    private void Start()
    {
        InitiateDevice();
    }

    private void InitiateDevice()
    {
        captureInstance = _Create();
        _InitOpenNI(captureInstance);
        _OpenDevice(captureInstance);
        logger.Log(kTAG, "get plugin name : " + Marshal.PtrToStringAnsi(_GetPluginName(captureInstance)));
    }

    public void OpenDevice()
    {
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
    public void GetDeviceFirstName()
    {
        logger.Log(kTAG, "get first device name : " + Marshal.PtrToStringAnsi(_GetFirstDeviceName(captureInstance)));
    }
    public void GetVendorname()
    {
        logger.Log(kTAG, "get vendor name : " + Marshal.PtrToStringAnsi(_GetVendorName(captureInstance)));
    }
    public void GetUsbProductId()
    {
        logger.Log(kTAG, "get us product id name : " + _GetUsbProdId(captureInstance));
    }
}
