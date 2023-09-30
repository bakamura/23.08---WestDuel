using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class ClientConnectionHandler
{
    public static Dictionary<IPEndPoint, MovableObjectData> PlayersList = new Dictionary<IPEndPoint, MovableObjectData>();

    [System.Serializable]
    public struct MovableObjectData
    {
        public GameObject Object;
        public Rigidbody Rigidbody;
        public AnimationsUpdate AnimationsUpdate;

        public MovableObjectData(GameObject obj)
        {
            Object = obj;
            Rigidbody = Object.GetComponent<Rigidbody>();
            AnimationsUpdate = Object.GetComponent<AnimationsUpdate>();
        }
    }
    public static bool _hasGameEnded;
}