using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    Transform target;      //target to follow
    NavMeshAgent agent;    //Reference to our agent



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        //   **********put this in a coroutine so it only updates a few times per second************
        if(target != null)
        {
            agent.SetDestination(target.position);

            //update rotation ourselves.
            FaceTarget();
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;

        //we want to handle rotation ourself when we are following a target because
        //if we're withing the target's radius and the target moves slighty,
        //but still with the player within it's radius,
        //then the player's rotation will not update.
        //in order to fix this, we'll handle rotation ourselves when focused
        //first we must stop updating rotation via the agent
        agent.updateRotation = false;


        target = newTarget.interactionTransform;


    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0;

        //once we stop following a target, we can update rotation via agent again
        agent.updateRotation = true;

        target = null;
    }

    void FaceTarget()
    {
        //get a direction towards our target
        Vector3 direction = (target.position - transform.position).normalized;

        //find out how to rotate in order to look in the direction, ignoring y values
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        //smoothly interpolate towards that rotation. the hardcoded 5 is the speed
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
