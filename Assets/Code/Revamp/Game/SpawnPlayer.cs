using UnityEngine;
using System.Linq;

public class SpawnPlayer : MonoBehaviour {

    [SerializeField] private Vector3[] _spawnPoints;

    public Vector3 GetPointFurthestFromOponent(Vector3 oponentPos) {
        return _spawnPoints.OrderByDescending(x => Vector3.Distance(oponentPos, x)).First();
    }
}
