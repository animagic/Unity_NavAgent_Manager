/*
 *  State Machine for controlling the enemy AI's patrol states.
 *
 *  Author: Wayne Work
 *  Initial Date: May 16, 2016
 *  Last Edit Date: May 16, 2016
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateMachine : MonoBehaviour
{
    private Dictionary<GuardStates, Action> fsm = new Dictionary<GuardStates, Action>();

    private enum GuardStates
    {
        PATROL,
        IDLE,
        PAUSE,
        ENTERANGRY,
        ANGRY,
        EXITANGRY,

        NUM_STATES
    }

    private NavAgent guardNavAgent;
    private GuardData guardData;

    [SerializeField]
    private float endWaitTime;

    [SerializeField]
    private float midWaitTime;

    [SerializeField]
    private float waitCounter;

    [SerializeField]
    private GuardStates newState;

    [SerializeField]
    private GuardStates oldState;

    [SerializeField]
    private Color preAngryColor;

    [SerializeField]
    private Transform alarmNode;

    [SerializeField]
    private Vector3 angryScale;

    [SerializeField]
    private Vector3 originalScale;

    [SerializeField]
    private GuardStates curState = GuardStates.PATROL;

    //  -------------------------------------------------------------------------------------------
    //  Unity Standard Functions Start
    //
    #region

    // Use this for initialization
    private void Start()
    {
        SetGuardAttributes();

        fsm.Add(GuardStates.PATROL, new Action(StatePatrol));
        fsm.Add(GuardStates.IDLE, new Action(StateIdle));
        fsm.Add(GuardStates.PAUSE, new Action(StatePause));
        fsm.Add(GuardStates.ENTERANGRY, new Action(StateEnterAngry));
        fsm.Add(GuardStates.ANGRY, new Action(StateAngry));
        fsm.Add(GuardStates.EXITANGRY, new Action(StateExitAngry));

        SetState(GuardStates.PATROL);
    }

    // Update is called once per frame
    private void Update()
    {
        SetPreAngryColor();
        fsm[curState].Invoke();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.name == "AlarmNode")
        {
            // Stop the unit from moving so that it stays in one spot to return to normal size/color
            guardData.GetComponent<NavMeshAgent>().Stop();
            HandleReleaseAngry();
        }

        if (col.transform.tag == "PathNode")
        {
            // Stop the unit at each patrol point
            guardData.GetComponent<NavMeshAgent>().Stop();
            SetState(GuardStates.IDLE);
        }
    }

    #endregion
    //
    //  Unity Standard Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  Helper Functions Start
    //
    #region

    // Sets the units Starting attributes
    private void SetGuardAttributes()
    {
        guardNavAgent = gameObject.GetComponent<NavAgent>();

        guardData = GetComponent<GuardData>();
        endWaitTime = 2.5f;
        midWaitTime = 1.75f;
        angryScale = new Vector3(2.0f, 3.0f, 2.0f);
        originalScale = gameObject.GetComponent<Transform>().localScale;
    }

    // Used in Update() to set the original color of the unit so that it returns to that color once it has returned from Angry
    private void SetPreAngryColor()
    {
        if (GetComponent<Renderer>().material.color == Color.white)
        {
            preAngryColor = Color.white;
        }
        else if (GetComponent<Renderer>().material.color == Color.blue)
        {
            preAngryColor = Color.blue;
        }
    }

    private void HandleGoToAngry()
    {
        if (guardData.GetAngryStatus())
        {
            SetState(GuardStates.ENTERANGRY);
        }
    }

    private void HandleReleaseAngry()
    {
        guardData.SetAngryStatus(false);
        SetState(GuardStates.EXITANGRY);
    }

    // Sets the unit's oldState to its currentState then stops the unit altogether, so that it can return to its previous state once started again
    private void HandlePauseGaurd()
    {
        if (guardData.GetPauseStatus())
        {
            oldState = curState;
            GetComponent<NavMeshAgent>().Stop();
            SetState(GuardStates.PAUSE);
        }
    }

    private void HandleReleasePause()
    {
        if (!guardData.GetPauseStatus())
        {
            GetComponent<NavMeshAgent>().Resume();
            SetState(oldState);
        }
    }

    #endregion
    //
    //  Helper Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  State Functions Start
    //
    #region

    private void SetState(GuardStates nextState)
    {
        if (nextState != curState)
        {
            curState = nextState;
        }
    }

    // State that the unit is in when it is moving between patrol points
    private void StatePatrol()
    {
        HandlePauseGaurd();
        HandleGoToAngry();
    }

    // State that the unit is in when it stops at patrol point
    private void StateIdle()
    {
        HandlePauseGaurd();
        HandleGoToAngry();

        waitCounter += Time.deltaTime;

        // Moves the unit back to patrolling after the designated time is up
        if (waitCounter >= endWaitTime)
        {
            guardNavAgent.GetComponent<NavMeshAgent>().Resume();
            guardNavAgent.FindDestination();
            waitCounter = 0f;
            SetState(GuardStates.PATROL);
        }
    }

    // State used to help debug a unit's patrol path and behaviour.  Pauses the unit where it is
    private void StatePause()
    {
        HandleReleasePause();
    }

    // State that controls the unit turning Angry
    // Is not effected by any other control commands so as not to interrupt the transformation
    private void StateEnterAngry()
    {
        guardNavAgent.GetComponent<NavMeshAgent>().Stop();

        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.red, Mathf.PingPong(Time.deltaTime, .5f));
        GetComponent<Transform>().localScale = Vector3.Lerp(GetComponent<Transform>().localScale, angryScale, Mathf.PingPong(Time.deltaTime, .5f));

        if (GetComponent<Renderer>().material.color == Color.red)
        {
            SetState(GuardStates.ANGRY);
        }
    }

    // State the unit is in after the Angry transformation has finished
    // Allows for changing to other states now that the transformation is complete
    private void StateAngry()
    {
        HandlePauseGaurd();

        // Sets the unit's State to EXITANGRY if the Angry boolean is set to false before it gets to the Alarm Node
        // Moves the unit to the Alarm Node if the Angry boolean is true
        if (!guardData.GetAngryStatus())
        {
            SetState(GuardStates.EXITANGRY);
        }
        else
        {
            guardNavAgent.GetComponent<NavMeshAgent>().Resume();
            guardNavAgent.GetComponent<NavMeshAgent>().SetDestination(alarmNode.position);
        }

        // See OnTriggerEnter for transition to StateExitAngry
    }

    // Exits the Angry state and is uninterruptable until the unit moves to the next state
    private void StateExitAngry()
    {
        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, preAngryColor, Mathf.PingPong(Time.deltaTime, 1));
        GetComponent<Transform>().localScale = Vector3.Lerp(GetComponent<Transform>().localScale, originalScale, Mathf.PingPong(Time.deltaTime, 1));

        if (GetComponent<Transform>().localScale == originalScale)
        {
            guardData.GetComponent<NavMeshAgent>().Resume();
            guardNavAgent.FindDestination();
            SetState(GuardStates.PATROL);
        }
    }

    #endregion
    //
    //  State Functions End
    //  -------------------------------------------------------------------------------------------
}