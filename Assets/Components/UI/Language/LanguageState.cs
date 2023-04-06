using UnityEngine;

[CreateAssetMenu(fileName = "New Language State", menuName = "Language State", order = 40)]
public class LanguageState : ScriptableObject
{
    public Language language = default;
}

public enum Language { EN, FR, DE, IT }
