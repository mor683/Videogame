using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon data")]
public class DungeonGeneratorData : ScriptableObject
{
    public int numCrawlers;
    public int iterationMin;
    public int iterationMax;
}
