using UnityEngine;

namespace Script
{
    public class Projectile : MonoBehaviour
    {
        public GameObject target;
        public float speed;
        public string killTag;
        public float life;

        private float eTime = 0f;
        private Vector2 targetPos;

        private bool dirLock = false;

        private void Update()
        {
            eTime += Time.deltaTime;

            if (target != null)
            {
                targetPos = target.transform.position;
            }
            else
            {
                if (!dirLock)
                {
                    targetPos *= 50f;
                    dirLock = true;
                }
            }

            Vector2 direction = targetPos - (Vector2)transform.position;
            direction.x += Mathf.PerlinNoise(eTime, 0f) * .4f;
            direction.x += Mathf.PerlinNoise(0f, eTime) * .4f;
            direction.Normalize();

            GetComponent<Rigidbody2D>().AddRelativeForce(Time.fixedDeltaTime * speed * direction);

            if (eTime >= life)
            {
                Destroy(gameObject);
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
