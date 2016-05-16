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

    private void ReverseGuardNav()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Transform[] guardNavArray = guards[activeGuard].GetComponent<NavAgent>().myNavPoints;
            Array.Reverse(guardNavArray);
        }
    }

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