using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Simple FSM example = enum + switch

public class GuardFSMScript : MonoBehaviour
{
    // States
    public enum GuardState { patrol, attack, runaway, heal}; // Enum
    GuardState guardState;

    GameObject enemy;
    public float enemyStrength = 75;
    public float selfStrength = 100;
    public float selfHealth = 100;
    public float selfRegenerationDelay = 1f;
    public float selfRegenerationTimer = 0f;
    public float safetyDistance = 5;
    public Color safeColor = Color.green;
    public Color threatenedColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        guardState = GuardState.patrol;
        enemy = GameObject.FindGameObjectWithTag("Player");
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
            case GuardState.patrol:
                UpdatePatrolState();
                break;
            case GuardState.attack:
                UpdateAttackState();
                break;
            case GuardState.runaway:
                UpdateRunawayState();
                break;
            case GuardState.heal:
                UpdateHealState();
                break;
            default:
                //print("Invalid GuardState: " + guardState);
                print($"Invalid GuardState: {guardState}");
                break;
        }
    }

    private void UpdateHealState()
    {
        print("In heal state");
        if (Injuried())
        {
            selfRegenerationTimer += Time.deltaTime;
            if (selfRegenerationTimer > selfRegenerationDelay)
            {
                selfHealth += 1;
                selfRegenerationTimer = 0;
            }
        }
        else
        {
            ChangeState(GuardState.patrol);
        }
    }

    private void UpdateRunawayState()
    {
        print("In runaway state");
        if(Safe())
        {
            ChangeState(GuardState.patrol);
        }
    }

    private bool Safe()
    {
        Vector3 guard = this.transform.position;
        Vector3 Enemy = enemy.transform.position;
        bool safe = Vector3.Distance(guard, Enemy) > safetyDistance;
        if(safe)
        {
            GetComponent<Renderer>().sharedMaterial.color = safeColor;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial.color = threatenedColor;
        }
        return safe;
    }

    private void UpdateAttackState()
    {
        print("In attack state");
        if(!StrongerThanEnemy())
        {
            ChangeState(GuardState.runaway);
        }
        // Return to patrolling when the enemy is out of range
        if(Safe())
        {
            ChangeState(GuardState.patrol);
        }
    }

    private void UpdatePatrolState()
    {
        print("In patrol state");
        if (Threatened())
        {
            if (StrongerThanEnemy())
            {
                ChangeState(GuardState.attack);
            }
            else
            {
                ChangeState(GuardState.runaway);
            }
        }
        else if(Injuried())
        {
            ChangeState(GuardState.heal);
        }
        else
        {
            ChangeState(GuardState.patrol);
        }
    }

    private void ChangeState(GuardState newState)
    {
        guardState = newState;
    }

    private bool StrongerThanEnemy()
    {
        return selfStrength > enemyStrength;
    }

    private bool Threatened()
    {
        return !Safe();
    }

    private bool Injuried()
    {
        return selfHealth < 100;
    }
}
