using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAim : MonoBehaviour
{
    public static event Action OnAimActive;
    public static event Action OnAimInactive;

    public bool IsAiming { get; private set; }

    [Header("Aim Camera Settings")]
    [SerializeField]
    private float turnSmoothTime = 0.1f;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip chargingAimSound;

    private float _turnSmoothVelocity;
    private bool _canAim = true;
    private bool _wasAiming = false;

    private void OnEnable()
    {
        PlayerClimb.OnClimbingEnter += DisableAim;
        PlayerClimb.OnClimbingExit += EnableAim;

        DialogueManager.OnDialogueEnter += DisableAim;
        DialogueManager.OnDialogueExit += EnableAim;

        GameManager.OnPause += HandlePause;
    }

    private void OnDisable()
    {
        PlayerClimb.OnClimbingEnter -= DisableAim;
        PlayerClimb.OnClimbingExit -= EnableAim;

        DialogueManager.OnDialogueEnter -= DisableAim;
        DialogueManager.OnDialogueExit -= EnableAim;

        GameManager.OnPause -= HandlePause;
    }

    private void Update()
    {
        if (!_canAim)
            return;

        if (InputManager.Instance.IsAimPressedDown)
        {
            IsAiming = true;
            OnAimActive?.Invoke();
            if (!_wasAiming)
            {
                AudioManager.Instance.PlaySound(chargingAimSound, true);
                _wasAiming = true;
            }
        }

        if (InputManager.Instance.IsAimPressedUp)
        {
            IsAiming = false;
            OnAimInactive?.Invoke(); 
            if (_wasAiming)
            {
                AudioManager.Instance.StopSound(chargingAimSound);
                _wasAiming = false;
            }
        }

        if(IsAiming)
        {
            // Calcolo rotazione player tenendo conto di dove sta guardando la camera
            float targetAngle = Mathf.Atan2(Vector3.zero.x, Vector3.zero.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void DisableAim() => _canAim = false;
    private void EnableAim() => _canAim = true;

    private void HandlePause(bool isPause)
    {
        if(isPause)
            DisableAim();
        else
            EnableAim();
    }
}
