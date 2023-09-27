using UnityEngine;

public class ServerInputReceiver : MonoBehaviour {

    [Header("Cache")]

    private InputDataPack _dataPackCache;

    private void Update() {
        ImplementPack();
    }

    private void ImplementPack() {
        while (DataReceiveHandler.queueInputData.Count > 0) {
            _dataPackCache = DataReceiveHandler.queueInputData.Dequeue();

            ServerPlayerInfo.player[PackingUtility.StringToIPEndPoint(_dataPackCache.ipEpString)].movement.SetInputDirection(PackingUtility.FloatArrayToVector2(_dataPackCache.movementInput));
            ServerPlayerInfo.player[PackingUtility.StringToIPEndPoint(_dataPackCache.ipEpString)].shoot.SetAimPoint(PackingUtility.FloatArrayToVector3(_dataPackCache.shootPoint));
            if (_dataPackCache.shootTrigger) ServerPlayerInfo.player[PackingUtility.StringToIPEndPoint(_dataPackCache.ipEpString)].shoot.Shoot();
        }
    }
}
