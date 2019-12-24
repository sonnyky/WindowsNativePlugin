using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CvUtilTest : MonoBehaviour {

    List<float> homography;
    CalcHomography m_HomographyCalculator;

    // Use this for initialization
    void Start () {

        m_HomographyCalculator = new CalcHomography();
        m_HomographyCalculator.InitiateDevice();

        homography = new List<float>();

        Vector3[] testSrc = new Vector3[4];
        Vector3[] testDst = new Vector3[4];

        testSrc[0].x = 12f; testSrc[0].y = 15f; testSrc[0].z = 23f;
        testSrc[1].x = 120f; testSrc[1].y = 15f; testSrc[1].z = 23f;
        testSrc[2].x = 120f; testSrc[2].y = 0f; testSrc[2].z = 100f;
        testSrc[3].x = 12f; testSrc[3].y = 0f; testSrc[3].z = 100f;

        testDst[0].x = 12f; testDst[0].y = 15f; testDst[0].z = 23f;
        testDst[1].x = 120f; testDst[1].y = 15f; testDst[1].z = 23f;
        testDst[2].x = 120f; testDst[2].y = 0f; testDst[2].z = 100f;
        testDst[3].x = 12f; testDst[3].y = 0f; testDst[3].z = 100f;

        homography = m_HomographyCalculator.CalculateHomography(testSrc, testDst);

        for(int i=0; i<homography.Count; i++)
        {
            Debug.Log(homography[i]);
        }
    }
}
