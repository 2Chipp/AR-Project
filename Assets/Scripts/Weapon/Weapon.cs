using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject weaponPrefab;

    public float health;
    public float rangeShot;
    public float erroRange;
    public float shootForce;
    public float explosionForce;

    public float rotSpeed;

}
