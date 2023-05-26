using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    // TODO: state enter exit에 애니메이션
    public enum State
    {
        Idle,
        Trace,
        Returning,
        Size
    }
    StateMachine<State, FlyingEnemy> stateMachine;
    // TODO: GameManager를 사용해 플레이어 접근
    [SerializeField]
    private Transform target;
    private Animator animator;
    [SerializeField]
    protected LayerMask playerMask;
    [SerializeField]
    protected float moveSpeed;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<State, FlyingEnemy>(this);
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

    private abstract class FlyingEnemyState : StateBase<State, FlyingEnemy>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.GetComponent<Rigidbody2D>();
        protected SpriteRenderer renderer => owner.GetComponentInChildren<SpriteRenderer>();
        protected Animator animator => owner.animator;

        protected FlyingEnemyState(FlyingEnemy owner, StateMachine<State, FlyingEnemy> stateMachine) : base(owner, stateMachine)
        {
        }
    }

    private class IdleState : FlyingEnemyState
    {
        private Transform target;
        private float range;

        public IdleState(FlyingEnemy owner, StateMachine<State, FlyingEnemy> stateMachine) : base(owner, stateMachine)
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

    private class TraceState : FlyingEnemyState
    {
        private Transform target;
        private float speed;
        private float range;

        public TraceState(FlyingEnemy owner, StateMachine<State, FlyingEnemy> stateMachine) : base(owner, stateMachine)
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

    private class ReturningState : FlyingEnemyState
    {
        private Vector3 returnPosition;
        private float speed;

        public ReturningState(FlyingEnemy owner, StateMachine<State, FlyingEnemy> stateMachine) : base(owner, stateMachine)
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
