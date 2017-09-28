using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UMotionEditor
{
    public static class CameraUtility
    {
        //********************************************************************************
        // Public Properties
        //********************************************************************************

        //********************************************************************************
        // Private Properties
        //********************************************************************************

        //********************************************************************************
        // Public Methods
        //********************************************************************************

        public static void SetAllowHdr(Camera camera, bool hdrAllowed)
        {
            #if UNITY_5_6_OR_NEWER
            camera.allowHDR = hdrAllowed;
            #else
            camera.hdr = hdrAllowed;
            #endif
        }

        //********************************************************************************
        // Private Methods
        //********************************************************************************
    }
}
