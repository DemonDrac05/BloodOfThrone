using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/System/SSDialogue")]
public class SSDialogueSO : ScriptableObject
{
    public List<SSDialogueLine> SSDialogueLines;
}

[System.Serializable]
public class SSDialogueLine
{
    public SSCharacter Character;
    [TextArea] public string Text;
}

public enum SSCharacter
{
    Evelyn,
    Raspitol
}