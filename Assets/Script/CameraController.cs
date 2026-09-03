using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Targeting")]
    public Transform player;

    [Header("Deadzone Settings")]
    public Vector2 deadzoneSize = new Vector2(3f, 2f);
    private Vector2 focusPosition;

    [Header("Mouse Look Settings")]
    [Range(0f, 1f)]
    public float mouseInfluence = 0.3f;
    public float maxMouseOffset = 4f;

    [Header("Smoothing")]
    public float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    private Vector3 unShakenPosition;

    [Header("Screen Shake (Juice)")]
    private float shakeTimer = 0f;
    private float currentShakeMagnitude = 0f;

    public static CameraController Instance;

    private Camera cam;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        unShakenPosition = transform.position;
        if (player != null) focusPosition = player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        UpdateFocusPosition();

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 mouseOffset = (mousePos - (Vector3)focusPosition) * mouseInfluence;
        mouseOffset = Vector2.ClampMagnitude(mouseOffset, maxMouseOffset);

        Vector3 desiredPosition = new Vector3(
            focusPosition.x + mouseOffset.x,
            focusPosition.y + mouseOffset.y,
            unShakenPosition.z
        );

        unShakenPosition = Vector3.SmoothDamp(unShakenPosition, desiredPosition, ref velocity, smoothTime);

        Vector3 finalPosition = unShakenPosition;
        if (shakeTimer > 0)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * currentShakeMagnitude;
            finalPosition += new Vector3(shakeOffset.x, shakeOffset.y, 0f);

            shakeTimer -= Time.unscaledDeltaTime;
        }

        transform.position = finalPosition;
    }

    public void TriggerShootJerk()
    {
        shakeTimer = 0.04f;
        currentShakeMagnitude = 0.08f;
    }

    public void TriggerDeathShake()
    {
        shakeTimer = 0.08f;
        currentShakeMagnitude = 0.1f;
    }

    public void TriggerCustomShake(float magnitude, float duration)
    {
        shakeTimer = duration;
        currentShakeMagnitude = magnitude;
    }

    void UpdateFocusPosition()
    {
        float left = focusPosition.x - deadzoneSize.x / 2f;
        float right = focusPosition.x + deadzoneSize.x / 2f;
        float bottom = focusPosition.y - deadzoneSize.y / 2f;
        float top = focusPosition.y + deadzoneSize.y / 2f;

        if (player.position.x < left) focusPosition.x -= left - player.position.x;
        else if (player.position.x > right) focusPosition.x += player.position.x - right;

        if (player.position.y < bottom) focusPosition.y -= bottom - player.position.y;
        else if (player.position.y > top) focusPosition.y += player.position.y - top;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireCube(focusPosition, deadzoneSize);
    }
}