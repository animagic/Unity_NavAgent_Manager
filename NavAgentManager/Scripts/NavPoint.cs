/*
 *  Only used to be able to reference the middle and end points of the AI's patrol route.
 *  Middle points have a shorter Hold timer than End Points and can be edited in GuardStateMachine.cs
 *  
 *  Author: Wayne Work
 *  Initial Date: May 16, 2016
 *  Last Edit Date: May 16, 2016
 */

using UnityEngine;
using System.Collections;

public class NavPoint : MonoBehaviour {

    [SerializeField]
    private bool isMidPoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool GetMidPointCheck()
    {
        return isMidPoint;
    }
}
