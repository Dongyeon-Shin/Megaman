using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class YellowDevil : Enemy
{
    public enum State
    {
        Spawn,
        Split,
        EyeBeam,
        Die,
        Size
    }
    Transform[] devilProjectiles;
    StateMachine<State, YellowDevil> stateMachine;
    protected Animator animator;
    private void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<State, YellowDevil>(this);
        stateMachine.AddState(State.Spawn, new SpawnState(this, stateMachine));
        stateMachine.AddState(State.Split, new SplitState(this, stateMachine));
        stateMachine.AddState(State.EyeBeam, new EyeBeamState(this, stateMachine));
        stateMachine.AddState(State.Die, new DieState(this, stateMachine));
    }

    private void Start()
    {
        stateMachine.SetUp(State.Spawn);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private abstract class YellowDevilState : StateBase<State, YellowDevil>
    {
        protected GameObject gameObject => owner.gameObject;
        protected Transform transform => owner.transform;
        protected Rigidbody2D rigidbody => owner.GetComponent<Rigidbody2D>();
        protected SpriteRenderer renderer => owner.GetComponentInChildren<SpriteRenderer>();
        protected Animator animator => owner.animator;

        protected YellowDevilState(YellowDevil owner, StateMachine<State, YellowDevil> stateMachine) : base(owner, stateMachine)
        {
        }
    }

    private class SpawnState : YellowDevilState
    {


        public SpawnState(YellowDevil owner, StateMachine<State, YellowDevil> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Transition()
        {

        }

        public override void Exit()
        {
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Attack");
        }
    }

    private class SplitState : YellowDevilState
    {


        public SplitState(YellowDevil owner, StateMachine<State, YellowDevil> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Transition()
        {

        }

        public override void Exit()
        {
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Attack");
        }
    }

    private class EyeBeamState : YellowDevilState
    {


        public EyeBeamState(YellowDevil owner, StateMachine<State, YellowDevil> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Transition()
        {

        }

        public override void Exit()
        {
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Attack");
        }
    }

    private class DieState : YellowDevilState
    {


        public DieState(YellowDevil owner, StateMachine<State, YellowDevil> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Transition()
        {

        }

        public override void Exit()
        {
            // TODO: 애니메이션 실행
            //animator.SetTrigger("Attack");
        }
    }
}
