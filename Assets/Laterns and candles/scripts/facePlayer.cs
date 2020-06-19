using System;
using UnityEngine;

public class facePlayer : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
    }

    void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}