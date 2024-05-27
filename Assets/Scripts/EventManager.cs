using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;

    private void Awake()
    {
        if (eventManager != null) Destroy(this);
        else eventManager = this;
    }
    
    public event Action OnShoot;
    public void Shoot()
    {
        OnShoot?.Invoke();
    }

    public event Action<float> OnTakeDamage;
    public void TakeDamage(float damageAmount)
    {
        if(OnTakeDamage!=null) OnTakeDamage(damageAmount);
    }
}
