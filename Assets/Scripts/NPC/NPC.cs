using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : Actor, IInteractable
{
    public bool IsAlive { get; protected set; }

    [Header("NPC References")]
    [SerializeField]
    protected Animator Animator = null;
    [SerializeField]
    protected MeshRenderer EyesRenderer = null;
    [SerializeField]
    protected Material GlowMat = null;
    [SerializeField]
    protected Material BlackMat = null;

    public abstract void Interact();

    public void Rise()
    {
        if (IsAlive)
            return;
        
        IsAlive = true;
        Animator.SetTrigger("Rise");
        EyesRenderer.material = GlowMat;
    }

    protected virtual void Awake()
    {
        EyesRenderer.material = BlackMat;
        IsAlive = false;
    }
}
