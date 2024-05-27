using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponShooter))]
public class TrajectoryTracer : MonoBehaviour
{
    private WeaponShooter weaponShooter;

    private LineRenderer lineRenderer;
    private Transform origin;

    [SerializeField]
    [Range(10, 100)]
    private int linePoints = 25;

    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float timeBetweenPoints = 0.06f;

    private float shootForce;
    private float bulletMass;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        weaponShooter = GetComponent<WeaponShooter>();
        lineRenderer = GetComponent<LineRenderer>();
        origin = weaponShooter.ShotPoint;
        shootForce = weaponShooter.ShotForce;
        bulletMass = weaponShooter.BulletPrefab.GetComponent<Rigidbody>().mass;
    }

    // Update is called once per frame
    void Update()
    {
        DrawProjection();
    }

    private void DrawProjection()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = origin.position;
        Vector3 startVelocity = shootForce * origin.forward / bulletMass;
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);
        }
    }
}
