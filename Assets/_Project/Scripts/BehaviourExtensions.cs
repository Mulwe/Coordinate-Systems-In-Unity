using UnityEngine;

public static class BehaviourExtensions
{
    // static class with extensions for objects with same methods
    public static void FlipRotationLook(this Transform target, Vector3 direction, float speed)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float step = speed * Time.deltaTime;
        target.rotation = Quaternion.RotateTowards(target.rotation, lookRotation, step);
    }

    public static void MoveOn(this Transform target, Vector3 direction, float speed) =>
      target.Translate(speed * Time.deltaTime * direction, Space.World);
}