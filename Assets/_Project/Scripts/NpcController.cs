using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField, Range(0f, 30f)] private float _speed = 10.5f;
    [SerializeField] private float _rotationSpeed = 400f;
    [SerializeField] private Transform _target;

    private Transform _virtualTarget;
    private float _minDistanceToTarget = 0.2f;
    private bool _isArrived;
    public bool IsArrived => _isArrived;

    private Vector3 _position;

    public void Initialize()
    {
        _isArrived = false;
        transform.position = _position;
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
        _isArrived = false;
    }

    public void SetTarget(Vector3 newTarget)
    {
        _virtualTarget.transform.position = newTarget;
        _target = _virtualTarget;
        _isArrived = false;
    }

    private void Awake()
    {
        _isArrived = false;
        _position = transform.position;

        GameObject temp = new GameObject("VirtualTarget");
        _virtualTarget = temp.transform;
    }

    private void Update() =>
        MoveToTarget();

    private void OnTargetLost()
    {
        _target = this.transform;
        OnTargetReached();
    }

    private void OnTargetReached() => _isArrived = true;

    private void MoveToTarget()
    {
        if (_target == null)
            OnTargetLost();

        if (_isArrived)
            return;

        Vector3 direction = _target.position - transform.position;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (direction.magnitude <= _minDistanceToTarget)
        {
            OnTargetReached();
            return;
        }

        Vector3 normalizedDirection = direction.normalized;

        transform.MoveOn(normalizedDirection, _speed);

        transform.FlipRotationLook(normalizedDirection, _rotationSpeed);
    }

    private void MoveSmoothly(Vector3 targetPosition, float smoothSpeed) =>
        this.transform.position =
        Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * smoothSpeed);
}