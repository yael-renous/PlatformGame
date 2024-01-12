using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : InteractableObject
{
    public static readonly string TAG_STRING = "Door";
    [SerializeField] private GameObject _doorClosedObject;
    [SerializeField] private GameObject _doorOpenBlackHole;
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.GotKeyAction += OpenDoor;
        _doorClosedObject.SetActive(true);
        _doorOpenBlackHole.SetActive(false);
    }

    public void OnDestroy()
    {
        GameManager.Instance.GotKeyAction -= OpenDoor;
    }

    public override string GetTag()
    {
        return TAG_STRING;
    }

    private void OpenDoor()
    {
        _doorClosedObject.SetActive(false);
        _doorOpenBlackHole.SetActive(true);
    }
 
}
