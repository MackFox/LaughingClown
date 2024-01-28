using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClownAgent : MonoBehaviour
{
    private static ClownAgent instance;
    [SerializeField] private Transform playerCollider;
    [SerializeField] private float maxRange = 5;
    [SerializeField, Range(0.01f, 0.5f)] float followSpeedAdditive = 0.3f;
    [SerializeField] private LayerMask _ignoredLayer;
    [SerializeField] private LayerMask _groundLayers;
    [Header("Seeing Settings")]
    [SerializeField] private float _seeingWaitTime = 3f;
    private float _seeingTimer;
    [Header("Random Walking Settings")]
    [SerializeField] private float rndDestionationInterval = 5f;
    [SerializeField] private float _rndDestionationRange = 10f;

    [Header("Death")]
    [SerializeField, Range(0,1)] private float _deathAniDelay = 0.3f;

    private EnemyStates _currentEnemyState;
    private EnemyStates _lastEnemyState;
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
        Death = 4,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _currentDestination = transform.position;
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agentDefaultSpeed = _agent.speed;
    }
    
    private void Update()
    {
        if (_lastEnemyState != _currentEnemyState)
        {
            _lastEnemyState = _currentEnemyState;
            ClownSoundManager.GetInstance().PlayLaugh(_currentEnemyState);
        }

        if (_currentEnemyState == EnemyStates.Death)
        {
            _agent.SetDestination(_currentDestination);
            return;
        }

        // Check if we've reached the destination
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    ClownAnimator.GetInstance().SetAnimationState(ClownAnimator.AnimationStates.Idle);

                    if (_currentEnemyState != EnemyStates.Seeing)
                        _currentEnemyState = EnemyStates.Searching;
                }
            }
            else
            {
                if (_currentEnemyState == EnemyStates.Following)
                    ClownAnimator.GetInstance().SetAnimationState(ClownAnimator.AnimationStates.Running);
                else
                    ClownAnimator.GetInstance().SetAnimationState(ClownAnimator.AnimationStates.Walking);

                
                DynamicWalkingSpeed();
            }
        }

        if (CheckInSight())
        {
            if (_currentEnemyState == EnemyStates.Following)
            {
                FollowPlayer(true);
                return;
            }
            _currentEnemyState = EnemyStates.Seeing;
            ClownAnimator.GetInstance().SetAnimationState(ClownAnimator.AnimationStates.Watching);
            //ClownSoundManager.GetInstance().PlayLaugh(_currentEnemyState);

            // ToDo:
            // Add Delay before follow
            // move instant when player leaves sight, or player sees enemy.
            // longes delay is when u cant see him and he sees you.

            _seeingTimer += Time.deltaTime;
            _currentDestination = transform.position;
            _agent.SetDestination(_currentDestination);

            if (_seeingTimer >= _seeingWaitTime)
            {
                FollowPlayer(true);
            }
        }
        else if (_currentEnemyState == EnemyStates.Searching)
        {
            // Random Searching Mode
            SetRandomDestination();
            if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                _timerRndDestination = rndDestionationInterval;
                SetRandomDestination();
            }
            else
                _agent.SetDestination(_currentDestination);
        }
        else if (_currentEnemyState == EnemyStates.Seeing)
        {
            // Enemy has seen Player, but lost sight befor wait time was over, move instant!
            FollowPlayer(true);
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
            float newAgentSpeed = _agentDefaultSpeed + followSpeedAdditive * aniSpeedMultiply;

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
        _seeingTimer = 0;
        if (follow)
        {
            _currentEnemyState = EnemyStates.Following;
            //ClownSoundManager.GetInstance().PlayLaugh(_currentEnemyState);
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
            float z = Random.Range(-_rndDestionationRange, _rndDestionationRange);
            float x = Random.Range(-_rndDestionationRange, _rndDestionationRange);

            Vector3 newRndDestination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            RaycastHit rndHit;
            if (Physics.Raycast(newRndDestination, Vector3.down, out rndHit))
            {
                Debug.Log("RayHit on Layer: " + rndHit.collider.gameObject.layer);
                NavMeshHit hit;
                if (!NavMesh.SamplePosition(newRndDestination, out hit, 10f, NavMesh.AllAreas))
                {
                    Debug.Log("Path not reachable!");
                    SetRandomDestination();
                }
                else
                {
                    _currentDestination = newRndDestination;
                    _timerRndDestination = 0;
                    _agent.SetDestination(_currentDestination);
                    Debug.Log("Path is Reachable!");
                }
            }
        }
    }

    public void SetNewDestination(Vector3 newDestination)
    {
        _currentEnemyState = EnemyStates.Following;
        _currentDestination = newDestination;
        _agent.SetDestination(_currentDestination);
    }

    public void MoveToDeathDestination(Vector3 deathDestination)
    {
        _currentEnemyState = EnemyStates.Death;
        _currentDestination = deathDestination;
        _agent.SetDestination(_currentDestination);
        StartCoroutine(PlayDeathAnimation());
    }

    // Maybe move to CharacterAnimator
    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSeconds(_deathAniDelay);
        transform.GetComponent<Transform>().localScale = new Vector3(2f, 0.1f, 1.35f);
    }

    public void KillPlayer()
    {
        _currentEnemyState = EnemyStates.Reached;
    }

    public static ClownAgent GetInstance()
    {
        return instance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _rndDestionationRange);
        Gizmos.DrawWireSphere(transform.position, maxRange);
    }
}
