using UnityEngine;
using System.Collections;

public class AlarmNode : MonoBehaviour {

    private GuardData guardData;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<GuardData>() != null)
        {
            guardData = col.GetComponent<GuardData>();
            guardData.SetAngryStatus(false);
        }
    }*/
}
