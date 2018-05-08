using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class RealsenseInterface : MonoBehaviour {

    // This is the pointer to the realsense_capture class.
    private IntPtr captureInstance;

    // This method creates the xtion_capture instance.
    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_realsense_capture_create")]
    private static extern IntPtr _Create();

    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "RealsenseUnity";

    public void InitiateDevice()
    {
        captureInstance = _Create();
        logger.Log(kTAG, "Loaded the plugin : " + Marshal.PtrToStringAnsi(_GetPluginName(captureInstance)));
    }


    // Update is called once per frame
    void Update () {
		
	}
}
