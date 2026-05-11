using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string scoreFormat = "Score: {0}";

    public void SetScore(int score)
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = string.Format(scoreFormat, score);
    }
}
