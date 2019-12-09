using UnityEngine;

namespace Script.FSM
{
    public class FiringBehaviour : StateMachineBehaviour
    {
        [SerializeField]
        private float interval;

        [SerializeField]
        private GameObject projectile;

        [SerializeField]
        private string targetTag;

        private float _currentInterval;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _currentInterval = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_currentInterval >= interval)
            {
                _currentInterval = 0;

                var objectsInRange = animator.GetComponent<Agent>().ObjectsInRange;
                if (objectsInRange.Count != 0)
                {
                    foreach (var obj in objectsInRange)
                    {
                        if (obj.CompareTag(targetTag))
                        {
                            GameObject spawnedObject =
                                Instantiate(projectile, animator.transform.position, Quaternion.identity);

                            spawnedObject.GetComponent<Projectile>().killTag = targetTag;
                            spawnedObject.GetComponent<Projectile>().target = obj;
                        }
                    }
                }
            }
            else
            {
                _currentInterval += Time.deltaTime;
            }
        }
    }
}
