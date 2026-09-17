using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class StateMachine : MonoBehaviour
{
    public enum EnumState { Idle, Wander, Fall, Jump, Attack}
    [SerializeField] private EnumState currentState = EnumState.Idle;

    [Header("IANavMesh")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float rayonMin = 20.0f;
    [SerializeField] private float rayonMax = 50.0f;

    [Header("Wander")]
    private bool _arrivedToHisPoint;
    
    [Header("Idle")] 
    private bool _wantToWander;
    private bool _wantAttack;
    
    [Header("Attack")] 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject warningInfo;
    [SerializeField] private EnemyAttack enemyAttack;
    private bool _finishedItsAttack = true;
    private bool _closeToThePlayer;

    [Header("Jump")] 
    [SerializeField] private float jumpForce = 5.0f;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private float gravityValue = -9.81f;
    
    [Header("Falling")]
    private bool _isGrounded;
    private bool _isFalling;


    private void Start()
    {
        ChangeState(EnumState.Idle);
    }

    public void ChangeState(EnumState newState)
    {
        currentState = newState;
        EnterState();
    }

    private void Update()
    {
        if (GameManager.instance.isInPostRound)
        {
            if ( currentState != EnumState.Idle)
                currentState = EnumState.Idle;
            return;
        }
        
        switch (currentState)
        {
            case EnumState.Idle:
                if ( _wantAttack )
                {
                    ChangeState(EnumState.Attack);
                    break;
                }
                if ( _wantToWander )
                {
                    ChangeState(EnumState.Wander);
                    break;
                }
                break;
            
            
            case EnumState.Wander:
                if ( _wantAttack )
                {
                    ChangeState(EnumState.Attack);
                    break;
                }
                if ( _arrivedToHisPoint )
                {
                    ChangeState(EnumState.Idle);
                    break;
                }
                break;
            
            
            case EnumState.Fall:
                if ( _isGrounded )
                {
                    ChangeState(EnumState.Idle);
                    break;
                }
                break;
            
            
            case EnumState.Jump:
                if ( _isFalling )
                {
                    ChangeState(EnumState.Fall);
                    break;
                }
                if ( _isGrounded )
                {
                    ChangeState(EnumState.Idle);
                    break;
                }
                break;
            
            
            case EnumState.Attack:
                if ( _finishedItsAttack )
                {
                    ChangeState(EnumState.Idle);
                    break;
                }
                break;
        }
        
        ExecuteState();
    }

    void ExecuteState()
    {
        switch (currentState)
        {
            case EnumState.Idle:
                Idle();
                break;
            case EnumState.Wander:
                Wander();
                break;
            case EnumState.Fall:
                Fall();
                break;
            case EnumState.Jump:
                Jump();
                break;
            case EnumState.Attack:
                Attack();
                break;
        }
    }

    void EnterState()
    {
        switch (currentState)
        {
            case EnumState.Idle:
                EnterIdle();
                break;
            case EnumState.Wander:
                EnterWander();
                break;
            case EnumState.Fall:
                EnterFall();
                break;
            case EnumState.Jump:
                EnterJump();
                break;
            case EnumState.Attack:
                EnterAttack();
                break;
        }
    }

    
    // Idle
    void EnterIdle()
    {
        _wantAttack = false;
        _wantToWander = false;
        
        int randomChoice =  Random.Range(0, 4);
        if (randomChoice == 3)
            _wantAttack = true;
        else
            _wantToWander = true;
    }
    
    void Idle()
    {
        
    }
    
    // Falling
    void EnterFall()
    {
        
    }
    
    void Fall()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    
    // Jump
    void EnterJump()
    {
        _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravityValue);
        _isFalling = false;
    }
    
    void Jump()
    {
        _velocity.y += gravityValue * Time.deltaTime;
        agent.Move(_velocity * Time.deltaTime);
        if (_velocity.y <= 0.0f)
            _isFalling = true;
    }
    
    
    // Attack
    void EnterAttack()
    {
        agent.SetDestination(playerTransform.position);
        _finishedItsAttack = false;
        _closeToThePlayer = false;
        warningInfo.SetActive(true);
    }
    
    void Attack()
    {
        warningInfo.transform.LookAt(playerTransform);
        if (agent.remainingDistance <= agent.stoppingDistance + 2.0f)
        {
            _closeToThePlayer = true;
            
            enemyAttack.enabled = true;
            enemyAttack.Attack();
            _finishedItsAttack = true;
            warningInfo.SetActive(false);
        }
        if ( !_closeToThePlayer )
            agent.SetDestination(playerTransform.position);
    }
    
    // Wander
    void EnterWander()
    {
        GoToARandomPoint();
        _wantAttack = false;
        _arrivedToHisPoint = false;
    }
    
    void Wander()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            _arrivedToHisPoint = true;
        }
    }
    
    void GoToARandomPoint()
    {
        Vector3 randomPoint = GetARandomPointOnNavMesh(transform.position, rayonMin, rayonMax);
        agent.SetDestination(randomPoint);
    }

    Vector3 GetARandomPointOnNavMesh(Vector3 origin, float rayonMin, float rayonMax)
    {
        Vector3 direction = Random.insideUnitSphere.normalized;

        float distance = Random.Range(rayonMin, rayonMax);

        Vector3 point = origin + direction * distance;

        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, rayonMax, NavMesh.AllAreas);

        return hit.position;
    }
}
