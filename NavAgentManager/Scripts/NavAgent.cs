using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour {

    private NavMeshAgent myNavAgent;
    private int navIndex;

    // Used to ensure that the guard does not get stuck at a node
    // Especially useful for when the array gets Reversed
    private float stayTimer;
    private float maxStayTimer;   

    [SerializeField]
    private bool hasNavAgent;
    public Transform[] myNavPoints;

    // Use this for initialization
    void Start () {

        maxStayTimer = 2f;
        navIndex = 0;

        CheckNavAgent();
        FindDestination();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        GetNextWaypoint();   
    }

    void OnTriggerStay()
    {
        stayTimer += Time.deltaTime;

        if (stayTimer >= maxStayTimer)
        {
            GetNextWaypoint();
            FindDestination();

            stayTimer = 0f;
        }
    }

    void OnTriggerExit()
    {
        stayTimer = 0f;
    }

    public void FindDestination()
    {
        Vector3 newTravelPosition = myNavPoints[navIndex].transform.position;

        myNavAgent.SetDestination(newTravelPosition);
    }

    void GetNextWaypoint()
    {
        ++navIndex;

        if (navIndex >= myNavPoints.Length)
        {
            navIndex = 0;
        }
    }

    void CheckNavAgent()
    {
        if (gameObject.GetComponent<NavMeshAgent>() == null)
        {
            gameObject.AddComponent<NavMeshAgent>();
        }

        myNavAgent = GetComponent("NavMeshAgent") as NavMeshAgent;
    }

    public int GetNavIndex()
    {
        return navIndex;
    }
}
