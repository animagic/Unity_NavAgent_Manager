/*
 *  Unit specific data and variables
 *
 *  Author: Wayne Work
 *  Initial Date: May 16, 2016
 *  Last Edit Date: May 16, 2016
 */

using UnityEngine;

public class GuardData : MonoBehaviour
{
    [SerializeField]
    private bool movementPaused;

    [SerializeField]
    private bool isAngry;

    private void Awake()
    {
        movementPaused = false;
        isAngry = false;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public bool GetPauseStatus()
    {
        return movementPaused;
    }

    public void SetPauseStatus(bool newPause)
    {
        movementPaused = newPause;
    }

    public bool GetAngryStatus()
    {
        return isAngry;
    }

    public void SetAngryStatus(bool newAngry)
    {
        isAngry = newAngry;
    }
}