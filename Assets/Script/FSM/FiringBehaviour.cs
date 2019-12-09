using FLS;
using FLS.MembershipFunctions;
using FLS.Rules;
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

        private float _currentMaxInterval;
        private float _currentInterval;

        private LinguisticVariable _enemyCount;
        private IMembershipFunction _few;
        private IMembershipFunction _some;
        private IMembershipFunction _many;

        private LinguisticVariable _enemyDistance;
        private IMembershipFunction _close;
        private IMembershipFunction _med;
        private IMembershipFunction _far;

        private LinguisticVariable _power;
        private IMembershipFunction _light;
        private IMembershipFunction _norm;
        private IMembershipFunction _heavy;

        private IFuzzyEngine _fuzzyEngine;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            _currentInterval = 0;
            _currentMaxInterval = interval;

            _enemyCount = new LinguisticVariable("_enemyCount");
            _few = _enemyCount.MembershipFunctions.AddTrapezoid("_few", 0, 0, 5, 12.5);
            _some = _enemyCount.MembershipFunctions.AddTriangle("_some", 5, 12.5, 21.25);
            _many = _enemyCount.MembershipFunctions.AddTrapezoid("_many", 12.5, 21.25, 25, 25);

            _enemyDistance = new LinguisticVariable("_enemyDistance");
            _close = _enemyDistance.MembershipFunctions.AddTrapezoid("_close", 0, 0, 1, 10);
            _med = _enemyDistance.MembershipFunctions.AddTriangle("_med", 1, 10, 17);
            _far = _enemyDistance.MembershipFunctions.AddTrapezoid("_far", 10, 17, 20, 20);

            _power = new LinguisticVariable("_power");
            _light = _power.MembershipFunctions.AddTrapezoid("_light", 0, 0, 2, 5);
            _norm = _power.MembershipFunctions.AddTriangle("_norm", 2, 5, 7.6);
            _heavy = _power.MembershipFunctions.AddTrapezoid("_heavy", 5, 7.6, 10, 10);

            _fuzzyEngine = new FuzzyEngineFactory().Default();

            var rule1 = Rule.If(_enemyDistance.Is(_close).And(_enemyCount.Is(_few))).Then(_power.Is(_norm));
            var rule2 = Rule.If(_enemyDistance.Is(_close).And(_enemyCount.Is(_some))).Then(_power.Is(_heavy));
            var rule3 = Rule.If(_enemyDistance.Is(_close).And(_enemyCount.Is(_many))).Then(_power.Is(_heavy));

            var rule4 = Rule.If(_enemyDistance.Is(_med).And(_enemyCount.Is(_few))).Then(_power.Is(_light));
            var rule5 = Rule.If(_enemyDistance.Is(_med).And(_enemyCount.Is(_some))).Then(_power.Is(_norm));
            var rule6 = Rule.If(_enemyDistance.Is(_med).And(_enemyCount.Is(_many))).Then(_power.Is(_heavy));

            var rule7 = Rule.If(_enemyDistance.Is(_far).And(_enemyCount.Is(_few))).Then(_power.Is(_light));
            var rule8 = Rule.If(_enemyDistance.Is(_far).And(_enemyCount.Is(_some))).Then(_power.Is(_light));
            var rule9 = Rule.If(_enemyDistance.Is(_far).And(_enemyCount.Is(_many))).Then(_power.Is(_norm));

            _fuzzyEngine.Rules.Add(
                rule1,
                rule2,
                rule3,
                rule4,
                rule5,
                rule6,
                rule7,
                rule8,
                rule9
                );
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            if (_currentInterval >= _currentMaxInterval)
            {
                _currentInterval = 0;

                var objectsInRange = animator.GetComponent<Agent>().ObjectsInRange;
                if (objectsInRange.Count != 0)
                {
                    GameObject target = null;

                    int targetCount = 0;
                    float targetAvgDistance = 0;

                    foreach (var obj in objectsInRange)
                    {
                        if (obj.CompareTag(targetTag))
                        {
                            if (target == null)
                            {
                                target = obj;
                            }

                            targetCount++;
                            targetAvgDistance += Vector2.Distance(obj.transform.position, animator.transform.position);
                        }
                    }

                    if (target != null)
                    {
                        targetAvgDistance /= targetCount;

                        float result = (float)_fuzzyEngine.Defuzzify(new { _enemyDistance = (double)targetAvgDistance, _enemyCount = (double)targetCount });
                        _currentMaxInterval = interval / (result * .5f);

                        GameObject spawnedObject =
                            Instantiate(projectile, animator.transform.position, Quaternion.identity);

                        spawnedObject.GetComponent<Projectile>().killTag = targetTag;
                        spawnedObject.GetComponent<Projectile>().target = target;
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
