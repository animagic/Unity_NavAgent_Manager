using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GuardStateMachine : MonoBehaviour {

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
    private Transform[] guardNavPoints;

    [SerializeField]
    private float endWaitTime;
    [SerializeField]
    private float midWaitTime;
    [SerializeField]
    private float waitCounter;
    [SerializeField]
    GuardStates newState;
    [SerializeField]
    GuardStates oldState;
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
    void Start () {

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
	void Update () {

        SetPreAngryColor();
        fsm[curState].Invoke();
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "AlarmNode")
        {
            guardData.GetComponent<NavMeshAgent>().Stop();
            HandleReleaseAngry();
        }

        if (col.transform.tag == "PathNode")
        {
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
    void SetGuardAttributes()
    {
        guardNavAgent = gameObject.GetComponent<NavAgent>();
        guardNavPoints = GetComponent<NavAgent>().myNavPoints;
        guardData = GetComponent<GuardData>();
        endWaitTime = 2.5f;
        midWaitTime = 1.75f;
        angryScale = new Vector3(2.0f, 3.0f, 2.0f);
        originalScale = gameObject.GetComponent<Transform>().localScale;

    }

    void SetPreAngryColor()
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
    void HandleGoToAngry()
    {
        if (guardData.GetAngryStatus())
        {
            SetState(GuardStates.ENTERANGRY);
        }
    }

    void HandleReleaseAngry()
    {
        guardData.SetAngryStatus(false);
        SetState(GuardStates.EXITANGRY); 
    }

    void HandlePauseGaurd()
    {
        
        if (guardData.GetPauseStatus())
        {
            oldState = curState;
            GetComponent<NavMeshAgent>().Stop();
            SetState(GuardStates.PAUSE);
        }
    }

    void HandleReleasePause()
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
    void SetState(GuardStates nextState)
    {
        if (nextState != curState)
        {
            curState = nextState;
        }
        
    }
    void StatePatrol()
    {
        HandlePauseGaurd();
        HandleGoToAngry();

        
    }

    void StateIdle()
    {
        HandlePauseGaurd();
        HandleGoToAngry();

        waitCounter += Time.deltaTime;

        if (waitCounter >= endWaitTime)
        {
            guardNavAgent.GetComponent<NavMeshAgent>().Resume();
            guardNavAgent.FindDestination();
            waitCounter = 0f;
            SetState(GuardStates.PATROL);

        }
    }

    void StatePause()
    {
        HandleReleasePause();
    }

    void StateEnterAngry()
    {
        guardNavAgent.GetComponent<NavMeshAgent>().Stop();
        
        GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.red, Mathf.PingPong(Time.deltaTime, .5f));
        GetComponent<Transform>().localScale = Vector3.Lerp(GetComponent<Transform>().localScale, angryScale, Mathf.PingPong(Time.deltaTime, .5f));

        

        if (GetComponent<Renderer>().material.color == Color.red)
        {
            SetState(GuardStates.ANGRY);
        }
    }

    void StateAngry()
    {
        HandlePauseGaurd();

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

    void StateExitAngry()
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
