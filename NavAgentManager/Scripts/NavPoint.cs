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
