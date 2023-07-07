using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObtects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    // Variables economicas
    public float _PlayerMoney { private set; get; } = 0f;
}