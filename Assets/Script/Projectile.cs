using UnityEngine;

namespace Script
{
    public class Projectile : MonoBehaviour
    {
        public GameObject target;
        public float speed;
        public string killTag;

        private void Update()
        {
            if (target != null)
            {
                Vector2 direction = target.transform.position - transform.position;
                direction.Normalize();

                GetComponent<Rigidbody2D>().AddRelativeForce(Time.fixedDeltaTime * speed * direction);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(killTag))
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
