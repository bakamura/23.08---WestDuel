using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerGameStateReceiver : MonoBehaviour {

    [Header("Cache")]

    private GameStateDataPack _dataPackCache;

    private void Update() {
        ImplementPack();
    }

    private void ImplementPack() {
        if (DataReceiveHandler.queueInputData.Count > 0) {
            _dataPackCache = DataReceiveHandler.queueGameStateData.Dequeue();

            // Game State Stuff
        }
    }

}
