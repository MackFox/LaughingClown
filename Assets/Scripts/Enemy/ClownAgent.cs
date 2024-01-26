using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClownAgent : MonoBehaviour
{
    private static ClownAgent instance;
    [SerializeField] private Transform playerCollider;
    [SerializeField] private float maxRange = 5;
    [SerializeField] private LayerMask _ignoredLayer;

    private NavMeshAgent _agent;
    private RaycastHit hit;

    public enum EnemyStates
    {
        Searching = 0,
        Seeing = 1,
        Following = 2,
        Reached = 3,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
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
        if (Vector3.Distance(transform.position, playerCollider.position) < maxRange)
        {
            if (Physics.Raycast(transform.position, (playerCollider.position - transform.position), out hit, maxRange, ~_ignoredLayer))
            {
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawRay(transform.position, (playerCollider.position - transform.position), Color.green);
                    return true;
                }
                else
                {
                    //Debug.Log("Hit of: " + hit.transform.tag);
                    Debug.DrawRay(transform.position, (playerCollider.position - transform.position), Color.red);
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
            _agent.SetDestination(playerCollider.position);
        }
        else
        {
            _agent.SetDestination(_agent.transform.position);
        }
    }

    public ClownAgent GetInstance()
    {
        return instance;
    }
}
