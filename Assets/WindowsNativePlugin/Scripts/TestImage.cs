using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestImage : MonoBehaviour {
    private RealsenseInterface realsenseInterface;
    private bool canGetImage = false;
    public Renderer rend;
    private Texture2D tex;
    private Color32[] pixel32;

    private GCHandle pixelHandle;
    private IntPtr pixelPtr;
    // Use this for initialization
    void Start () {
        realsenseInterface = GameObject.Find("CameraInterface").GetComponent<RealsenseInterface>();
        InitTexture();
        rend.material.mainTexture = tex;
    }

    void InitTexture()
    {
        tex = new Texture2D(1280, 720, TextureFormat.RGBA32, false);
        pixel32 = tex.GetPixels32();
        //Pin pixel32 array
        pixelHandle = GCHandle.Alloc(pixel32, GCHandleType.Pinned);
        //Get the pinned address
        pixelPtr = pixelHandle.AddrOfPinnedObject();
    }

    // Update is called once per frame
    void Update () {
        if (canGetImage)
        {
            int width = 0, height = 0;
            
            realsenseInterface.GetThresholdedImage(ref pixelPtr, ref width, ref height);
            tex.SetPixels32(pixel32);
            tex.Apply();
            //tex.LoadRawTextureData(image);
            //tex.Apply();
            //rend.material.mainTexture = tex;
        }
    }
    public void StartCapture()
    {
        canGetImage = true;
    }

    void OnApplicationQuit()
    {
        //Free handle
        pixelHandle.Free();
    }
}
