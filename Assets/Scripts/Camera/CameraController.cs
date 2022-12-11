using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Cameras References")]
    [SerializeField]
    private GameObject playerAimCamera;
    [SerializeField]
    private GameObject playerClimbCamera;
    [SerializeField]
    private GameObject playerMovementCamera;
    [SerializeField]
    private GameObject playerDialogueCamera;
    [SerializeField]
    private GameObject playerCombatCamera;

    private bool _onCombat = false;
    private float _maxSpeedX = 0;
    private float _maxSpeedY = 0;

    private void Start()
    {
        playerAimCamera.SetActive(false);
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
        playerCombatCamera.SetActive(false);
        playerDialogueCamera.SetActive(false);
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnter += DialogueEnterCameras;
        DialogueManager.OnDialogueExit += DialogueExitCameras;
        PlayerAim.OnAimActive += AimActiveCameras;
        PlayerAim.OnAimInactive += AimInactiveCameras;
        PlayerClimb.OnClimbingEnter += ClimbingEnterCameras;
        PlayerClimb.OnClimbingExit += ClimbingExitCameras;
        EnemyManager.OnCombatEnter += CombatEnterCameras;
        EnemyManager.OnCombatExit += CombatExitCameras;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnter -= DialogueEnterCameras;
        DialogueManager.OnDialogueExit -= DialogueExitCameras;
        PlayerAim.OnAimActive -= AimActiveCameras;
        PlayerAim.OnAimInactive -= AimInactiveCameras;
        PlayerClimb.OnClimbingEnter -= ClimbingEnterCameras;
        PlayerClimb.OnClimbingExit -= ClimbingExitCameras;
        EnemyManager.OnCombatEnter -= CombatEnterCameras;
        EnemyManager.OnCombatExit -= CombatExitCameras;
    }

    private void CombatEnterCameras()
    {
        _onCombat = true;
        playerCombatCamera.SetActive(true);
        playerAimCamera.SetActive(false);
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(false);
    }

    private void CombatExitCameras()
    {
        _onCombat = false;
        playerCombatCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
        playerClimbCamera.SetActive(false);
        playerAimCamera.SetActive(false);
    }

    private void DialogueEnterCameras()
    {
        playerDialogueCamera.SetActive(true);
        playerMovementCamera.SetActive(false);
    }

    private void DialogueExitCameras()
    {
        playerDialogueCamera.SetActive(false);
        playerMovementCamera.SetActive(true);
    }

    private void AimActiveCameras()
    {
        playerAimCamera.SetActive(true);
        playerClimbCamera.SetActive(false);
        playerMovementCamera.SetActive(false);
    }

    private void AimInactiveCameras()
    {
        playerAimCamera.SetActive(false);
        if(!_onCombat)
            playerMovementCamera.SetActive(true);
    }

    private void ClimbingEnterCameras()
    {
        playerAimCamera.SetActive(false);
        playerClimbCamera.SetActive(true);
        playerMovementCamera.SetActive(false);
    }

    private void ClimbingExitCameras()
    {
        playerClimbCamera.SetActive(false);
        if (!_onCombat)
            playerMovementCamera.SetActive(true);
    }

    public void StopPlayerCameraMovement()
    {
        CinemachineFreeLook cinemachineFreeLook = playerMovementCamera.GetComponent<CinemachineFreeLook>();
        _maxSpeedX = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
        _maxSpeedY = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;

    }

    public void RestorePlayerCameraMovement()
    {
        CinemachineFreeLook cinemachineFreeLook = playerMovementCamera.GetComponent<CinemachineFreeLook>();
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = _maxSpeedX;
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = _maxSpeedY;
    }
}
