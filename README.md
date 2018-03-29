# WindowsNativePlugin
This is a Unity project sample to demonstrate using Asus Xtion2 camera with Unity.
The plugin XtionCapture.dll provides the API needed to communicate with the Xtion2 device.

# Dependencies
* The DLL file XtionCapture.dll inside Plugins folder.
* This DLL was built on Windows 10 but if you need to rebuild the DLL you can find it [here](https://github.com/sonnyky/XtionUtil).

# Usage
* Open the scene "TestNative"
* Run the scene and press Init Device button.
* Press Vendor name and Device name to see the connected device's information.
* Press StartDepthStream to go into depth stream mode.
* This sample generates depth information as Terrain. Check the HeightMap object in the inspector for details.
* __IMPORTANT__ : Always press close device button before stopping Unity. If not, the Xtion2 will hang and will need to be disconnected
and reconnected to be detected by the OS.
