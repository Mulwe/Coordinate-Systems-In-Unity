using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private NpcController _npc;

    [Header("Reset Game:")]
    [SerializeField] private Key _resetKey = Key.R;

    [Header("Win Condition")]
    [Header("Distance from NPC")]
    [SerializeField] private float _npcZoneRadius;

    [Header("Timers")]
    [SerializeField] private float _timeOutsideNpcZone;
    [SerializeField] private float _timeInsideNpcZone;

    private bool _isCooldownTimerActive;
    private float _cooldownTimer;
    private bool _previousPlayerState;

    [Tooltip("GameState")]
    private bool _gameStopped;

    private void OnValidate()
    {
        if (_player == null)
            Debug.LogError($"{nameof(_player)} is not assigned");

        if (_npc == null)
            Debug.LogError($"{nameof(_npc)} is not assigned");
    }

    private void Awake()
    {
        Initialize();
    }

    private bool IsOutOfZone() =>
        (_npc.transform.position - _player.transform.position).magnitude > _npcZoneRadius;

    private void Initialize()
    {
        ResetTimer();
        _isCooldownTimerActive = true;
        _gameStopped = false;
        _previousPlayerState = false;
    }

    private void Update()
    {
        if (Keyboard.current[_resetKey].wasPressedThisFrame)
        {
            Debug.Log($"You pressed [{_resetKey}]. Resetting game...");

            //reset gameManager
            Initialize();

            //reset player
            _player.Initialize();

            //reset npc
            _npc.Initialize();
        }

        if (_gameStopped)
            return;

        RunGame();
    }

    private void RunGame()
    {
        if (_isCooldownTimerActive)
            _cooldownTimer += Time.deltaTime;

        bool isPlayerOutside = IsOutOfZone();

        if (isPlayerOutside)
        {
            if (IsLose())
            {
                _gameStopped = true;
                GameOver();
                return;
            }
        }

        if (isPlayerOutside == false)
        {
            if (IsWon())
            {
                _gameStopped = true;
                GameWon();
                return;
            }
        }

        if (isPlayerOutside != _previousPlayerState)
            PlayerChangedZone(isPlayerOutside);
    }

    private void PlayerChangedZone(bool currentPlayerState)
    {
        ResetTimer();
        _previousPlayerState = currentPlayerState;
    }

    private bool IsLose() => _cooldownTimer >= _timeOutsideNpcZone;

    private bool IsWon() => _cooldownTimer >= _timeInsideNpcZone;

    private void ResetTimer() =>
        _cooldownTimer = 0f;

    private void GameOver() =>
        Debug.Log("You was too far away from NPC. <color=red>Game Over.</color>");

    private void GameWon() =>
        Debug.Log("Congratulations!. <color=green>You Won.</color>");

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_npc.transform.position, _npcZoneRadius);
    }
}