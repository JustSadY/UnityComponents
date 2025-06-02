using UnityEngine;
using UnityEngine.Serialization;


public class AcceleratingGravity2D : MonoBehaviour
{
    [Tooltip("Maximum gravity scale that can be reached")] [FormerlySerializedAs("MaxGravityScale")] [SerializeField]
    private float maxGravityScale = 5f;

    [Header("Velocity-Based Acceleration")]
    [Tooltip("Falling velocity threshold to start accelerating gravity")]
    [FormerlySerializedAs("AccelerationStartVelocity")]
    [SerializeField]
    private float accelerationStartVelocity = -2f;

    [Tooltip("How much gravity increases per unit of falling velocity")]
    [FormerlySerializedAs("GravityPerVelocityUnit")]
    [SerializeField]
    private float gravityPerVelocityUnit = 0.2f;

    [Tooltip("Maximum falling velocity (terminal velocity)")] [FormerlySerializedAs("MaxFallVelocity")] [SerializeField]
    private float maxFallVelocity = -25f;

    [Header("Reset Conditions")]
    [Tooltip("Velocity threshold to reset gravity (when moving up or slowing down)")]
    [FormerlySerializedAs("ResetVelocityThreshold")]
    [SerializeField]
    private float resetVelocityThreshold = -0.1f;


    // Components
    private Rigidbody2D _rigidbody;

    // State
    private float _currentGravityScale;
    private float _defaultGravityScale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _defaultGravityScale = _rigidbody.gravityScale;
        _currentGravityScale = _defaultGravityScale;
    }

    private void Update()
    {
        UpdateGravityBasedOnVelocity();
    }

    private void UpdateGravityBasedOnVelocity()
    {
        float verticalVelocity = _rigidbody.linearVelocity.y;

        if (verticalVelocity > resetVelocityThreshold)
        {
            ResetGravity();
            return;
        }


        if (verticalVelocity <= accelerationStartVelocity)
        {
            float excessVelocity = Mathf.Abs(verticalVelocity - accelerationStartVelocity);

            float velocityBasedGravity = _defaultGravityScale + (excessVelocity * gravityPerVelocityUnit);

            _currentGravityScale = Mathf.Min(velocityBasedGravity, maxGravityScale);
        }
        else
        {
            _currentGravityScale = _defaultGravityScale;
        }

        _rigidbody.gravityScale = _currentGravityScale;

        if (verticalVelocity < maxFallVelocity)
        {
            Vector2 velocity = _rigidbody.linearVelocity;
            velocity.y = maxFallVelocity;
            _rigidbody.linearVelocity = velocity;
        }
    }

    private void ResetGravity()
    {
        _currentGravityScale = _defaultGravityScale;
        _rigidbody.gravityScale = _currentGravityScale;
    }
}