using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Target : MonoBehaviour, IDamageable
{
    [SerializeField] private float hp;
    [SerializeField] private float defense;

    [SerializeField] private bool movingTarget;
    [SerializeField] private float speed;

    private Animator animator;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount - defense;
        if (hp <= 0) Destroy();
    }

    private void Destroy()
    {
        animator.SetBool("Destroyed", true);
    }
}
