using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager eventManager;

    public event Action OnShoot;
    private void Awake()
    {
        if (eventManager != null) Destroy(this);
        else eventManager = this;
    }
    
    public void Shoot()
    {
        OnShoot?.Invoke();
    }
}
