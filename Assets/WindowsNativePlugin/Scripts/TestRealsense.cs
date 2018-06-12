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
   

    public void GetTestHomography()
    {
        float[] testSrcPts = new float[8];
        float[] testDestPts = new float[8];

        testSrcPts[0] = 100f;
        testSrcPts[1] = 100f;
        testSrcPts[2] = 300f;
        testSrcPts[3] = 100f;
        testSrcPts[4] = 300f;
        testSrcPts[5] = 300f;
        testSrcPts[6] = 100f;
        testSrcPts[7] = 300f;

        testDestPts[0] = 100f;
        testDestPts[1] = 100f;
        testDestPts[2] = 300f;
        testDestPts[3] = 100f;
        testDestPts[4] = 300f;
        testDestPts[5] = 300f;
        testDestPts[6] = 100f;
        testDestPts[7] = 300f;

        int size = 0;
        List<float> hmMat = new List<float>();
        realsenseInterface.GetHomography(ref hmMat, testSrcPts[0], testSrcPts[1], testSrcPts[2], testSrcPts[3], testSrcPts[4], testSrcPts[5], testSrcPts[6], testSrcPts[7],
            testDestPts[0], testDestPts[1], testDestPts[2], testDestPts[3], testDestPts[4], testDestPts[5], testDestPts[6], testDestPts[7], ref size);

        for (int i = 0; i < size; i++)
        {
            Debug.Log("Mat value : " + hmMat[i]);
        }

    }

    public void SetupDetectionParams()
    {
        realsenseInterface.SetDetectionParams(1800, 1600, 1200, 1000, 3000, 50000, 5);
    }

}
