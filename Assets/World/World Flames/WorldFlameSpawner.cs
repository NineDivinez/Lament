using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFlameSpawner : MonoBehaviour
{
    #region Singleton
    public static WorldFlameSpawner instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    static public Transform spawner;
}
