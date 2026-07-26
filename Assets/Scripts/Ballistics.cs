using UnityEngine;

public static class Ballistics
{
    private const float Gravity = 9.81f;
    private const float ThrowSpeed = 15f;
    private const float ThrowSpeedSquared = ThrowSpeed * ThrowSpeed;

    public static float Solve(Vector3 start, Vector3 target)
    {
        var delta = target - start;

        var y = delta.y;
        delta.y = 0f;

        var x = delta.magnitude;

        if (x < 0.001f)
            return 0f;

        var discriminant =
            ThrowSpeedSquared * ThrowSpeedSquared -
            Gravity * (Gravity * x * x + 2f * y * ThrowSpeedSquared);

        if (discriminant < 0f)
            return 45f;

        var sqrt = Mathf.Sqrt(discriminant);

        var angle = Mathf.Atan(
            (ThrowSpeedSquared - sqrt) /
            (Gravity * x));

        return angle * Mathf.Rad2Deg;
    }

    public static Vector3 GetInitialVelocity(Vector3 start, Vector3 target, float angle)
    {
        var direction = target - start;
        direction.y = 0f;
        direction.Normalize();

        var radians = angle * Mathf.Deg2Rad;

        var horizontalSpeed = Mathf.Cos(radians) * ThrowSpeed;
        var verticalSpeed = Mathf.Sin(radians) * ThrowSpeed;

        return direction * horizontalSpeed + Vector3.up * verticalSpeed;
    }

    public static Vector3 Evaluate(
        Vector3 start,
        Vector3 initialVelocity,
        float t)
    {
        return start
               + initialVelocity * t
               + Vector3.down * (0.5f * Gravity * t * t);
    }

    public static Vector3 EvaluateVelocity(
        Vector3 initialVelocity,
        float t)
    {
        return initialVelocity + Vector3.down * Gravity * t;
    }
}