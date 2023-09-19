using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SceneInfoTransmiter : MonoBehaviour {

    public IPEndPoint _ipOther;
    public bool _isHost;
    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGameConnection() {
        if (_isHost) ServerConnectionHandler.players[0].ip = _ipOther;
        else ClientConnectionHandler.ServerEndPoint = _ipOther;
    }

}
