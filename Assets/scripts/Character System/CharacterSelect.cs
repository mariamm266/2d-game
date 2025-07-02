using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform[] spawnPoints;

    public void SelectCharacter(int prefabIndex, int playerID)
    {
        Transform spawnPoint = spawnPoints[playerID - 1];
        GameObject character = Instantiate(characterPrefabs[prefabIndex], spawnPoint.position, Quaternion.identity);
        
        // Configure input
        PlayerController pc = character.GetComponent<PlayerController>();
        pc.horizontalAxis = $"Horizontal_P{playerID}";
        pc.jumpButton = $"Jump_P{playerID}";
        pc.attackButton_1 = $"Attack_P{playerID}";
        pc.attackButton_2 = $"Attack_P{playerID}_2";
    }
}