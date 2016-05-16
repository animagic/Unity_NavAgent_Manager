/*
 *  NavAgent script for controlling the enemy AI's movement behaviour.
 *
 *  Author: Wayne Work
 *  Initial Date: May 16, 2016
 *  Last Edit Date: May 16, 2016
 */

using UnityEngine;

public class NavAgent : MonoBehaviour
{
    private NavMeshAgent myNavAgent;

    public Transform[] myNavPoints;

    // Index value of the myNavPoints array
    private int navIndex;

    // Used to ensure that the guard does not get stuck at a node
    // Especially useful for when the array gets Reversed by the Debug Manager
    private float stayTimer;

    private float maxStayTimer;

    [SerializeField]
    private bool hasNavAgent;

    // Use this for initialization
    private void Start()
    {
        maxStayTimer = 2f;
        navIndex = 0;

        CheckNavAgent();
        FindDestination();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter()
    {
        GetNextWaypoint();
    }

    // Used to move the unit if it gets stuck at a nav point
    private void OnTriggerStay()
    {
        stayTimer += Time.deltaTime;

        if (stayTimer >= maxStayTimer)
        {
            GetNextWaypoint();
            FindDestination();

            stayTimer = 0f;
        }
    }

    private void OnTriggerExit()
    {
        stayTimer = 0f;
    }

    // Locates the next position in the unit's myNavPoints array and moves it to that location
    public void FindDestination()
    {
        Vector3 newTravelPosition = myNavPoints[navIndex].transform.position;

        myNavAgent.SetDestination(newTravelPosition);
    }

    // Iterates through the myNavPoints array so the unit doesn't stop moving when it reaches a nav point
    private void GetNextWaypoint()
    {
        ++navIndex;

        if (navIndex >= myNavPoints.Length)
        {
            navIndex = 0;
        }
    }

    // Ensures that the unit has a NavAgent attached to it in case the user removes it
    private void CheckNavAgent()
    {
        if (gameObject.GetComponent<NavMeshAgent>() == null)
        {
            gameObject.AddComponent<NavMeshAgent>();
        }

        myNavAgent = GetComponent("NavMeshAgent") as NavMeshAgent;
    }

    // Returns the current index of the myNavPoints array
    public int GetNavIndex()
    {
        return navIndex;
    }
}