using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestNative : MonoBehaviour {
    private XtionInterface xtionInterface;
    private HeightMap heightMap;

    // Use this for initialization
    void Start () {
        xtionInterface = GameObject.Find("CameraInterface").GetComponent<XtionInterface>();
        heightMap = GameObject.Find("HeightMap").GetComponent<HeightMap>();
    }
	
    public void CloseDevice()
    {
        heightMap.StopGenerate();
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
    public void InitDevice()
    {
        xtionInterface.InitiateDevice();
    }
    public void StartDepthStream()
    {
        xtionInterface.StartDepthStream();
    }
    public void GetDepthData()
    {
        xtionInterface.GetDepthData();
    }
    public void GenerateHeightMap()
    {
        heightMap.StartGenerate();
    }
  
    public void GetErrorMessage()
    {
        xtionInterface.GetErrorMessage();
    }
}
