/*
 *  Debug Manager to test unit behaviour in different states and settings
 *
 *  Author: Wayne Work
 *  Initial Date: May 16, 2016
 *  Last Edit Date: May 16, 2016
 */

using System;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private GuardData[] guards;

    [SerializeField]
    private int activeGuard;

    // Use this for initialization
    private void Start()
    {
        FindAllGuards();

        //sets the actively selected guard to the 0 index of the guard array and sets its color to blue
        activeGuard = 0;
        guards[activeGuard].GetComponent<Renderer>().material.color = Color.blue;
    }

    // Update is called once per frame
    private void Update()
    {
        CycleGuards();
        PauseGuard();
        ReverseGuardNav();
        MakeGuardAngry();
    }

    private void FindAllGuards()
    {
        guards = GameObject.FindObjectsOfType<GuardData>();
    }

    // Cycles the actively selected guard
    private void CycleGuards()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            guards[activeGuard].GetComponent<Renderer>().material.color = Color.white;

            if (activeGuard == 0)
            {
                activeGuard = guards.Length - 1;
            }
            else
            {
                activeGuard--;
            }
            guards[activeGuard].GetComponent<Renderer>().material.color = Color.blue;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            guards[activeGuard].GetComponent<Renderer>().material.color = Color.white;

            activeGuard++;

            if (activeGuard >= guards.Length)
            {
                activeGuard = 0;
            }

            guards[activeGuard].GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    // Stops the guard where it is
    // Will not interrupt the Angry transformtion, to or from, but will stop the guard from moving once the transformation is complete
    private void PauseGuard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!guards[activeGuard].GetComponent<GuardData>().GetPauseStatus())
            {
                guards[activeGuard].GetComponent<GuardData>().SetPauseStatus(true);
            }
            else if (guards[activeGuard].GetComponent<GuardData>().GetPauseStatus())
            {
                guards[activeGuard].GetComponent<GuardData>().SetPauseStatus(false);
            }
        }
    }

    // Reverses the guard's nav point array so it follows the route backwards
    private void ReverseGuardNav()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Transform[] guardNavArray = guards[activeGuard].GetComponent<NavAgent>().myNavPoints;
            Array.Reverse(guardNavArray);
        }
    }

    // Forces the guard into the Angry state
    private void MakeGuardAngry()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!guards[activeGuard].GetComponent<GuardData>().GetAngryStatus())
            {
                guards[activeGuard].GetComponent<GuardData>().SetAngryStatus(true);
            }
            else if (guards[activeGuard].GetComponent<GuardData>().GetAngryStatus())
            {
                guards[activeGuard].GetComponent<GuardData>().SetAngryStatus(false);
            }
        }
    }
}