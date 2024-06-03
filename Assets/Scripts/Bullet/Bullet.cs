
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;

    private float damageAmount;
    private float explosionRange;
    private float explosionForce;
    public BulletData _BulletData {
        set 
        {
            damageAmount = value.damageAmount;
            explosionForce = value.explosionForce;
            explosionRange = value.explosionRange;
        } 
    }

    [SerializeField] private AnimationCurve damageVsDistanceCurve;

    private bool isExploded;

    // =================

    [SerializeField] private float timeBetweenPoints;
    private float time;
    private float timeDelay;
    private WaitForSeconds waitForSeconds;

    private void Update()
    {
        if (isExploded) return;
        CheckCollision();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (isExploded) return;
        //Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach ( Collider collider in colliders)
        {
            Transform direction = transform;
            direction.LookAt(collider.transform.position);

            if(Physics.Raycast(transform.position, direction.forward, out RaycastHit hit))
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                float pointToEvaluate = distance / explosionRange;
                float totalDamage = damageVsDistanceCurve.Evaluate(pointToEvaluate) * damageAmount;

                if (collider.gameObject.TryGetComponent<IDamageable>(out IDamageable target))
                {
                    target.TakeDamage(totalDamage);
                }
            }

            //Rigidbody rb = collider.GetComponent<Rigidbody>();
            //if (rb != null)
            //{
            //    rb.AddExplosionForce(explosionForce, transform.position, explosionRange);
            //}
        }
    }

    private void CheckCollision()
    {
        if (Time.unscaledTime > time)
        {
            time += timeBetweenPoints;

            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            Transform currentPos = transform;
            Vector3 nextPos = currentPos.position + (velocity * timeBetweenPoints);
            currentPos.LookAt(nextPos);

            float rayDistance = Vector3.Distance(currentPos.position, nextPos);
            Ray ray = new Ray(currentPos.position, currentPos.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                timeDelay = Vector3.Distance(currentPos.position, hit.point) / velocity.magnitude;
                waitForSeconds = new WaitForSeconds(timeDelay);
                Debug.Log($"Distance: {rayDistance}, Collision in {hit.point} in {timeDelay} seconds");
                isExploded = true;
                StartCoroutine(Collision());
            }
        }

    }

    private IEnumerator Collision()
    {
        yield return waitForSeconds;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Debug.Log("Boom!");

        Explode();
    }

    private void OnDisable()
    {
        isExploded = false;
    }

    public struct BulletData
    {
        public float damageAmount;
        public float explosionRange;
        public float explosionForce;
    }
}
