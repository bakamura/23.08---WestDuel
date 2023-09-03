using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstantiateHandler
{
    public static GameObject GetPlayerPrefab()
    {
        return Resources.Load<GameObject>("/Prefab/Player");
    }

    public static GameObject GetAmmoBoxPrefab()
    {
        return Resources.Load<GameObject>("/Prefab/AmmoBox");
    }

    public static GameObject GetBulletPrefab()
    {
        return Resources.Load<GameObject>("/Prefab/Bullet");
    }
}
