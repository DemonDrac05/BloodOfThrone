using UnityEngine;

public class WitchHutLevel : MonoBehaviour
{
    public DialogueSO dialogueSO;

    public Transform Player;
    public Transform Prince; 
    public Transform Witch;

    public bool AbleToExit = false;

    public async void Update()
    {
        if (Player.position.x >= Prince.position.x - 2f
            && Player.position.x <= Witch.position.x + 2f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                await DialogueManager.Instance.DisplayDialogue(dialogueSO);
                AbleToExit = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (AbleToExit)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Cave");
            }
        }
    }
}
