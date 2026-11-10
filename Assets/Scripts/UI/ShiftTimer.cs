
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class ShiftTimer : MonoBehaviour
{
    [Header("Shift Settings")]
    [SerializeField] private string shiftDuration = "8:00";

    [Tooltip("In-game minutes per real-world second")]
    [SerializeField, Min(0.01f)]
    private float timeScale = 1f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Events")]
    public UnityEvent onShiftEnd;

    private float remainingMinutes;
    private bool isRunning = false;

    private void Start()
    {
        string[] parts = shiftDuration.Split(':');

        if (parts.Length != 2 ||
            !int.TryParse(parts[0], out int hours) ||
            !int.TryParse(parts[1], out int minutes) ||
            hours < 0 || minutes < 0 || minutes >= 60)
        {
            Debug.LogError("Invalid shift duration! Use HH:MM.");
            enabled = false;
            return;
        }

        remainingMinutes = hours * 60 + minutes;
        isRunning = true;

        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        remainingMinutes -= Time.deltaTime * timeScale;

        if (remainingMinutes <= 0f)
        {
            remainingMinutes = 0f;
            isRunning = false;

            UpdateTimerDisplay();
            Debug.Log("Shift ended! Invoking UnityEvent.");
            onShiftEnd?.Invoke();
            return;
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        // Round up so the displayed minute changes
        // only after a full in-game minute has passed.
        int totalMinutes = Mathf.CeilToInt(remainingMinutes);

        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        timerText.text = $"{hours:00}:{minutes:00}";
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        if (remainingMinutes > 0)
            isRunning = true;
    }
}
