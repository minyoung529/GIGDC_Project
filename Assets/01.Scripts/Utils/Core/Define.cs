using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public static Camera mainCam;
    public static Camera MainCam
    {
        get
        {
            if (mainCam == null)
            {
                mainCam = Camera.main;
            }

            return mainCam;
        }
    }
}
