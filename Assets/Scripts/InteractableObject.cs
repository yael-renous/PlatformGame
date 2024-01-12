using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject: MonoBehaviour
{

    protected virtual void Start()
    {
        gameObject.tag = GetTag();
    }

    public abstract string GetTag();
}
