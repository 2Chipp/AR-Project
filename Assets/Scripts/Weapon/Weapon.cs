using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;

    public float health;
    public float shotDamage;
    public float erroRange;
    public float shotForce;
    public float explosionRange;

    public float rotSpeed;

}
