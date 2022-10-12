using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine
{
    public class State
    {

        public string name;
        public System.Action onframe;
        public System.Action onEnter;
        public System.Action onExit;

        public override string ToString()
        {
            return name;
        }
    }

        Dictionary<string, State> states = new Dictionary<string, State>();

    public State currentState { get; private set; }

    public State initialState;

    // Creates a new state, registers it, and returns it
    public State CreateState(string name)
    {
        var state = new State();
        state.name = name;
        if(states.Count == 0)
        {
            initialState = state;
        }

        states[name] = state;
        return state;
    }

    public void Update()
    {
        // no states => log error
        if(states.Count == 0)
        {
            Debug.LogErrorFormat("State machine with no states");
        }

        // no current state, transition to initial state
        if(currentState == null)
        {
            TransitionTo(initialState);
        }

        // onFrame? run it: skip it
        if(currentState.onframe != null)
        {
            currentState.onframe();
        }
    }

    public void TransitionTo(State newState)
    {
        // newState = null, log error and return
        if(newState == null)
        {
            Debug.LogErrorFormat("no transition to a null state");
            return;
        }

        // if currentState ! null and has on exit, run it, else skip it
        if(currentState != null && currentState.onExit != null)
        {
            currentState.onExit();
        }


        // Optional: log transition from currentState to newState
        Debug.LogErrorFormat($"Transition from {currentState} to {newState}");

        currentState = newState;

        // if currentState onEnter, run it, else skip it
        if(currentState.onEnter != null)
        {
            currentState.onEnter();
        }
    }

    public void TransitionTo(string stateName)
    {
        // states has no stateName key? log error
        var newState = states[stateName];
        TransitionTo(newState);
    }
}
