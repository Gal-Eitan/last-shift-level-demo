
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasePatient : MonoBehaviour
{
    [Header("Patient Requests")]
    [SerializeField] private List<GameObject> requestableItems = new();

    [Header("Request Timer")]
    [SerializeField, Min(0.1f)] private float minRequestInterval = 5f;
    [SerializeField, Min(0.1f)] private float maxRequestInterval = 15f;

    [Header("Events")]
    public UnityEvent onRequestCreated;
    public UnityEvent onRequestFulfilled;

    public GameObject CurrentRequest { get; private set; }
    public bool HasActiveRequest => CurrentRequest != null;

    private float requestTimer;

    private void Start()
    {
        ResetRequestTimer();
    }

    private void Update()
    {
        // Don't generate another request while one is active.
        if (HasActiveRequest)
            return;

        requestTimer -= Time.deltaTime;

        if (requestTimer <= 0f)
        {
            GenerateRequest();
        }
    }

    private void GenerateRequest()
    {
        if (requestableItems.Count == 0)
        {
            Debug.LogWarning(
                $"{gameObject.name} has no requestable items!"
            );

            ResetRequestTimer();
            return;
        }

        int index = Random.Range(0, requestableItems.Count);

        CurrentRequest = requestableItems[index];

        if (CurrentRequest == null)
        {
            ResetRequestTimer();
            return;
        }

        Debug.Log(
            $"{gameObject.name} requests {CurrentRequest.name}"
        );

        onRequestCreated?.Invoke();
    }

    public bool FulfillRequest(GameObject deliveredItem)
    {
        if (!HasActiveRequest)
            return false;

        if (deliveredItem != CurrentRequest)
            return false;

        Debug.Log(
            $"{gameObject.name} received {deliveredItem.name}"
        );

        CurrentRequest = null;

        onRequestFulfilled?.Invoke();

        ResetRequestTimer();

        return true;
    }

    private void ResetRequestTimer()
    {
        float min = Mathf.Max(0.1f, minRequestInterval);
        float max = Mathf.Max(min, maxRequestInterval);

        requestTimer = Random.Range(min, max);
    }

    private void OnValidate()
    {
        minRequestInterval = Mathf.Max(0.1f, minRequestInterval);
        maxRequestInterval = Mathf.Max(
            minRequestInterval,
            maxRequestInterval
        );
    }
}
