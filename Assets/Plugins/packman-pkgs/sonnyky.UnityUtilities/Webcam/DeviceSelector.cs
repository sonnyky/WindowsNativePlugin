using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeviceSelector : MonoBehaviour
{

    public UnityEvent m_DevicesReadyEvent;

    private WebCamDevice[] devices;

    // Use this for initialization
    void Start()
    {
        devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
            Debug.Log(devices[i].name);

        m_DevicesReadyEvent.Invoke();
    }

    public int GetNumberOfDevices()
    {
        return devices.Length;
    }

    public WebCamDevice GetDevice(int id)
    {

        if (id > devices.Length - 1)
        {
            Debug.LogError("Specified ID exceeds number of available devices");
            return devices[0];
        }
        return devices[id];
    }

}
