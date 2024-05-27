using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Transform shotPoint;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject[] bulletArray;
    private Rigidbody[] bulletRb;

    [SerializeField] private int bulletCount = 10;
    private int bulletIndex;

    [SerializeField] private float bulletLifetime = 2f;
    private WaitForSeconds waitForSeconds;

    private EventManager eventManager;

    private float shotForce;

    private bool isSelected;

    // DataToTrajectoryTracer ===============
    public float ShotForce {
        get { return shotForce; }
        set { shotForce = value; }        
    }
    public Transform ShotPoint
    {
        get { return shotPoint; }
    }
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }

    // DataToBullet ===========================
    public float DamageAmount { get; set; }

    private Bullet.BulletData bulletData;


    private void Awake()
    {
        eventManager = EventManager.eventManager;
        eventManager.OnShoot += Shoot;
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        waitForSeconds = new WaitForSeconds(bulletLifetime);

        bulletArray = new GameObject[bulletCount];
        bulletRb = new Rigidbody[bulletCount];

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            bulletArray[i] = bullet;
            bulletArray[i].GetComponent<Bullet>()._BulletData = bulletData;
            bulletRb[i] = bullet.GetComponent<Rigidbody>();
            bullet.SetActive(false);
        }
    }
    
    public void SetBulletData(float damageAmount, float explosionRange, float explosionForce)
    {
        bulletData.damageAmount = damageAmount;
        bulletData.explosionRange = explosionRange;
        bulletData.explosionForce = explosionForce;
    }

    IEnumerator DisableBullet(int bulletIndex)
    {
        yield return waitForSeconds;
        bulletRb[bulletIndex].velocity = Vector3.zero;
        bulletArray[bulletIndex].SetActive(false);
    }

    public void SetShootForce(float shootForce)
    {
        ShotForce = shootForce;
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

            bulletArray[bulletIndex].transform.position = shotPoint.position;
            bulletArray[bulletIndex].transform.rotation = shotPoint.rotation;

            bulletRb[bulletIndex].AddForce(bulletRb[bulletIndex].transform.forward * shotForce, ForceMode.Impulse);

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
