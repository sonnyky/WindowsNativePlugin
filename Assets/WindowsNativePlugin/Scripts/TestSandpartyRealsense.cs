using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestSandpartyRealsense : MonoBehaviour
{
    private RealsenseSandparty realsenseInterface;
    private HeightMapRealsense heightMap;
    private bool canGetDepth = false;

    // Use this for initialization
    void Start()
    {
        realsenseInterface = GameObject.Find("CameraInterface").GetComponent<RealsenseSandparty>();
        heightMap = GameObject.Find("HeightMap").GetComponent<HeightMapRealsense>();
    }

    private void Update()
    {

    }

    public void InitDevice()
    {
        realsenseInterface.InitiateDevice();
    }

    public void ListDevices()
    {
        realsenseInterface.ListDevices();
    }

    public void GetDebugMessage()
    {
        realsenseInterface.GetDebugMessage();
    }

    public void GetErrorMessage()
    {
        realsenseInterface.GetErrorMessage();
    }

    public void StartThread()
    {
        realsenseInterface.StartThread();
    }

    public void GetDepthData()
    {
        heightMap.StartGenerate();
    }
    public void GenerateHeightMap()
    {
        heightMap.StartGenerate();
    }
}
