using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Transform shotPoint;
    private GameObject currentBulletPrefab;

    private Dictionary<GameObject, Rigidbody> bulletRbDictionary;

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
        objectPool = ObjectPool.instance;
    }

    private Rigidbody AddBulletRb(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bulletRbDictionary.Add(bullet, rb);
        return rb;
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

#elif UNITY_ANDROID
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
#endif

}
