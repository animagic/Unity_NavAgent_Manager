using UnityEngine;
using System.Collections;

public class GuardData : MonoBehaviour {

    [SerializeField]
    private bool movementPaused;
    [SerializeField]
    private bool isAngry;


    void Awake()
    {
        movementPaused = false;
        isAngry = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
