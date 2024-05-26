using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject[] bulletArray;
    private Rigidbody[] bulletRb;

    [SerializeField] private int bulletCount = 10;
    private int bulletIndex;

    [SerializeField] private float bulletLifetime = 2f;
    WaitForSeconds waitForSeconds;

    private EventManager eventManager;

    private float shootForce;

    private bool isSelected;

    // DataToTrajectoryTracer ===============
    public float ShootForce {
        get { return shootForce; }
        set { shootForce = value; }        
    }
    public Transform ShootPoint
    {
        get { return shootPoint; }
    }
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        eventManager = EventManager.eventManager;
        eventManager.OnShoot += Shoot;

        waitForSeconds = new WaitForSeconds(bulletLifetime);

        bulletArray = new GameObject[bulletCount];
        bulletRb = new Rigidbody[bulletCount];

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            bulletArray[i] = bullet;
            bulletRb[i] = bullet.GetComponent<Rigidbody>();
            bullet.SetActive(false);
        }
    }

    IEnumerator DisableBullet(int bulletIndex)
    {
        yield return waitForSeconds;
        bulletRb[bulletIndex].velocity = Vector3.zero;
        bulletArray[bulletIndex].SetActive(false);
    }

    public void SetShootForce(float shootForce)
    {
        ShootForce = shootForce;
    }

    private void OnDestroy()
    {
        eventManager.OnShoot -= Shoot;
    }

    private void OnEnable()
    {
        isSelected = true;
    }

    private void OnDisable()
    {
        isSelected = false;
        StopAllCoroutines();
    }

#if UNITY_EDITOR
    public void Shoot()
    {
        if (isSelected)
        {
            bulletArray[bulletIndex].SetActive(true);
            bulletRb[bulletIndex].velocity = Vector3.zero;

            bulletArray[bulletIndex].transform.position = shootPoint.position;
            bulletArray[bulletIndex].transform.rotation = shootPoint.rotation;

            bulletRb[bulletIndex].AddForce(bulletRb[bulletIndex].transform.forward * shootForce, ForceMode.Impulse);

            StartCoroutine(DisableBullet(bulletIndex));

            if (bulletIndex >= bulletCount - 1) bulletIndex = 0;
            else bulletIndex++;
        }
    }

#elif UNITY_ANDROID
    public void Shoot()
    {
        if (isSelected)
        {
            bulletArray[bulletIndex].SetActive(true);
            bulletRb[bulletIndex].velocity = Vector3.zero;

            bulletArray[bulletIndex].transform.position = shootPoint.position;
            bulletArray[bulletIndex].transform.rotation = shootPoint.rotation;

            bulletRb[bulletIndex].AddForce(bulletRb[bulletIndex].transform.forward * shootForce, ForceMode.Impulse);

            StartCoroutine(DisableBullet(bulletIndex));

            if (bulletIndex >= bulletCount - 1) bulletIndex = 0;
            else bulletIndex++;
        }
    }
#endif

}
