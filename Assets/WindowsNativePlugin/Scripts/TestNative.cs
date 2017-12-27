using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TestNative : MonoBehaviour {

    [DllImport("XtionCapture", EntryPoint = "TestHello")]
    public static extern IntPtr TestHello();

    // Use this for initialization
    void Start () {
        IntPtr returnedChar = TestHello();
        string returnedString = Marshal.PtrToStringAnsi(returnedChar);
        Debug.Log("Returned : " + returnedString);
        Marshal.FreeHGlobal(returnedChar);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
