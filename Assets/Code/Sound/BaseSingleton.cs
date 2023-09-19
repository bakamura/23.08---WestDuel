using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSingleton<T> : MonoBehaviour
    where T : MonoBehaviour// o T significa q qualquer classe pode ser criada com singleton, porém ela deve conter o que vem do Where, no caso, o MonoBehavior
{
    private static T _instance;

    public static T Instance {
        get {
            // se ainda n tiver uma referência da instancia, procura ela no GameObject
            if (_instance == null) _instance = GameObject.FindObjectOfType<T>();
            // se ainda n tiver uma referência da instancia, cria uma do tipo desejado
            if (_instance == null) _instance = new GameObject($"Instance of Type: {typeof(T)}").AddComponent<T>();            
            return _instance;
        }
    }

    // isso previne qualquer duplicata encontrada no projeto
    protected virtual void Awake() {
        if (_instance != null) Destroy(this);
    }
}
