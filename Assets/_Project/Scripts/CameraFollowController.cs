using Mono.Cecil.Cil;
using Unity.Mathematics.Geometry;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed;

    private Camera _camera;
    private float _threshold = 0.05f;
    private Transform _cam => _camera.transform;
    private bool _isFollowing;

    public void StartFollowTarget() => _isFollowing = true;

    public void StopFollowTarget() => _isFollowing = false;

    private void Awake()
    {
        _camera = Camera.main;

        if (_camera == null)
            Debug.LogError($"{nameof(_camera)}: Is not init");

        if (_target == null)
            Debug.LogError($"{nameof(_target)}: Is not found");

        _isFollowing = true;
    }

    private void LateUpdate()
    {
        if (_isFollowing && IsCameraAlignedOverTarget() == false)
            FollowTarget(_target);
    }

    private bool IsCameraAlignedOverTarget()
    {
        return
             Mathf.Abs(_cam.position.x - _target.position.x) < _threshold
             && Mathf.Abs(_cam.position.z - _target.position.z) < _threshold;
    }

    private void FollowTarget(Transform target)
    {
        if (target == null)
            return;

        Vector3 newPosition = new(target.position.x, _cam.position.y, target.position.z);

        _cam.position = Vector3.Lerp(_cam.position, newPosition, Time.deltaTime * _smoothSpeed);
    }
}