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
    [Header("Random Walking Settings")]
    [SerializeField] private float rndDestionationInterval = 5f;
    [SerializeField] private float _rndDestionationRange = 10f;

    private EnemyStates _currentEnemyState;
    private NavMeshAgent _agent;
    private RaycastHit hit;
    private Vector3 _currentDestination;
    private float _agentDefaultSpeed;

    // Random Destination
    private float _timerRndDestination;

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
        _agentDefaultSpeed = _agent.speed;
    }
    
    private void Update()
    {
        // Check if we've reached the destination
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    ClownAnimator.GetInstance().SetAnimationState(ClownAnimator.AnimationStates.Idle);
                    _currentEnemyState = EnemyStates.Searching;
                }
            }
            else
            {
                ClownAnimator.GetInstance().SetAnimationState(ClownAnimator.AnimationStates.Walking);
                DynamicWalkingSpeed();
            }
        }

        if (CheckInSight())
        {
            _currentEnemyState = EnemyStates.Seeing;
            FollowPlayer(true);
        }
        else if(_currentEnemyState == EnemyStates.Searching)
        {
            // Random Searching Mode
            SetRandomDestination();

            _agent.SetDestination(_currentDestination);
        }
    }

    private void DynamicWalkingSpeed()
    {
        if (_currentEnemyState == EnemyStates.Following)
        {
            // Walking Speed control, ToDo: Only increase walking speed when player destionation is the target.
            float remainingDistance = _agent.remainingDistance - _agent.stoppingDistance;
            //Debug.Log("RemainingDistance: " + (remainingDistance);
            float aniSpeedMultiply = Mathf.InverseLerp(10, 1, remainingDistance) * 5;
            float newAgentSpeed = _agentDefaultSpeed + 0.3f * aniSpeedMultiply;

            ClownAnimator.GetInstance().SetWalkingSpeed(aniSpeedMultiply);
            _agent.speed = newAgentSpeed;
        }
        else
        {
            // Reset Speed to Defaults, because Player is not the Target.
            _agent.speed = _agentDefaultSpeed;
            ClownAnimator.GetInstance().SetWalkingSpeed(0);
        }
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
            _currentEnemyState = EnemyStates.Following;
            _currentDestination = playerCollider.position;
            _agent.SetDestination(_currentDestination);
        }
        else
        {
            // Currently not used
            _currentDestination = _agent.transform.position;
            _agent.SetDestination(_currentDestination);
        }
    }

    private void SetRandomDestination()
    {
        _timerRndDestination += Time.deltaTime;
        if (_timerRndDestination >= rndDestionationInterval)
        {
            _currentEnemyState = EnemyStates.Searching;
            _timerRndDestination = 0;
            float z = Random.Range(-_rndDestionationRange, _rndDestionationRange);
            float x = Random.Range(-_rndDestionationRange, _rndDestionationRange);

            Vector3 newRndDestination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            if (Physics.Raycast(newRndDestination, Vector3.down))
            {
                _currentDestination = newRndDestination;
            }
        }
    }

    public void SetNewDestination(Vector3 newDestination)
    {
        _currentEnemyState = EnemyStates.Following;
        _currentDestination = newDestination;
        _agent.SetDestination(_currentDestination);
    }

    public static ClownAgent GetInstance()
    {
        return instance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _rndDestionationRange);
    }
}
