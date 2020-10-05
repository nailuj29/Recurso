using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Recurso/LevelData", order = 0)]
public class LevelData : ScriptableObject {
    public int loops;
    public int maxMoves;
    public List<Vector3> tiles;
    public Vector3 goal;
    public Vector3 playerStart;
}