using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestNative : MonoBehaviour {
    private XtionInterface xtionInterface;

    // Use this for initialization
    void Start () {
        xtionInterface = GameObject.Find("CameraInterface").GetComponent<XtionInterface>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenDevice()
    {
        xtionInterface.OpenDevice();
    }
    public void CloseDevice()
    {
        xtionInterface.CloseDevice();
    }
    public void GetDeviceName()
    {
        xtionInterface.GetDeviceName();
    }

    public void GetDeviceFirstName()
    {
        xtionInterface.GetDeviceFirstName();
    }

    public void GetVendorName()
    {
        xtionInterface.GetVendorname();
    }
    public void GetInitFlag()
    {
        xtionInterface.GetInitFlag();
    }
    public void GetUsbProductId()
    {
        xtionInterface.GetUsbProductId();
    }
}
