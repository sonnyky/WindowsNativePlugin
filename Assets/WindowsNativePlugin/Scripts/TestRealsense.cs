using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestRealsense : MonoBehaviour {
    private RealsenseInterface realsenseInterface;

    // Use this for initialization
    void Start () {
        realsenseInterface = GameObject.Find("CameraInterface").GetComponent<RealsenseInterface>();
    }
 
    public void InitDevice()
    {
        realsenseInterface.InitiateDevice();
    }

    public void ListDevices()
    {
        realsenseInterface.ListDevices();
    }
    public void GetDepth()
    {
        realsenseInterface.GetDepth();
    }
}
