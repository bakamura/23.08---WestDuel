using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InstantiateHandler
{
    public static GameObject GetPlayerServerPrefab()
    {
        return Resources.Load<GameObject>("/Prefab/Player Instance Server");
    }

    public static GameObject GetPlayerClientPrefab()
    {
        return Resources.Load<GameObject>("/Prefab/Player Instance Client");
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
