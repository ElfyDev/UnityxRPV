using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SoccerManager : MonoBehaviour
{
    [SerializeField] private GameObject goalA;
    [SerializeField] private GameObject goalB;

    [SerializeField] private float scoreA = 0;
    [SerializeField] private float scoreB = 0;

    [SerializeField] private TextMeshProUGUI ScoreLabel;

    [SerializeField] private Vector3 originPos;

    private int waitTime = 2;

    private bool scorable = true;

    [SerializeField] private GoalGateUnlocked gateUnlockedGoal;
    [SerializeField] private TextMeshProUGUI totalLabel;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreLabel.text = $"{scoreA} - {scoreB}";
        totalLabel.text = $"{scoreA + scoreB}/3";

        if (scoreA + scoreB >= 3)
        {
            if (gateUnlockedGoal != null)
            {
                gateUnlockedGoal.OpenGate();
            }
        }

        if (this.transform.localPosition.y < -0.47)
        {
            this.transform.position = originPos;
        }
    }

    private async Task OnCollisionEnter(Collision collision)
    {
        if (scorable)
        {
            if (collision.gameObject == goalA)
            {
                scoreB++;
                ResetBall();
            }
            if (collision.gameObject == goalB)
            {
                scoreA++;
                ResetBall();
            }
        }

    }

    public async void ResetBall()
    {
        ScoreLabel.text = $"{scoreA} {scoreB}";

        scorable = false;
        await Task.Delay(waitTime * 1000);

        this.transform.localPosition = originPos;
        scorable = true;
    }
}
