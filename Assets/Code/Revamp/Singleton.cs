using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    public static T Instance;

    protected virtual void Awake() {
        if (Instance == null) Instance = this as T;
        else if(Instance != this) Destroy(gameObject);
    }

}
