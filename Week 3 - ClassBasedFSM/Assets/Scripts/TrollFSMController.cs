using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollFSMController : MonoBehaviour
{
    public GameObject player;
    public FSMSystemTroll fsm;
    public Transform[] path;

    // Start is called before the first frame update
    void Start()
    {
        MakeFSM();
    }

    private void MakeFSM()
    {
        //throw new NotImplementedException();
        FollowPathState follow = new FollowPathState(path);
        follow.AddTransition(Transition.SawPlayer, StateID.ChasingPlayer);

        ChasingPlayerState chase = new ChasingPlayerState();
        chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);

        fsm = new FSMSystemTroll();
        fsm.AddState(follow);
        fsm.AddState(chase);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fsm.CurrentState.Reason(player, this.gameObject);
        fsm.CurrentState.Act(player, this.gameObject);
    }
}

internal class ChasingPlayerState : FSMState
{
    public override void Act(GameObject player, GameObject npc)
    {
        throw new NotImplementedException();
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        throw new NotImplementedException();
    }
}

internal class FollowPathState : FSMState
{
    private int currentWaypoint;
    private Transform[] path;

    public FollowPathState(Transform[] path)
    {
        this.path = path;
        currentWaypoint = 0;
        stateID = StateID.FollowingPath;
    }

    public override void Act(GameObject player, GameObject npc)
    {
        throw new NotImplementedException();
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        //throw new NotImplementedException();

    }
}