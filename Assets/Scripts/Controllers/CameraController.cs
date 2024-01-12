using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineBrain _cinemachineBrain;
    [SerializeField]
    private CinemachineVirtualCamera _mainCamera;
    [SerializeField]
    private CinemachineVirtualCamera _fullScreenCamera;
    [SerializeField]
    private CinemachineVirtualCamera _doorCamera;

    public void Start()
    {
        SetUpCameras();
        GameManager.Instance.GotKeyAction += ShowDoor;
    }

    private void OnDestroy()
    {
        GameManager.Instance.GotKeyAction -= ShowDoor;

    }

    private void SetUpCameras()
    {
        if (_mainCamera.Follow == null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            _mainCamera.Follow = player.transform;
        }
        _doorCamera.gameObject.SetActive(false);
        _mainCamera.gameObject.SetActive(true);
    }
    private void ShowDoor()
    {
        if (_doorCamera.Follow == null)
        {
            DoorController door = FindObjectOfType<DoorController>();
            _doorCamera.Follow = door.transform;
        }
        // _mainCamera.gameObject.SetActive(false);
        _doorCamera.gameObject.SetActive(true);
        StartCoroutine(SwitchBackToMainCamera());
    }
    
    private IEnumerator SwitchBackToMainCamera()
    {
        yield return new WaitForSeconds(4.2f);
        // _mainCamera.gameObject.SetActive(true);
        _doorCamera.gameObject.SetActive(false);

    }
}
