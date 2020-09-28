using UnityEngine;
using System.Collections;

public class AISensor : MonoBehaviour
{
    private AIStateMachine aiStateMachine;

    //public
    public void SetAIStateMachine(AIStateMachine aIStateMachine)
    {
        aiStateMachine = aIStateMachine;
    }


    void OnTriggerEnter(Collider col)
    {
        if (aiStateMachine != null)
            aiStateMachine.OnTriggerEvent(AITriggerEventType.Enter,col);
    }

    void OnTriggerStay(Collider col)
    {
        if (aiStateMachine != null)
            aiStateMachine.OnTriggerEvent(AITriggerEventType.Stay, col);
    }

    void OnTriggerExit(Collider col)
    {
        if (aiStateMachine != null)
            aiStateMachine.OnTriggerEvent(AITriggerEventType.Exit, col);
    }
}
