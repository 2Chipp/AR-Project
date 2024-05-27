
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damageAmount;
    private float explosionRange;
    private float explosionForce;
    public BulletData _BulletData { get; set; }

    [SerializeField] private AnimationCurve damageVsDistanceCurve;

    

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        damageAmount = _BulletData.damageAmount;
        explosionForce = _BulletData.explosionForce;
        explosionRange = _BulletData.explosionRange;

        //curve = AnimationCurve.EaseInOut()
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Explode();
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
                    Debug.Log($"CurvePoint : {pointToEvaluate}, Distance {distance}, Total Damage : {damageVsDistanceCurve.Evaluate(pointToEvaluate) * damageAmount}");
                }
            }

            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }
    }

    public struct BulletData
    {
        public float damageAmount;
        public float explosionRange;
        public float explosionForce;
    }
}
