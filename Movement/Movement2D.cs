using UnityEngine;
using UnityEngine.Serialization;

public class Movement2D : MonoBehaviour
{
    // Components
    private Rigidbody2D _rigidbody;

    // Settings
    [Header("Input Settings")]
    [Tooltip("Input deadzone threshold: minimum input value to detect movement")]
    [FormerlySerializedAs("InputThreshold")]
    [SerializeField]
    private float inputThreshold = 0.1f;

    [Header("Movement Settings")]
    [Tooltip("Maximum walking speed (units per second)")]
    [FormerlySerializedAs("MaxWalkSpeed")]
    [SerializeField]
    private float maxWalkSpeed = 12.5f;

    [Header("Ground Movement")]
    [Tooltip("Acceleration rate while grounded")]
    [FormerlySerializedAs("GroundAcceleration")]
    [SerializeField]
    private float groundAcceleration = 5f;

    [Tooltip("Deceleration rate while grounded")] [FormerlySerializedAs("GroundDeceleration")] [SerializeField]
    private float groundDeceleration = 20f;

    [Header("Air Movement")]
    [Tooltip("Acceleration rate while in air")]
    [FormerlySerializedAs("AirAcceleration")]
    [SerializeField]
    private float airAcceleration = 5f;

    [Tooltip("Deceleration rate while in air")] [FormerlySerializedAs("AirDeceleration")] [SerializeField]
    private float airDeceleration = 5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void MoveWithParams(float acceleration, float deceleration, float input)
    {
        Vector2 currentVelocity = _rigidbody.linearVelocity;
        if (Mathf.Abs(input) > inputThreshold)
        {
            float newSpeed = Mathf.Lerp(currentVelocity.x, maxWalkSpeed * input, acceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = new Vector2(newSpeed, currentVelocity.y);
        }
        else
        {
            float newSpeed = Mathf.Lerp(currentVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = new Vector2(newSpeed, currentVelocity.y);
        }
    }

    public void Move(float input, MovementType movementType = MovementType.Ground)
    {
        if (!_rigidbody) return;
        switch ((byte)movementType)
        {
            case 0:
                MoveWithParams(airAcceleration, airDeceleration, input);
                break;
            case 1:
                MoveWithParams(groundAcceleration, groundDeceleration, input);
                break;
            default:
                MoveWithParams(groundAcceleration, groundDeceleration, input);
                break;
        }
    }
}