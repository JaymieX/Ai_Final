using Script.Global;
using UnityEngine;

namespace Script.FSM
{
    public class PlayerClickBehaviour : StateMachineBehaviour
    {
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            Agent agent = animator.GetComponent<Agent>();

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Moving");

                agent.CurrentPath = LevelMap.Instance.GetPath(agent.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}
