using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject: MonoBehaviour
{
    public static readonly string INTERACTABLE_OBJECT_LAYER = "InteractableObject";

    protected virtual void Start()
    {
        gameObject.tag = GetTag();
        gameObject.layer = LayerMask.NameToLayer(INTERACTABLE_OBJECT_LAYER);
    }

    public abstract string GetTag();
}
