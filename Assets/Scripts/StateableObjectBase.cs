using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateableObjectBase : MonoBehaviour
{
    [SerializeField] protected GameObject defaultState;

    protected List<GameObject> allStates;

    protected virtual void Awake()
    {
        allStates = GetAllPossibleStates();

        SetStateByName(defaultState.name);
    }

    protected abstract List<GameObject> GetAllPossibleStates();

    public void SetStateByName(string stateName)
    {
        foreach (var state in allStates)
            state.SetActive(state.name.Equals(stateName));
    }
}
