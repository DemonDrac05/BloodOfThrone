using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;


public class CaveLevel : MonoBehaviour
{
    public DialogueSO cave1;
    public DialogueSO cave2;
    public DialogueSO cave3;
    public DialogueSO cave4;
    public DialogueSO cave5;

    public Transform Player;
    public Transform Prince;
    public Transform Dagon;

    public Transform PlayerCheckpoint1;
    public Transform PrinceCheckpoint1;

    public GameObject Coffer;

    public Transform DagonCheckpoint1;

    public Transform PlayerCheckpoint2;
    public Transform DagonCheckpoint2;

    public async void Start()
    {
        await DialogueManager.Instance.DisplayDialogue(cave1);

        await DialogueManager.Instance.FadeInEffect();
        Player.position = new Vector3(PlayerCheckpoint1.position.x, Player.position.y);
        Prince.position = new Vector3(PrinceCheckpoint1.position.x, Prince.position.y);
        Prince.GetComponent<SpriteRenderer>().flipX = true;
        await DialogueManager.Instance.FadeOutEffect();

        await DialogueManager.Instance.DisplayDialogue(cave2);
        await UniTask.WaitForSeconds(2f);
        Coffer.GetComponent<Animator>().Play("Open");
        await UniTask.WaitForSeconds(2f);
        await DialogueManager.Instance.DisplayDialogue(cave3);

        Image foreScreen = DialogueManager.Instance.Forescreen;
        foreScreen.color = Color.white;
        foreScreen.gameObject.SetActive(true);

        Prince.gameObject.SetActive(false);
        Dagon.gameObject.SetActive(true);

        CameraMovement.Instance.target = Dagon;

        await DialogueManager.Instance.TransitionAlpha(foreScreen, 0f);
        foreScreen.gameObject.SetActive(false);

        await DialogueManager.Instance.DisplayDialogue(cave4);

        CameraMovement.Instance.target = Player;
        Dagon.position = DagonCheckpoint2.position;

        await UniTask.WaitUntil(() => Player.position.x >= PlayerCheckpoint2.position.x);
        await DialogueManager.Instance.DisplayDialogue(cave5);

        foreScreen.color = Color.black;
        await DialogueManager.Instance.FadeInEffect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Cave(Boss)");
    }


    public async UniTask MoveToTarget(Transform target, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = target.position;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration; 
            t = Mathf.SmoothStep(0f, 1f, t); 

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            await UniTask.Yield(); 
        }
        
        transform.position = targetPosition;
    }
}
