using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClownAgent : MonoBehaviour
{
    [SerializeField] private Transform player;

    private NavMeshAgent _agent;
    [SerializeField] private float maxRange = 5;
    private RaycastHit hit;

    public enum EnemyStates
    {
        Searching = 0,
        Seeing = 1,
        Following = 2,
        Reached = 3,
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        FollowPlayer(CheckInSight());
    }

    private bool CheckInSight()
    {
        if (Vector3.Distance(transform.position, player.position) < maxRange)
        {
            if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, (player.position - transform.position), Color.green);
                    return true;
                }
                else
                {
                    //Debug.Log("Hit of: " + hit.transform.tag);
                    Debug.DrawRay(transform.position, (player.position - transform.position), Color.red);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void FollowPlayer(bool follow)
    {
        if (follow)
        {
            //Debug.Log("Player in sight, following");
            _agent.SetDestination(player.position);
        }
        else
        {
            _agent.SetDestination(_agent.transform.position);
        }
    }
}
