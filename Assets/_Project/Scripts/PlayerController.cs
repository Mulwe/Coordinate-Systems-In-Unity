using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(1f, 15f)] private float _speed;
    private Transform _player;

    private Vector3 _defaultPosition;

    public void Initialize() =>
        _player.transform.position = _defaultPosition;

    private void Awake()
    {
        _player = GetComponent<Transform>();
        _defaultPosition = transform.position;
    }

    private void Update() =>
        ProcessInput();

    private void ProcessInput()
    {
        Vector3 direction = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            direction += Vector3.forward;
        if (Keyboard.current.sKey.isPressed)
            direction += Vector3.back;
        if (Keyboard.current.aKey.isPressed)
            direction += Vector3.left;
        if (Keyboard.current.dKey.isPressed)
            direction += Vector3.right;

        if (direction == Vector3.zero)
            return;

        Vector3 normalizedDirection = direction.normalized;

        FlipLook(normalizedDirection);
        MoveOn(normalizedDirection);
    }

    private void FlipLook(Vector3 direction)
    {
        if (direction != Vector3.zero)
            _player.rotation = Quaternion.LookRotation(direction);
    }

    private void MoveOn(Vector3 direction) =>
        _player.transform.Translate(direction * _speed * Time.deltaTime, Space.World);
}