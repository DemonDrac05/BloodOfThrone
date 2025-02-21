using GifImporter;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image MainMenuBackground;
    [SerializeField] private Image MainMenuForeground;
    [SerializeField] private Image TextForeground;

    [SerializeField] private GameObject NameOfGame;
    [SerializeField] private Gif NameGif;
    private bool _gifStarted = false;

    [SerializeField] private Button PlayButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private GameObject GameLogo;

    [SerializeField] private float transitionDuration;

    [TextArea][SerializeField] private string IntroContent1;
    [SerializeField] private TextMeshProUGUI IntroContentText1;
    [TextArea][SerializeField] private string IntroContent2;
    [SerializeField] private TextMeshProUGUI IntroContentText2;

    private TypewriterEffect TypewriterEffect1;
    private TypewriterEffect TypewriterEffect2;

    [SerializeField] private float contentDelay;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        TypewriterEffect1 = IntroContentText1.GetComponent<TypewriterEffect>();
        TypewriterEffect2 = IntroContentText2.GetComponent<TypewriterEffect>();
        TypewriterEffect1.Initialize(IntroContentText1);
        TypewriterEffect2.Initialize(IntroContentText2);
    }

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(async () => await LoadGame());
        QuitButton.onClick.AddListener(async () => await QuitGame());
        MainMenuForeground.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(async () => await LoadGame());
        QuitButton.onClick.RemoveListener(async () => await QuitGame());
        MainMenuForeground.gameObject.SetActive(false);
    }

    private async UniTask LoadGame()
    {
        TextForeground.gameObject.SetActive(true);
        MainMenuForeground.gameObject.SetActive(true);
        await TransitionAlpha(MainMenuForeground, 1f);

        IntroContentText1.gameObject.SetActive(true);
        await TypewriterEffect1.StartTyping(IntroContent1);
        await UniTask.WaitForSeconds(contentDelay);

        await TransitionAlpha(TextForeground, 1f);
        IntroContentText1.gameObject.SetActive(false);
        await TransitionAlpha(TextForeground, 0f);

        IntroContentText2.gameObject.SetActive(true);
        await TypewriterEffect2.StartTyping(IntroContent2);
        await UniTask.WaitForSeconds(contentDelay);

        await TransitionAlpha(TextForeground, 1f);
        IntroContentText2.gameObject.SetActive(false);
        SceneManager.LoadScene("Forest");
    }

    private async UniTask QuitGame()
    {
        MainMenuForeground.gameObject.SetActive(true);
        await TransitionAlpha(MainMenuForeground, 1f);
        Application.Quit();
    }

    private async void Start()
    {
        await MainMenuStartProcess();
    }

    async UniTask MainMenuStartProcess()
    {
        await UniTask.WaitForSeconds(1f);

        Image NameOfGameImage = NameOfGame.GetComponent<Image>();

        await TransitionAlpha(NameOfGameImage, 1f);

        await TransitionColor(MainMenuBackground, Color.white);

        Image playButtonImg = PlayButton.GetComponent<Image>();
        Image quitButtonImg = QuitButton.GetComponent<Image>();

        await UniTask.WhenAll(TransitionAlpha(playButtonImg, 1f), TransitionAlpha(quitButtonImg, 1f));

        await TransitionAlpha(GameLogo.GetComponent<Image>(), 1f);
    }

    async UniTask NameOfGameAnimated(Image image)
    {
        if (NameOfGame.GetComponent<GifPlayer>() == null)
        {
            GifPlayer gifPlayer = NameOfGame.AddComponent<GifPlayer>();
            gifPlayer.Gif = NameGif;
        }

        GifPlayer nameGifPlayer = NameOfGame.GetComponent<GifPlayer>();
        nameGifPlayer.Activate();

        await UniTask.WaitUntil(() =>
            nameGifPlayer != null &&
            nameGifPlayer.Gif != null &&
            nameGifPlayer.Gif.Frames != null &&
            nameGifPlayer._index == nameGifPlayer.Gif.Frames.Count - 1
        );

        nameGifPlayer.loop = false;
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

            if (targetImage == NameOfGame.GetComponent<Image>() && !_gifStarted && currentAlpha >= (50f / 255f))
            {
                _gifStarted = true; 
                _ = NameOfGameAnimated(targetImage);
            }

            progress += Time.deltaTime / transitionDuration;
            await UniTask.Yield();
        }

        Color finalColor = targetImage.color;
        finalColor.a = targetAlpha;
        targetImage.color = finalColor;
    }


    async UniTask TransitionColor(Image targetImage, Color targetColor)
    {
        float progress = 0f;
        Color startColor = targetImage.color;

        while (progress < 1f)
        {
            Color currentColor = Color.Lerp(startColor, targetColor, progress);
            targetImage.color = currentColor; 

            progress += Time.deltaTime / transitionDuration;
            await UniTask.Yield();
        }

        targetImage.color = targetColor;
    }

}
