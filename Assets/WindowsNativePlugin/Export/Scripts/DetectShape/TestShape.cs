using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TestShape : MonoBehaviour {

    #region Declarations

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

        webcam.InitiateDevice();

        webcam.SetupCamera(0);

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
}
