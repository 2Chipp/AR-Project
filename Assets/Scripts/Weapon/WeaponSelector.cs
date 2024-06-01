using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public Weapon[] weapons;
    private GameObject[] weaponPrefabs;

    private int weaponSelectedIndex = 0;

    private ObjectPool objectPool;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        objectPool = ObjectPool.instance;

        weaponPrefabs = new GameObject[weapons.Length];

        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject weaponPrefab = Instantiate(weapons[i].weaponPrefab, this.transform.position, this.transform.rotation, this.transform);
            weaponPrefabs[i] = weaponPrefab;

            WeaponMov weaponMov = weaponPrefab.GetComponent<WeaponMov>();
            weaponMov.SetRotSpeed(weapons[i].rotSpeed);

            WeaponShooter weaponShooter = weaponPrefab.GetComponent<WeaponShooter>();
            weaponShooter.SetShootForce(weapons[i].shotForce);
            objectPool.SetBulletData(weapons[i].shotDamage, weapons[i].explosionRange, weapons[i].explosionForce);

            weaponPrefab.SetActive(false);
        }
        weaponPrefabs[0].SetActive(true);
    }

    public void SelectWeapon(bool next)
    {
        weaponPrefabs[weaponSelectedIndex].SetActive(false);

        if (next) weaponSelectedIndex++;
        else weaponSelectedIndex--;

        if (weaponSelectedIndex < 0) weaponSelectedIndex = weapons.Length - 1;
        else if (weaponSelectedIndex >= weapons.Length) weaponSelectedIndex = 0;

        weaponPrefabs[weaponSelectedIndex].SetActive(true);
    }
}
