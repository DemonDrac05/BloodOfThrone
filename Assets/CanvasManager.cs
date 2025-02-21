using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Unity.Hierarchy;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 2f;
    [SerializeField] private GameObject Hatake;

    [SerializeField] private Transform Player;
    [SerializeField] private GameObject MoveIntroCanvas;
    [SerializeField] private GameObject JumpIntroCanvas;
    [SerializeField] private GameObject AttackIntroCanvas;
    [SerializeField] private GameObject BlockIntroCanvas;
    [SerializeField] private GameObject InteractIntroCanvas;

    [SerializeField] private Transform MovePoint;
    [SerializeField] private Transform JumpPoint1;
    [SerializeField] private Transform JumpPoint2;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private Transform BlockPoint;
    [SerializeField] private Transform InteractPoint;

    private void Update()
    {
        float playerPosX = Player.transform.position.x;

        float movePointX = MovePoint.transform.position.x;
        float jumpPoint1X = JumpPoint1.transform.position.x;
        float jumpPoint2X = JumpPoint2.transform.position.x;
        float attackPointX = AttackPoint.transform.position.x;
        float blockPointX = BlockPoint.transform.position.x;
        float interactPointX = InteractPoint.transform.position.x;

        if (playerPosX >= movePointX) MoveIntroCanvas.SetActive(false);

        if (playerPosX >= jumpPoint1X - 1f) JumpIntroCanvas.SetActive(true);
        if (playerPosX >= jumpPoint1X + 1f) JumpIntroCanvas.SetActive(false);

        if (playerPosX >= jumpPoint2X - 1f) JumpIntroCanvas.SetActive(true);
        if (playerPosX >= jumpPoint2X + 1f) JumpIntroCanvas.SetActive(false);

        if (Hatake.GetComponent<EnemyHealth>().IsDead() || playerPosX >= attackPointX + 2f)
        {
            AttackIntroCanvas.SetActive(false);
        }
        else
        {
            if (playerPosX >= attackPointX - 1f) AttackIntroCanvas.SetActive(true);
        }

        if (playerPosX >= blockPointX + 1f) BlockIntroCanvas.SetActive(true);
        if (playerPosX >= blockPointX + 3f) BlockIntroCanvas.SetActive(false);

        if (playerPosX >= interactPointX - 1f)
        {
            InteractIntroCanvas.SetActive(true);
        }
    }

    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Image FadeScreen;



    public async UniTask RespawnEffect(Player player, Transform spawnPoint)
    {
        player.SetFlip(false);

        await TransitionAlpha(FadeScreen, 1f);

        player.transform.position = spawnPoint.position;

        await TransitionAlpha(FadeScreen, 0f);

        player.SetFlip(true);
    }


    async UniTask TransitionAlpha(Image targetImage, float targetAlpha)
    {
        float progress = 0f;
        float startAlpha = targetImage.color.a;

        while (progress < 1f)
        {
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, progress);
            Color newColor = targetImage.color;
            newColor.a = currentAlpha;
            targetImage.color = newColor;

            progress += Time.deltaTime / transitionDuration;
            await UniTask.Yield();
        }

        Color finalColor = targetImage.color;
        finalColor.a = targetAlpha;
        targetImage.color = finalColor;
    }
}
