using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PatrolEnemy : Enemy
{
    // TODO: state enter exit에 점프 trace에서는 레이캐스팅으로 점프
    public enum State
    {
        Idle,
        Trace,
        Returning,
        Size
    }

    StateMachine<State, PatrolEnemy> stateMachine;
    // TODO: GameManager를 사용해 플레이어 접근
    [SerializeField]
    private Transform target;
    [SerializeField]
    protected LayerMask playerMask;
    [SerializeField]
    private LayerMask groundMask;
    private Transform cliffSensor;
    protected Animator animator;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float jumpPower;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<State, PatrolEnemy>(this);
        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Trace, new TraceState(this, stateMachine));
        stateMachine.AddState(State.Returning, new ReturningState(this, stateMachine));
    }

    private void Start()
    {
        stateMachine.SetUp(State.Idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void Move()
    {
        if (DetectCliff())
        {
            transform.Rotate(Vector3.up, 180);
        }
        
        rigidbody.velocity = new Vector2(transform.right.x * moveSpeed, rigidbody.velocity.y);
    }

    protected void Jump()
    {
        // 트리거 사용하니 addforce말고 translate로 직접 구현
        // 혹은 velocity y 혹은 코루틴 최고 높이에 도달할 수록 속도가 느려지고 도달한 순간 -로
    }

    private void DetectPlayerHorizontal()
    {
        // 탐지 범위 안 O && x축 레이캐스팅 감지 O 조건문
        Jump();
    }

    private bool DetectCliff()
    {
        return !Physics2D.Raycast(cliffSensor.position, Vector2.down, transform.localScale.y + 0.1f, groundMask);
    }

    private abstract class PatrolEnemyState : StateBase<State, PatrolEnemy>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.GetComponent<Rigidbody2D>();
        protected SpriteRenderer renderer => owner.GetComponentInChildren<SpriteRenderer>();
        protected Animator animator => owner.animator;

        protected PatrolEnemyState(PatrolEnemy owner, StateMachine<State, PatrolEnemy> stateMachine) : base(owner, stateMachine)
        {
        }
    }

    private class IdleState : PatrolEnemyState
    {
        private Transform target;
        private float range;

        public IdleState(PatrolEnemy owner, StateMachine<State, PatrolEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            range = owner.detectRange;
        }

        public override void Enter()
        {
            rigidbody.velocity = Vector3.zero;
        }

        public override void Update()
        {

        }

        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude < range * range)
            {
                stateMachine.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {

        }
    }

    private class TraceState : PatrolEnemyState
    {
        private Transform target;
        private float speed;
        private float range;

        public TraceState(PatrolEnemy owner, StateMachine<State, PatrolEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            target = owner.target;
            speed = owner.moveSpeed;
            range = owner.detectRange;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            Vector2 dir = (target.position - transform.position).normalized;
            rigidbody.velocity = dir * speed;
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;
        }

        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude > range * range)
            {
                stateMachine.ChangeState(State.Returning);
            }
        }

        public override void Exit()
        {

        }
    }

    private class ReturningState : PatrolEnemyState
    {
        private Vector3 returnPosition;
        private float speed;

        public ReturningState(PatrolEnemy owner, StateMachine<State, PatrolEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
            returnPosition = transform.position;
            speed = owner.moveSpeed;
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            Vector2 dir = (returnPosition - transform.position).normalized;
            rigidbody.velocity = dir * speed;
            renderer.flipX = rigidbody.velocity.x > 0 ? true : false;
        }

        public override void Transition()
        {
            if ((returnPosition - transform.position).sqrMagnitude < 0.01f)
            {
                stateMachine.ChangeState(State.Idle);
            }
        }

        public override void Exit()
        {

        }
    }
}
