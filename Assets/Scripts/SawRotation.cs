using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        
        if (transform.position == waypoints[currentWaypointIndex].position) //check current way point
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;// change to next way point
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);
        //Vector3.MoveTowards(current, target, maxDistanceDelta)
    }
}
