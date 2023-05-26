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
        Alert,
        Aggressive,
        Size
    }
    StateMachine<State, HelmetEnemy> stateMachine;
    // TODO: GameManager를 사용해 플레이어 접근
    [SerializeField]
    private Transform target;
    private Animator animator;
    private EnemyProjectile projectile;
    [SerializeField]
    protected LayerMask playerMask;
    protected Vector2 targetDirection;
    protected bool readyToChangeState;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<State, HelmetEnemy>(this);
        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Alert, new AlertState(this, stateMachine));
        stateMachine.AddState(State.Aggressive, new Aggressive(this, stateMachine));
        projectile = transform.GetChild(1).GetComponent<EnemyProjectile>();
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

    protected void Attack()
    {
        projectile.transform.position = transform.position;
        projectile.transform.localRotation = targetDirection == Vector2.left ? Quaternion.Euler(0, 0, -90f) : Quaternion.Euler(0, 0, 90f);
        projectile.gameObject.SetActive(true);
    }


    IEnumerator HideRoutine()
    {
        readyToChangeState = false;
        yield return new WaitForSeconds(1.0f);
        readyToChangeState = true;
    }

    private abstract class HelmetEnemyState : StateBase<State, HelmetEnemy>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.GetComponent<Rigidbody2D>();
        protected SpriteRenderer renderer => owner.GetComponentInChildren<SpriteRenderer>();
        protected Animator animator => owner.animator;

        protected HelmetEnemyState(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }
    }

    private class IdleState : HelmetEnemyState
    {
        private Transform target;
        private float detectRange;

        public IdleState(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            detectRange = owner.detectRange;
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
                stateMachine.ChangeState(State.Alert);
            }
        }

        public override void Exit()
        {
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Attack");
        }
    }
    private class AlertState : HelmetEnemyState
    {
        private Transform target;
        private float detectRange;
        private bool inRange;
        

        public AlertState(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            detectRange = owner.detectRange;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            owner.targetDirection = transform.position.x - target.position.x > 0 ? Vector2.left : Vector2.right;
            renderer.flipX = owner.targetDirection == Vector2.right ? true : false;
        }

        public override void Transition()
        {
            // TODO: playerLayer 설정
            Debug.DrawRay(transform.position, owner.targetDirection * owner.attackRange, Color.red);
            if (Physics2D.Raycast(transform.position, owner.targetDirection, owner.attackRange, owner.playerMask))
            {
                stateMachine.ChangeState(State.Aggressive);
            }
            else if ((target.position - transform.position).sqrMagnitude > detectRange * detectRange)
            {
                stateMachine.ChangeState(State.Idle);
            }
        }

        public override void Exit()
        {
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Attack");
        }
    }
    private class Aggressive : HelmetEnemyState
    {
        private Transform target;
        private float attackRange;

        public Aggressive(HelmetEnemy owner, StateMachine<State, HelmetEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            attackRange = owner.attackRange;
        }

        public override void Enter()
        {
            owner.isInvincible = false;
            owner.Attack();
            owner.StartCoroutine("HideRoutine");
        }

        public override void Update()
        {
            owner.targetDirection = transform.position.x - target.position.x > 0 ? Vector2.left : Vector2.right;
            renderer.flipX = owner.targetDirection == Vector2.right ? true : false;
        }

        public override void Transition()
        {
            if (owner.readyToChangeState)
            {
                stateMachine.ChangeState(State.Alert);
            }
        }

        public override void Exit()
        {
            owner.isInvincible = true;
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Hide");
        }
    }
}
