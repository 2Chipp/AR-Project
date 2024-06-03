using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingController : MonoBehaviour
{
    [SerializeField, Range(-180f, 180f)] private float maxVerticalRotationRange;
    [SerializeField, Range(-180f, 180f)] private float minVerticalRotationRange;

    [SerializeField, Range(0.01f,10f)] private float maxWingForceRange;
    [SerializeField, Range(0.01f,10f)] private float minWingForceRange;

    [SerializeField, Range(0,1000)] private float wF;

    [SerializeField, Range(0, 1f)] private float TimeScale;


    public static Transform WingDirection { get; set; }
    public static float WingForce { get; set; }

    private void Start()
    {
        SetWingDir();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) SetWingDir();
    }

    private void SetTimeScale()
    {
        Time.timeScale = TimeScale;
    }
    private void SetWingDir()
    {
        float wingForce = wF;
        //float wingForce = Random.Range(minWingForceRange, maxWingForceRange);
        float verticalRotation = Random.Range(minVerticalRotationRange, maxVerticalRotationRange);
        float horizontalRotation = Random.Range(0, 359);

        transform.eulerAngles = new Vector3( 0, horizontalRotation, 0);
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, 0);

        WingDirection = transform;
        WingForce = wingForce;

        SetTimeScale();
    }
}
