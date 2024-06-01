using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    public GameObject BulletPrefab { get => bulletPrefab; }

    private List<GameObject> bulletList;
    private int bulletCount = 10;

    private Bullet.BulletData bulletData;

    public static ObjectPool instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        bulletList = new List<GameObject>();

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            bulletList.Add(bullet);
            bullet.GetComponent<Bullet>()._BulletData = bulletData;
            bullet.SetActive(false);
        }
    }

    public void SetBulletData(float damageAmount, float explosionRange, float explosionForce)
    {
        bulletData.damageAmount = damageAmount;
        bulletData.explosionRange = explosionRange;
        bulletData.explosionForce = explosionForce;
    }

    public GameObject GetBullet()
    {
        GameObject bullet;

        if (bulletList.Count > 0)
        {
            bullet = bulletList[0];
            bullet.SetActive(true);
            bulletList.Remove(bulletList[0]);
            return bullet;
        }

        bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
        bullet.GetComponent<Bullet>()._BulletData = bulletData;
        return bullet;
    }

    public void BackToPool(GameObject bulletPrefab)
    {
        bulletList.Add(bulletPrefab);
        bulletPrefab.SetActive(false);
    }

}
