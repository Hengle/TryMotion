using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateInfo_MonkeyKing : IAbstractStateInfo
{
    //IAbstractStateInfo
    private AIStateMachine_MonkeyKing aiStateMachine;
    public AIStateMachine_MonkeyKing AiStateMachine { get => aiStateMachine; set => aiStateMachine = value; }

}
