using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RealsenseInterface : MonoBehaviour {

    [StructLayout(LayoutKind.Sequential)]
    public struct PeoplePosition
    {
        public int x;
        public int y;
    }

    // This is the pointer to the realsense_capture class.
    private IntPtr captureInstance;

    // This method creates the xtion_capture instance.
    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_realsense_capture_create")]
    private static extern IntPtr _Create();

    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_get_plugin_name")]
    private static extern IntPtr _GetPluginName(IntPtr instance);

    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_list_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _ListDevices(IntPtr instance);

    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_get_depth", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern IntPtr _GetDepth(IntPtr instance, ref int size);

    [DllImport("uplugin_realsense_d415", EntryPoint = "com_tinker_remove_devices", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void _RemoveDevices(IntPtr instance);


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
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ListDevices()
    {
        IntPtr p = _ListDevices(captureInstance);
        deviceNames = MarshalStringArray(p, 4);
        print("strArray[" + 4 + "] = [" + String.Join(",", deviceNames) + "]");
    }

    public void GetDepth(ref List<PeoplePosition> listOfPeoplePositions)
    {
        int depthDataSize = 0;
        IntPtr depthDataPtr = _GetDepth(captureInstance, ref depthDataSize);

        logger.Log(kTAG, "test depth data pointer : " + depthDataPtr + " and size : " + depthDataSize);

        listOfPeoplePositions = MarshalPeoplePositionArray(depthDataPtr, depthDataSize);
    }

    // Decodes struct array from raw pointer
    private static List<PeoplePosition> MarshalPeoplePositionArray(IntPtr dataPtr, int arraySize)
    {
        var peoplePositionList = new List<PeoplePosition>();

        int offset = 0;
        int pointSize = Marshal.SizeOf(typeof(PeoplePosition));

        for (int i=0; i<arraySize; i++)
        {
            IntPtr thisDataPtr = new IntPtr(dataPtr.ToInt32() + offset);
            PeoplePosition oneData = (PeoplePosition) Marshal.PtrToStructure(thisDataPtr, typeof(PeoplePosition));
            peoplePositionList.Add(oneData);
            offset += pointSize;
            //Marshal.FreeCoTaskMem(thisDataPtr);
        }
        //Marshal.FreeCoTaskMem(dataPtr);
        return peoplePositionList;
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
        _RemoveDevices(captureInstance);
    }
    
}
