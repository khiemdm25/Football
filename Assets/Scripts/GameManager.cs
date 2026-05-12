using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Buttons")]
    public Button autoKickButton;
    public Button restartButton;

    [Header("Kick UI")]
    public GameObject kickButton;
    public GameObject powerUI;
    public Slider powerSlider;
    private Tween kickTween;

    [Header("Goal Effects")]
    public GameObject evenGoals;

    private Ball[] balls;

    private Player player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        balls = FindObjectsOfType<Ball>();
        player = FindObjectOfType<Player>();

        autoKickButton.onClick.AddListener(AutoKick);
        restartButton.onClick.AddListener(Restart);

        ShowKickButton(false);
    }

    public void ShowKickButton(bool show)
    {
        if (show)
        {
            powerUI.SetActive(true);
            kickButton.SetActive(true);

            kickButton.transform.localScale = Vector3.zero;

            kickButton.transform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    kickTween = kickButton.transform
                        .DOScale(1.1f, 0.5f)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                });
        }
        else
        {
            kickTween?.Kill();
            powerUI.SetActive(false);
            kickButton.SetActive(false);
        }
    }
    public void KickCurrentBall()
    {
        if (Ball.currentBall != null)
        {
            Ball.currentBall.KickToNearestGoal( powerSlider.value );
        }
    }

    public void AutoKick()
    {
        Ball farthestBall = FindFarthestBall();

        if (farthestBall != null)
        {
            farthestBall.AutoKick();
        }
    }

    Ball FindFarthestBall()
    {
        Ball farthest = null;
        float maxDistance = 0f;

        foreach (Ball ball in balls)
        {
            float dist = Vector3.Distance(ball.transform.position, player.transform.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                farthest = ball;
            }
        }

        return farthest;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}