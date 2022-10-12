using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFSM_for_Finite_State_Machine : MonoBehaviour
{

    public GameObject player;
    public Transform[] path;// waypoints arrow that form a path
    public FiniteStateMachine fsm;

    public float closeEnoughDistance = 5;
    public float attackDistance = 2;
    public Color patrolingColor = Color.green;
    public Color chasingColor = Color.yellow;
    public Color attackingColor = Color.red;
    public float health = 100;
    public float criticalHealth = 10;


    // Start is called before the first frame update
    void Start()
    {
        fsm = new FiniteStateMachine();

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

            // Check if guard is close enough to player to attack
            if (CheckAttackDistance(player.transform.position, this.gameObject.transform.position))
            {
                fsm.TransitionTo("Attacking");
            }
        };
        chasingState.onExit = delegate
        {
            print("Exiting Chasing state");
        };

        var attackingState = fsm.CreateState("Attacking");
        // register onEnter, onFrame, and onExit
        attackingState.onEnter = delegate
        {
            print("Enter Attacking state");
            this.gameObject.GetComponent<Renderer>().sharedMaterial.color = attackingColor;
        };
        attackingState.onframe = delegate
        {
            print("On frame of Attacking state");
            // check if close enough to player and chase
            if (!CheckAttackDistance(player.transform.position, this.gameObject.transform.position))
            {
                fsm.TransitionTo("Chasing");
            }
            if(CheckCriticalHealth())
            {
                fsm.TransitionTo("Aggressive_Attacking");
            }
        };
        attackingState.onExit = delegate
        {
            print("Exiting attacking state");
        };

        var aggressiveAttackingState = fsm.CreateState("Aggressive_Attacking");
        // Register onEnter, onFrame, and onExit
        aggressiveAttackingState.onEnter = delegate
        {
            print("Guard in critical health, entering Aggressive_Attacking state");
        };
        aggressiveAttackingState.onframe = delegate
        {
            print("On frame of Aggressive_Attacking state");
            // check if close enough to player and explode
            if (!CheckCriticalHealth())
            {
                fsm.TransitionTo("Attacking");
            }

            if(!CheckIfCloseEnough(player.transform.position, this.gameObject.transform.position))
            {
                fsm.TransitionTo("Patroling");
            }

            if(!CheckAttackDistance(player.transform.position, this.gameObject.transform.position))
            {
                fsm.TransitionTo("Chasing");
            }

            if (CheckCriticalHealth())
            {
                GuardExplodeAttack();
            }
        };
    }

    private void GuardExplodeAttack()
    {
        // Guard explodes themselves to cause huge damage to player
    }

    private bool CheckCriticalHealth()
    {
        return health < criticalHealth;
    }

    private bool CheckIfCloseEnough(Vector3 player, Vector3 troll)
    {
        return Vector3.Distance(player, troll) < closeEnoughDistance;
    }

    private bool CheckAttackDistance(Vector3 player, Vector3 troll)
    {
        return Vector3.Distance(player, troll) < attackDistance;
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Update();
    }
}
