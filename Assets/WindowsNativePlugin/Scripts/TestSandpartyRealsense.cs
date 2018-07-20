using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestSandpartyRealsense : MonoBehaviour
{
    private RealsenseSandparty realsenseInterface;
    private HeightMap heightMap;

    // Use this for initialization
    void Start()
    {
        realsenseInterface = GameObject.Find("CameraInterface").GetComponent<RealsenseSandparty>();
        heightMap = GameObject.Find("HeightMap").GetComponent<HeightMap>();
    }
   
    public void InitDevice()
    {
        realsenseInterface.InitiateDevice();
    }
   
    public void GetDepthData()
    {
        realsenseInterface.GetDepth();
    }
    public void GenerateHeightMap()
    {
        heightMap.StartGenerate();
    }
}
