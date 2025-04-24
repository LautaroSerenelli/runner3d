using TMPro;
using UnityEngine;

public class ScoreMultiplierManager : MonoBehaviour
{
    public static ScoreMultiplierManager Instance;

    private float currentMultiplier = 1f;
    private float timer = 0f;

    [SerializeField] private TextMeshProUGUI multiplierText;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (multiplierText != null)
        {
            multiplierText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            multiplierText.text = $"X{currentMultiplier}";

            if (timer <= 0f)
            {
                currentMultiplier = 1f;

                multiplierText.gameObject.SetActive(false);
            }
        }
    }

    public void ApplyMultiplier(float multiplier, float duration)
    {
        currentMultiplier *= multiplier;
        timer = duration;

        multiplierText.text = $"X{currentMultiplier}";
        multiplierText.gameObject.SetActive(true);
    }

    public float GetMultiplier()
    {
        return currentMultiplier;
    }
}