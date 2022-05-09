using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    public static void DebugLog(string text)
    { 
        if (Debug.isDebugBuild)
            Debug.Log(text);
    }
}
