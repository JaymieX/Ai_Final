using UnityEngine;

namespace Script
{
    public class Projectile : MonoBehaviour
    {
        public GameObject target;
        public float speed;
        public string killTag;

        private float eTime = 0f;

        private void Update()
        {
            eTime += Time.deltaTime;

            if (target != null)
            {
                Vector2 direction = target.transform.position - transform.position;
                direction.x += Mathf.PerlinNoise(eTime, 0f) * .5f;
                direction.x += Mathf.PerlinNoise(0f, eTime) * .5f;
                direction.Normalize();

                GetComponent<Rigidbody2D>().AddRelativeForce(Time.fixedDeltaTime * speed * direction);
            }
            else
            {
                Destroy(this.gameObject);
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
