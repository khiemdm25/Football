using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera normalCam;

    [SerializeField] private CinemachineCamera dollyCam;

    private bool isPlaying = false;

    private Coroutine camRoutine;

    private void Start()
    {
        normalCam.gameObject.SetActive(true);

        dollyCam.gameObject.SetActive(false);
    }

    public void PlayKickCamera(Transform ballTarget)
    {
        dollyCam.Follow = ballTarget;
        dollyCam.LookAt = ballTarget;

        if (camRoutine != null)
        {
            StopCoroutine(camRoutine);
        }

        camRoutine = StartCoroutine(Cam());
    }

    IEnumerator Cam()
    {
        isPlaying = true;
        normalCam.gameObject.SetActive(false);
        dollyCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(4f);

        normalCam.gameObject.SetActive(true);
        dollyCam.gameObject.SetActive(false);
        isPlaying = false;
    }
}