using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponShooter))]
public class TrajectoryTracer : MonoBehaviour
{
    private WeaponShooter weaponShooter;
    private ObjectPool objectPool;

    private LineRenderer lineRenderer;
    private Transform origin;

    [Header("Trace Line")]

    [SerializeField, Range(10, 100)] private int linePoints = 25;
    [SerializeField, Range(0.1f, 5f)] private float lineRange;
    [SerializeField, Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.06f;

    private float shotForce;
    private float bulletMass;

    //==========================

    [Header("Rings")]

    [SerializeField] private int ringCount;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private GameObject[] ringPrefabArray;

    private Transform ringTransform;
    private Transform ringTarget;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        objectPool = ObjectPool.instance;
        weaponShooter = GetComponent<WeaponShooter>();
        lineRenderer = GetComponent<LineRenderer>();
        origin = weaponShooter.ShotPoint;
        shotForce = weaponShooter.ShotForce;
        bulletMass = objectPool.BulletPrefab.GetComponent<Rigidbody>().mass;
        ringTransform = new GameObject().transform;
        ringTarget = new GameObject().transform;

        ringPrefabArray = new GameObject[ringCount];
        for (int i = 0; i < ringPrefabArray.Length; i++)
        {
            ringPrefabArray[i] = Instantiate(ringPrefab, Vector3.up*100, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DrawProjection();
    }

    private void DrawProjection()
    {
        int ringArrayIndex = 0;

        float spaceBetweenRings = lineRange / (ringCount + 1);
        float spaceBetweenRingsSummation = spaceBetweenRings;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints);

        Vector3 startPosition = origin.position;
        Vector3 startVelocity = shotForce * origin.forward / bulletMass;

        int i = 0;
        lineRenderer.SetPosition(i, startPosition);

        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            if (time > lineRange) break;

            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);

            if (time > spaceBetweenRingsSummation) // Add rings
            {
                Vector3 ringPoint = startPosition + spaceBetweenRingsSummation * startVelocity;
                ringPoint.y = startPosition.y + startVelocity.y * spaceBetweenRingsSummation + (Physics.gravity.y / 2f * spaceBetweenRingsSummation * spaceBetweenRingsSummation);
                
                Vector3 _ringTarget = startPosition + (spaceBetweenRingsSummation + 0.01f ) * startVelocity;
                _ringTarget.y = startPosition.y + startVelocity.y * (spaceBetweenRingsSummation + 0.01f) + (Physics.gravity.y / 2f * (spaceBetweenRingsSummation + 0.01f) * (spaceBetweenRingsSummation + 0.01f));

                ringTransform.position = ringPoint;
                ringTarget.position = _ringTarget;
                ringTransform.LookAt(ringTarget);
                                
                ringPrefabArray[ringArrayIndex].transform.position = ringTransform.position;
                ringPrefabArray[ringArrayIndex].transform.rotation = ringTransform.rotation;
                ringArrayIndex++;

                spaceBetweenRingsSummation = spaceBetweenRings * (ringArrayIndex + 1);
            }
        }
        lineRenderer.positionCount = i;
    }
}
