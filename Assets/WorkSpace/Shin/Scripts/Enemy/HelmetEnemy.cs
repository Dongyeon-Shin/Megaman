using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HelmetEnemy : Enemy
{
    public enum State
    {
        Idle,
        Attack,
        Size
    }
    StateMachine<State, HelmetEnemy> stateMachine;
    // TODO: GameManager를 사용해 플레이어 접근
    [SerializeField]
    private Transform target;
    private Animator animator;
    private EnemyProjectile projectile;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<State, HelmetEnemy>(this);
        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Attack, new AttackState(this, stateMachine));
    }

    private void Start()
    {
        isInvincible = true;
        stateMachine.SetUp(State.Idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    protected void EndAttackAnimation()
    {
        isInvincible = !isInvincible;
        Attack();
    }
    protected void EndHideAnimation()
    {
        isInvincible = !isInvincible;
    }
    private void Attack()
    {
        projectile.transform.position = transform.position;
        
        projectile.gameObject.SetActive(true);
    }

    private abstract class HelmetEnemyState : StateBase<State, HelmetEnemy>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.GetComponent<Rigidbody2D>();
        protected SpriteRenderer renderer => owner.GetComponent<SpriteRenderer>();
        protected Animator animator => owner.animator;
        //protected Collider2D collider => owner.GetComponent<Collider>();

        protected HelmetEnemyState(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }
    }

    private class IdleState : HelmetEnemyState
    {
        private Transform target;
        private float detectRange;
        private float attackDelay;

        public IdleState(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            detectRange = owner.detectRange;
            attackDelay = 1f;
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {

        }

        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude < detectRange * detectRange)
            {               
                stateMachine.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {
            animator.SetTrigger("Attack");
        }
    }
    private class AttackState : HelmetEnemyState
    {
        private Transform target;
        private float attackRange;
        private float hideCoolTime;

        public AttackState(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            attackRange = owner.attackRange;
        }

        public override void Enter()
        {
            hideCoolTime = 0f;
        }

        public override void Update()
        {
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;
        }

        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude > attackRange * attackRange && hideCoolTime > 1f)
            {
                stateMachine.ChangeState(State.Idle);
            }
            hideCoolTime += Time.deltaTime;
        }

        public override void Exit()
        {
            animator.SetTrigger("Hide");
        }
    }
}
