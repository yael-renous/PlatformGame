using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : InteractableObject
{
    public static readonly string TAG_STRING = "Platform";

  
    public override string GetTag()
    {
        return TAG_STRING;
    }

  
}
