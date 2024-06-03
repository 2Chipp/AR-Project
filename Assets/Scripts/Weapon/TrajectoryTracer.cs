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

    [SerializeField]
    [Range(10, 100)]
    private int linePoints = 25;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float lineRange;

    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float timeBetweenPoints = 0.06f;

    private float shotForce;
    private float bulletMass;

    //==========================

    [SerializeField]
    private int ringCount;

    [SerializeField]
    private GameObject ringPrefab;
    [SerializeField] private GameObject[] ringPrefabArray;

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
        float spaceBetweenRings = lineRange / (ringCount + 1) ;
        float spaceBetweenRingsSum = spaceBetweenRings;
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

            while (time > spaceBetweenRingsSum)
            {
                Debug.Log($"Index = {ringArrayIndex}, SBRSum = {spaceBetweenRingsSum}, i = {i}");
                ringPrefabArray[ringArrayIndex].transform.position = point;
                ringArrayIndex++;

                spaceBetweenRingsSum = spaceBetweenRings * (ringArrayIndex + 1);
            }
        }
        lineRenderer.positionCount = i;
    }
}
