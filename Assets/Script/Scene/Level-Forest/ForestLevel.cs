using UnityEngine;

public class ForestLevel : MonoBehaviour
{
    public DialogueSO dialogueSO;

    public Transform Player;
    public Transform Prince;

    public async void Update()
    {
        if (Player.position.x >= Prince.position.x - 1f
            && Player.position.x <= Prince.position.x + 1f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                await DialogueManager.Instance.DisplayDialogue(dialogueSO);
                await DialogueManager.Instance.FadeInEffect();
                UnityEngine.SceneManagement.SceneManager.LoadScene("WitchHut");
            }
        }
    }
}
