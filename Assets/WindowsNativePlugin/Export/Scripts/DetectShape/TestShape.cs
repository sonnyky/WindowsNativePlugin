using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TestShape : MonoBehaviour {

    #region Declarations

    // Dropdown menu to select webcam device
    Dropdown m_Dropdown;

    List<string> options;

    int m_SelectedDeviceId = 0;


    private Shape webcam;
    private bool canGetImage = false;
    public Renderer rend;
    private Texture2D tex;
    private Color32[] pixel32;

    private GCHandle pixelHandle;
    private IntPtr pixelPtr;

    #endregion

    public void Initialize()
    {
        webcam = GameObject.Find("CameraInterface").GetComponent<Shape>();
        InitTexture();
        rend.material.mainTexture = tex;

        options = new List<string>();

        webcam.InitiateDevice();

        webcam.SetupCamera(m_SelectedDeviceId);

        m_Dropdown = GameObject.Find("Canvas").transform.Find("DeviceSelectorDropdown").GetComponent<Dropdown>();
        PrepareDropdownOptions();

    }

    void InitTexture()
    {
        tex = new Texture2D(640, 480, TextureFormat.RGBA32, false);
        pixel32 = tex.GetPixels32();
        //Pin pixel32 array
        pixelHandle = GCHandle.Alloc(pixel32, GCHandleType.Pinned);
        //Get the pinned address
        pixelPtr = pixelHandle.AddrOfPinnedObject();
    }
    // Update is called once per frame
    void Update()
    {
        if (canGetImage)
        {
            int width = 0, height = 0;

            webcam.GetColorImage(ref pixelPtr, ref width, ref height);
            tex.SetPixels32(pixel32);
            tex.Apply();
        }
    }

    public void StartCapture()
    {
        canGetImage = true;
    }

    public void StopCapture()
    {
        canGetImage = false;
    }

    void OnApplicationQuit()
    {
        webcam.ReleaseCamera();
        //Free handle
        pixelHandle.Free();
    }

    void PrepareDropdownOptions()
    {
        int numberOfOptions = GetComponent<DeviceSelector>().GetNumberOfDevices();

        m_Dropdown.ClearOptions();

        for (int i=0; i<numberOfOptions; i++)
        {
            options.Add(GetComponent<DeviceSelector>().GetDevice(i).name);            
        }

        m_Dropdown.AddOptions(options);

        m_Dropdown.onValueChanged.AddListener(
            delegate { DropdownValueChanged(m_Dropdown); }
            );

    }

    void DropdownValueChanged(Dropdown changed)
    {
        StopCapture();
        m_SelectedDeviceId = changed.value;
        webcam.ReleaseCamera();
        webcam.SetupCamera(m_SelectedDeviceId);
    }
}
