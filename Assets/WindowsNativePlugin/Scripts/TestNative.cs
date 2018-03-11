using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestNative : MonoBehaviour {
    private XtionInterface xtionInterface;

    // Use this for initialization
    void Start () {
        xtionInterface = GameObject.Find("CameraInterface").GetComponent<XtionInterface>();
    }
	
    public void CloseDevice()
    {
        xtionInterface.CloseDevice();
    }
    public void GetDeviceName()
    {
        xtionInterface.GetDeviceName();
    }

    public void GetVendorName()
    {
        xtionInterface.GetVendorname();
    }
    public void GetInitFlag()
    {
        xtionInterface.GetInitFlag();
    }
    public void StartDepthStream()
    {
        xtionInterface.StartDepthStream();
    }
    public void GetDepthData()
    {
        xtionInterface.GetDepthData();
    }
  
}
