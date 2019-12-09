using UnityEngine;

namespace Script.FSM
{
    public class PathMoveBehaviour : StateMachineBehaviour
    {
        private Agent _agent;
        private Rigidbody2D _body;

        private Vector2 _currentTarget;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _currentTarget = Vector2.zero;

            _agent = animator.GetComponent<Agent>();
            _body = animator.GetComponent<Rigidbody2D>();

            if (_agent.CurrentPath.Count != 0)
            {
                _currentTarget = _agent.CurrentPath.Pop();
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_currentTarget == Vector2.zero)
            {
                if (_agent.CurrentPath.Count != 0)
                {
                    _currentTarget = _agent.CurrentPath.Pop();
                }
                else
                {
                    return;
                }
            }

            // Far, move in
            if (Vector2.Distance(_agent.transform.position, _currentTarget) > 0.1f)
            {
                Vector2 direction = _currentTarget - (Vector2)_agent.transform.position;
                direction.Normalize();

                _body.AddRelativeForce(Time.fixedDeltaTime * _agent.speed * direction, ForceMode2D.Force);
            }
            // Close switch to next
            else
            {
                if (_agent.CurrentPath.Count != 0)
                {
                    _currentTarget = _agent.CurrentPath.Pop();
                }
                else
                {
                    animator.SetBool("Move", false);
                }
            }
        }
    }
}
