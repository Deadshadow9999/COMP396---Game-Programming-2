using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollFSM_for_Finite_State_Machine : MonoBehaviour
{
    //                SawPlayer
    // Patroling   ----->    ChasingPlayer
    //                 <-----
    //                LostPlayer

    public GameObject player;
    public Transform[] path;// waypoints arrow that form a path
    public FiniteStateMachine fsm;

    public float closeEnoughDistance = 5;
    public Color patrolingColor = Color.green;
    public Color chasingColor = Color.red;


    // Start is called before the first frame update
    void Start()
    {
        fsm = new FiniteStateMachine();

        // create patroling state and register it to the machine
        var patrolingState = fsm.CreateState("Patroling");
        // register onEnter, onFrame, and onExit
        patrolingState.onEnter = delegate
        {
            print("Enter Patroling state");
        };
        patrolingState.onframe = delegate
        {
            print("On frame of Patroling state");
            // check if close enough to player and chase
            if(CheckIfCloseEnough(player.transform.position, this.gameObject.transform.position))
            {
                fsm.TransitionTo("Chasing");
            }
        };
        patrolingState.onExit = delegate
        {
            print("Exiting Patroling state");
        };

        // create chasingPlayer state
        var chasingState = fsm.CreateState("Chasing");
        // register onEnter, onFrame, and onExit
        chasingState.onEnter = delegate
        {
            print("Enter Chasing state");
            this.gameObject.GetComponent<Renderer>().sharedMaterial.color = chasingColor;
        };
        chasingState.onframe = delegate
        {
            print("On frame of Chasing state");
            // check if not close enough to player and patrol
            if (!CheckIfCloseEnough(player.transform.position, this.gameObject.transform.position))
            {
                fsm.TransitionTo("Patroling");
                this.gameObject.GetComponent<Renderer>().sharedMaterial.color = patrolingColor;
            }
        };
        chasingState.onExit = delegate
        {
            print("Exiting Chasing state");
        };
    }

    private bool CheckIfCloseEnough(Vector3 player, Vector3 troll)
    {
        return Vector3.Distance(player, troll) < closeEnoughDistance;
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }
}
