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
        Jump,
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
    // TODO: 센서의 transform 코드 가져오기
    [SerializeField]
    private Transform cliffSensor;
    protected Animator animator;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float jumpPower;
    [SerializeField]
    protected float fallSpeed;
    protected Vector2 targetDirection;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<State, PatrolEnemy>(this);
        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Trace, new TraceState(this, stateMachine));
        stateMachine.AddState(State.Jump, new JumpState(this, stateMachine));
        stateMachine.AddState(State.Returning, new ReturningState(this, stateMachine));
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        stateMachine.SetUp(State.Idle);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private bool DetectCliff()
    {
        return !Physics2D.Raycast(cliffSensor.position, Vector2.down, transform.localScale.y + 0.1f, groundMask);
    }

    private abstract class PatrolEnemyState : StateBase<State, PatrolEnemy>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.rigidbody;
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
            Patrol();
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

        private void Patrol()
        {
            if (owner.DetectCliff())
            {
                transform.Rotate(Vector3.up, 180);
            }

            rigidbody.velocity = new Vector2(-transform.right.x * owner.moveSpeed, rigidbody.velocity.y);
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
            owner.targetDirection = transform.position.x - target.position.x > 0 ? Vector2.left : Vector2.right;
            renderer.flipX = owner.targetDirection == Vector2.right ? true : false;
            rigidbody.velocity = owner.targetDirection * speed;
        }

        public override void Transition()
        {
            if ((target.position - transform.position).sqrMagnitude > range * range)
            {
                stateMachine.ChangeState(State.Returning);
            }
            else if (Physics2D.Raycast(transform.position, owner.targetDirection, range, owner.playerMask))
            {
                stateMachine.ChangeState(State.Jump);
            }
            Debug.DrawRay(transform.position, owner.targetDirection * range, Color.yellow);
        }

        public override void Exit()
        {

        }
    }

    private class JumpState : PatrolEnemyState
    {
        private float startHeight;
        private float highestHeight;

        public JumpState(PatrolEnemy owner, StateMachine<State, PatrolEnemy> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {
        }

        public override void Enter()
        {
            startHeight = transform.position.y;
            highestHeight = startHeight + owner.jumpPower;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, Vector2.up.y * owner.fallSpeed);
        }

        public override void Update()
        {
            if (highestHeight <= transform.position.y)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, Vector2.down.y * owner.fallSpeed);
            }
        }

        public override void Transition()
        {
            if (transform.position.y <= startHeight)
            {
                stateMachine.ChangeState(State.Trace);
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
            renderer.flipX = owner.targetDirection == Vector2.right ? true : false;
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
