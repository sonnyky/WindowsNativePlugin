using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;

public class TestRealsense : MonoBehaviour {
    private RealsenseInterface realsenseInterface;
    private bool canGetDepth = false;

    List<RealsenseInterface.PeoplePosition> peoplePositions;

    // Use this for initialization
    void Start () {
        realsenseInterface = GameObject.Find("CameraInterface").GetComponent<RealsenseInterface>();
        peoplePositions = new List<RealsenseInterface.PeoplePosition>();
    }

    private void Update()
    {
        if (canGetDepth)
        {
            realsenseInterface.GetDepth(ref peoplePositions);
        }
    }

    public void InitDevice()
    {
        realsenseInterface.InitiateDevice();
    }

    public void ListDevices()
    {
        realsenseInterface.ListDevices();
    }
    public void GetDepth()
    {
        canGetDepth = true;
    }
}
