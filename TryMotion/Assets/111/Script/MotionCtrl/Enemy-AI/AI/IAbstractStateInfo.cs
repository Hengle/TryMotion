using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAbstractStateInfo : ScriptableObject
{
    public virtual void OnEnter(Animator animator , AnimatorStateInfo stateInfo , int layerIndex) { }
    public virtual void OnUpdate(Animator animator , AnimatorStateInfo stateInfo , int layerIndex) { }
    public virtual void OnExit(Animator animator , AnimatorStateInfo stateInfo , int layerIndex) { }
}
