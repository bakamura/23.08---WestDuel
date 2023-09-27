using UnityEngine;

public class ServerInputReceiver : MonoBehaviour {

    [Header("Cache")]

    private InputDataPack _dataPackCache;

    private void Update() {
        ImplementPack();
    }

    private void ImplementPack() {
        if (DataReceiveHandler.queueInputData.Count > 0) {
            _dataPackCache = DataReceiveHandler.queueInputData.Dequeue();

            ServerPlayerInfo.player[_dataPackCache.senderIp].movement.SetInputDirection(PackingUtility.FloatArrayToVector2(_dataPackCache.movementInput));
            ServerPlayerInfo.player[_dataPackCache.senderIp].shoot.SetAimPoint(PackingUtility.FloatArrayToVector3(_dataPackCache.shootPoint));
            if (_dataPackCache.shootTrigger) ServerPlayerInfo.player[_dataPackCache.senderIp].shoot.Shoot();
        }
    }
}
