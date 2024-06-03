using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Transform shotPoint;
    private GameObject currentBulletPrefab;

    private Dictionary<GameObject, Rigidbody> bulletRbDictionary;
    private Dictionary<GameObject, Bullet> bulletDataDictionary;
    private Bullet.BulletData bulletData;

    [SerializeField] private float bulletLifetime = 2f;
    private WaitForSeconds waitForSeconds;

    private EventManager eventManager;
    private ObjectPool objectPool;

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

    // DataToBullet ===========================
    public float DamageAmount { get; set; }


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
        bulletRbDictionary = new Dictionary<GameObject, Rigidbody>();
        bulletDataDictionary = new Dictionary<GameObject, Bullet>();
        objectPool = ObjectPool.instance;
    }

    private Rigidbody AddBulletRb(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bulletRbDictionary.Add(bullet, rb);
        return rb;
    }

    private Bullet AddBulletData(GameObject bullet)
    {
        Bullet bulletData = bullet.GetComponent<Bullet>();
        bulletDataDictionary.Add(bullet, bulletData);
        return bulletData;
    }

    IEnumerator DisableBullet(GameObject bullet)
    {
        yield return waitForSeconds;
        if (bulletRbDictionary.TryGetValue(bullet, out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            objectPool.BackToPool(bullet);
        }
    }
    public void SetBulletData(float damageAmount, float explosionRange, float explosionForce)
    {
        bulletData.damageAmount = damageAmount;
        bulletData.explosionRange = explosionRange;
        bulletData.explosionForce = explosionForce;
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
            currentBulletPrefab = objectPool.GetBullet();
            Rigidbody currentRb;
            Bullet currentBulletData;

            if(bulletRbDictionary.TryGetValue(currentBulletPrefab, out Rigidbody _rb))
            {
                currentRb = _rb;
            }
            else currentRb = AddBulletRb(currentBulletPrefab);

            if (bulletDataDictionary.TryGetValue(currentBulletPrefab, out Bullet _bulletData))
            {
                currentBulletData = _bulletData;
            }
            else currentBulletData = AddBulletData(currentBulletPrefab);

            currentBulletPrefab.SetActive(true);
            currentBulletPrefab.transform.position = shotPoint.position;
            currentBulletPrefab.transform.rotation = shotPoint.rotation;

            currentBulletData._BulletData = bulletData;
            currentRb.AddForce(currentBulletPrefab.transform.forward * shotForce, ForceMode.Impulse);

            currentRb.AddForce(WingController.WingDirection.forward * WingController.WingForce, ForceMode.Force);
            Debug.Log(" Wing force: " + WingController.WingForce);

            StartCoroutine(DisableBullet(currentBulletPrefab));
        }
    }

#elif UNITY_ANDROID
    public void Shoot()
    {
        if (isSelected)
        {
            currentBulletPrefab = objectPool.GetBullet();
            Rigidbody rb;

            if(bulletRbDictionary.TryGetValue(currentBulletPrefab, out Rigidbody _rb))
            {
                rb = _rb;
            }
            else rb = AddBulletRb(currentBulletPrefab);

            currentBulletPrefab.SetActive(true);
            currentBulletPrefab.transform.position = shotPoint.position;
            currentBulletPrefab.transform.rotation = shotPoint.rotation;

            rb.AddForce(currentBulletPrefab.transform.forward * shotForce, ForceMode.Impulse);

            StartCoroutine(DisableBullet(currentBulletPrefab));
        }
    }
#endif

}
