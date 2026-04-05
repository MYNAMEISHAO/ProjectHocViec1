using System;
using UnityEngine;

public class Enemy : Character
{

    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    private IState currentState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;

    [SerializeField] private GameObject AttackArea;
    [SerializeField] private GameObject SightArea;
    private void Update()
    {
        if(currentState != null && !isDeath)
        {
            currentState.OnExecute(this);
        }
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeactiveAttackArea();
        ActiveSightArea();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        Debug.Log("Enemy died");
        DeactiveSightArea();
        base.OnDeath();
    }


    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if(currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {
        ChangeAnim("Run");
        rb.linearVelocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("Idle");
        rb.linearVelocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("Attack");
        ActiveAttackArea();
        Invoke(nameof(DeactiveAttackArea), 0.5f);
    }

    public void Throw()
    {

    }
    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }

    internal void SetTarget(Character character)
    {
        this.target = character;
        if(IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else
        {
            if(Target != null)
            {
                ChangeState(new PatrolState());
            }
             else
            {
                ChangeState(new IdleState());
            }
        }   
    }
    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ActiveAttackArea()
    {
        AttackArea.SetActive(true);
    }

    private void DeactiveAttackArea()
    {
        AttackArea.SetActive(false);
    }

    private void ActiveSightArea()
    {
        SightArea.SetActive(true);
    }
    private void DeactiveSightArea()
    {
        SightArea.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWall"))
        {
            ChangeDirection(!isRight);
        }
    }

    
}
