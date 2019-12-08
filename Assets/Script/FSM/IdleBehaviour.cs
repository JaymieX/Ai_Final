using UnityEngine;

namespace Script.FSM
{
    public class IdleBehaviour : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (animator.GetComponent<Agent>().CurrentPath.Count > 0)
            {
                animator.SetBool("Move", true);
            }
        }
    }
}
