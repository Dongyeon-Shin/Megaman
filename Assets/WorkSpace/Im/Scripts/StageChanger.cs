using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChanger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase Vcam1;
    [SerializeField] CinemachineVirtualCameraBase Vcam2;
    [SerializeField] CinemachineVirtualCameraBase Vcam3;
    [SerializeField] CinemachineVirtualCameraBase Vcam4;
    [SerializeField] CinemachineVirtualCameraBase Vcam5;
    [SerializeField] CinemachineVirtualCameraBase redDoor;
    [SerializeField] CinemachineVirtualCameraBase blueDoor;
    [SerializeField] GameObject laser1;
    [SerializeField] GameObject laser2;
    [SerializeField] GameObject laser3;
    [SerializeField] GameObject laser4;
    [SerializeField] GameObject laser5;
    [SerializeField] GameObject laser6;
    [SerializeField] GameObject laser7;
    [SerializeField] GameObject laser8;
    [SerializeField] GameObject laser9;
    [SerializeField] GameObject laser10;
    [SerializeField] GameObject laser11;

    private void Awake()
    {
        Vcam1.Priority = 10;
        Vcam2.Priority = 0;
        Vcam3.Priority = 0;
        Vcam4.Priority = 0;
        Vcam5.Priority = 0;
        redDoor.Priority = 0;
        blueDoor.Priority = 0;

        laser1.gameObject.SetActive(false);
        laser2.gameObject.SetActive(false);
        laser3.gameObject.SetActive(false);
        laser4.gameObject.SetActive(false);
        laser5.gameObject.SetActive(false);
        laser6.gameObject.SetActive(false);
        laser7.gameObject.SetActive(false);
        laser8.gameObject.SetActive(false);
        laser9.gameObject.SetActive(false);
        laser10.gameObject.SetActive(false);
        laser11.gameObject.SetActive(false);
    }

    public void S1()
    {
        Vcam1.Priority = 10;
        Vcam2.Priority = 0;
        Vcam3.Priority = 0;
        Vcam4.Priority = 0;
        Vcam5.Priority = 0;
    }
    public void S1toS2()
    {
        Vcam1.Priority = 0;
        Vcam2.Priority = 10;
        Vcam3.Priority = 0;
        Vcam4.Priority = 0;
        Vcam5.Priority = 0;
        laser1.gameObject.SetActive(true);
        laser2.gameObject.SetActive(true);
        laser3.gameObject.SetActive(true);
        laser4.gameObject.SetActive(true);
    }
    public void S2toS3()
    {
        Vcam1.Priority = 0;
        Vcam2.Priority = 0;
        Vcam3.Priority = 10;
        Vcam4.Priority = 0;
        Vcam5.Priority = 0;
        laser1.gameObject.SetActive(false);
        laser2.gameObject.SetActive(false);
        laser3.gameObject.SetActive(false);
        laser4.gameObject.SetActive(false);
    }
    public void S3toS4()
    {
        Vcam1.Priority = 0;
        Vcam2.Priority = 0;
        Vcam3.Priority = 0;
        Vcam4.Priority = 10;
        Vcam5.Priority = 0;
        laser5.gameObject.SetActive(true);
        laser6.gameObject.SetActive(true);
        laser7.gameObject.SetActive(true);
        laser8.gameObject.SetActive(true);
        laser9.gameObject.SetActive(true);
        laser10.gameObject.SetActive(true);
        laser11.gameObject.SetActive(true);
    }
    public void S4toS5()
    {
        Vcam1.Priority = 0;
        Vcam2.Priority = 0;
        Vcam3.Priority = 0;
        Vcam4.Priority = 0;
        Vcam5.Priority = 10;
        laser5.gameObject.SetActive(false);
        laser6.gameObject.SetActive(false);
        laser7.gameObject.SetActive(false);
        laser8.gameObject.SetActive(false);
        laser9.gameObject.SetActive(false);
        laser10.gameObject.SetActive(false);
        laser11.gameObject.SetActive(false);
    }
    IEnumerator Red()
    {
        redDoor.Priority = 30;
        yield return new WaitForSeconds(6);
        redDoor.Priority = 0;
    }
    IEnumerator Blue()
    {
        blueDoor.Priority = 30;
        yield return new WaitForSeconds(6);
        blueDoor.Priority = 0;
    }
    public void RedDoor()
    {
        StartCoroutine(Red());
    }
    public void BlueDoor()
    {
        StartCoroutine(Blue());
    }
}
