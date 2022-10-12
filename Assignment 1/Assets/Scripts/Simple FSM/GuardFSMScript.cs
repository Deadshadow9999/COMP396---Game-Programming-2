using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Simple FSM example = enum + switch

public class GuardFSMScript : MonoBehaviour
{
    // States
    public enum GuardState { patroling, chasing, attacking, aggressiveAttacking}; // Enum
    GuardState guardState;

    public GameObject player;

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
        guardState = GuardState.patroling;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch(guardState) // switch
        {
            case GuardState.patroling:
                UpdatePatrolingState();
                break;
            case GuardState.chasing:
                UpdateChasingState();
                break;
            case GuardState.attacking:
                UpdateAttackingState();
                break;
            case GuardState.aggressiveAttacking:
                UpdateAggressiveAttackingState();
                break;
            default:
                //print("Invalid GuardState: " + guardState);
                print($"Invalid GuardState: {guardState}");
                break;
        }
    }

    private void UpdatePatrolingState()
    {
        print("In patroling state");
        this.gameObject.GetComponent<Renderer>().sharedMaterial.color = patrolingColor;

        if(CheckIfCloseEnough(player.transform.position, this.gameObject.transform.position))
        {
            ChangeState(GuardState.chasing);
        }
    }

    private void UpdateChasingState()
    {
        print("In chasing state");
        this.gameObject.GetComponent<Renderer>().sharedMaterial.color = chasingColor;

        if (!CheckIfCloseEnough(player.transform.position, this.transform.position))
        {
            ChangeState(GuardState.patroling);
        }
        if(CheckAttackDistance(player.transform.position, this.gameObject.transform.position))
        {
            ChangeState(GuardState.attacking);
        }
    }

    private void UpdateAttackingState()
    {
        print("In Attacking state");
        this.gameObject.GetComponent<Renderer>().sharedMaterial.color = attackingColor;
        if (!CheckAttackDistance(player.transform.position, this.gameObject.transform.position))
        {
            ChangeState(GuardState.chasing);
        }
        if(CriticalHealth())
        {
            ChangeState(GuardState.aggressiveAttacking);
        }
    }

    private void UpdateAggressiveAttackingState()
    {
        print("In Aggressive Attacking state");

        if (!CheckIfCloseEnough(player.transform.position, this.transform.position))
        {
            ChangeState(GuardState.patroling);
        }
        if (!CheckAttackDistance(player.transform.position, this.transform.position))
        {
            ChangeState(GuardState.chasing);
        }
        if(!CriticalHealth())
        {
            ChangeState(GuardState.attacking);
        }

        GuardExplodeAttack();
    }

    private void GuardExplodeAttack()
    {
        // Guard explodes themselves to cause huge damage to player
        print("Guard used explode attack");
    }

    private void ChangeState(GuardState newState)
    {
        guardState = newState;
    }

    private bool CriticalHealth()
    {
        return health < criticalHealth;
    }

    private bool CheckIfCloseEnough(Vector3 player, Vector3 guard)
    {
        return Vector3.Distance(player, guard) < closeEnoughDistance;
    }
    private bool CheckAttackDistance(Vector3 player, Vector3 guard)
    {
        return Vector3.Distance(player, guard) < attackDistance;
    }
}
