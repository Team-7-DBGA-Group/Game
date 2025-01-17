using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;

[System.Serializable]
public class Waypoint
{
    public Transform wTransform { get => waypointTransform; }
    public bool DoOnce { get => doOnlyOnce; }
    public bool Done { get; private set; }

    [SerializeField]
    private Transform waypointTransform;
    [SerializeField]
    private bool doOnlyOnce;
    
    public void SetDone(bool value)
    {
        Done = value;
    }
}

public class AdvancedTransportableObject : InteractableObject
{
    public bool IsTransporting { get; private set; }
    public event Action OnStopTransport;

    [SerializeField]
    private List<Waypoint> waypoints = new List<Waypoint>();

    [SerializeField]
    private float objectSpeed = 2f;
    [SerializeField]
    private bool lookAtWaypoints = true;

    [Header("Transportable sounds")]
    [SerializeField]
    private AudioClip onMovingSound;

    private GameObject _playerRef;
    private int _waypointIndex;
    private bool _canObjectMove = false;
    private bool _canRemove = false;
    private bool _soundFlag = true;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        AudioManager.OnChangedSoundVolume += ChangeSoundVolume;
    }

    private void OnDisable()
    {
        AudioManager.OnChangedSoundVolume -= ChangeSoundVolume;
    }

    public override void Interact()
    {
        if (_canObjectMove)
            return;

        base.Interact();

        if (_playerRef != null)
        {
            _playerRef.transform.parent = this.gameObject.transform;
            _playerRef.GetComponent<PlayerMovement>().StopMovement();
        }

        if (waypoints[_waypointIndex].DoOnce)
            _canRemove = true;

        CheckNextIndex();

        _canObjectMove = true;
    }

    public void SetTransport(GameObject player)
    {
        _playerRef = player;
        IsTransporting = true;
    }

    public void StopTransport()
    {
        _playerRef = null;
        IsTransporting = false;

    }

    private void Awake()
    {
        if (GetComponent<AudioSource>())
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = onMovingSound;
            _audioSource.loop = true;
        }
    }

    private void Start()
    {
        _waypointIndex = 0;
        transform.position = waypoints[_waypointIndex].wTransform.position;
        if (lookAtWaypoints)
            transform.LookAt(waypoints[(_waypointIndex + 1) % waypoints.Count].wTransform.position);
    }

    private void Update()
    {
        if (!_canObjectMove)
            return;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[_waypointIndex].wTransform.position, objectSpeed * Time.deltaTime);

        if (_soundFlag && onMovingSound != null && _audioSource != null)
        {
            _audioSource.Play();
            _soundFlag = false;
        }

        if (Vector3.Distance(transform.position, waypoints[_waypointIndex].wTransform.position) < 0.1f)
        {
            if (_canRemove) 
            {
                waypoints[checkPreviousIndex()].SetDone(true);
                _canRemove = false;
            }

            OnStopTransport?.Invoke();
            if (_playerRef != null)
            {
                _playerRef.GetComponent<PlayerMovement>().CanMove = true;
                _playerRef.transform.parent = null;
                _playerRef = null;
            }

            if (lookAtWaypoints && checkNextLookAt() > -1)
            {
                transform.LookAt(waypoints[checkNextLookAt()].wTransform.position);
            }

            _canObjectMove = false;
            _audioSource.Stop();
            _soundFlag = true;
        }
    }

    private int checkNextLookAt()
    {
        int index = _waypointIndex;
        index = (index + 1) % waypoints.Count;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[index].Done == false)
                return index;
            index = (index + 1) % waypoints.Count;
        }
        return -1;
    }
    private void CheckNextIndex()
    {
        _waypointIndex = (_waypointIndex + 1) % waypoints.Count;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[_waypointIndex].Done == false)
                return;
            _waypointIndex = (_waypointIndex + 1) % waypoints.Count;
        }
    }

    private int checkPreviousIndex()
    {
        int index = _waypointIndex;
        index = (_waypointIndex - 1 + waypoints.Count) % waypoints.Count;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[index].Done == false)
                return index;
            index = (index - 1 + waypoints.Count) % waypoints.Count;
        }
        return -1;
    }

    private void ChangeSoundVolume()
    {
        if(_audioSource != null)
            _audioSource.volume = AudioManager.Instance.GetSoundVolume();
    }
}
