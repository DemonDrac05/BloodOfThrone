using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/System/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<DialogueLine> dialogues;
}

[System.Serializable]
public class DialogueLine
{
    public Character Character;
    [TextArea] public string Text;
    public bool isQuestion;
    public List<Choice> Choices;
}

[System.Serializable]
public class Choice
{
    public string ChoiceText;
    public string AnswerText;
    public DialogueSO NextDialogue;
}

public enum Character
{
    Unknown,
    MainCharacter,
    Prince,
    Helga,
    Dagon
}