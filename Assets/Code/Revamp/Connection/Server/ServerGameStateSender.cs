using System.Net;

public class ServerGameStateSender : Singleton<ServerGameStateSender> {

    private GameStateDataPack _dataPackCache = new GameStateDataPack();

    public void StartMatch() {
        _dataPackCache.gameState = GameStateDataPack.GameState.Initiate;

        SendPack();
    }

    public void UpdateHealth() {
        _dataPackCache.gameState = GameStateDataPack.GameState.Continue;
        foreach (IPEndPoint ip in ServerPlayerInfo.player.Keys) _dataPackCache.playerHealth[ip] = ServerPlayerInfo.player[ip].health.GetCurrentHealth(); // Implement GetCurrentHealth in PlayerHealth()

        SendPack();
    }

    public void EndMatch() {
        _dataPackCache.gameState = GameStateDataPack.GameState.Ended;
        foreach (IPEndPoint ip in ServerPlayerInfo.player.Keys) _dataPackCache.playerHealth[ip] = ServerPlayerInfo.player[ip].health.GetCurrentHealth(); // Implement GetCurrentHealth in PlayerHealth()

        SendPack();
    }

    public void RestartMatch() {
        _dataPackCache.gameState = GameStateDataPack.GameState.Restart;

        SendPack();
    }

    public void QuitMatch() {
        _dataPackCache.gameState = GameStateDataPack.GameState.Quit;

        SendPack();
    }

    private void SendPack() {
        DataSendHandler.SendPack(ConnectionHandler.DataPacksIdentification.GameStateDataPack, _dataPackCache, null); // null => JoinerIP
    }

    public void AddPlayerIP(IPEndPoint[] playerIp) { // Make ServerPlayerInfo call this
        _dataPackCache = new GameStateDataPack(playerIp);
    }

    private void OnApplicationQuit() {
        QuitMatch();
    }

}
