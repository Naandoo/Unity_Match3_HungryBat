#if UNITY_EDITOR
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHelper", menuName = "Level/LevelHelper")]
public class LevelHelper : ScriptableObject
{
    public int levelNumber;
    public void SetLevel(int number)
    {
        PlayerPrefs.SetInt("savedLevel", number);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
#endif