using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    [SerializeField] private Transform[] _waypointsArray;
    [SerializeField] private Queue<Vector3> _waypoints;

    [SerializeField] private NpcController _npc;
    [SerializeField] private float _delay = 1f;

    private Coroutine _routine;
    private WaitForSeconds _wait;

    /*
     * Enqueue - enter -> добавить элемент в очередь
     * Dequeue - 'delete' -> вытащить элемент из очереди
     */

    private void Awake()
    {
        _waypoints ??= new Queue<Vector3>();

        foreach (var point in _waypointsArray)
            _waypoints.Enqueue(point.position);

        if (_npc == null)
            Debug.LogError($"NpcController: not found");

        _wait = new WaitForSeconds(_delay);
    }

    private Vector3 GetNextWaypoint()
    {
        //чтобы очередь не заканчивалась, добавляю тот же путь в конец queue

        if (_waypoints.TryDequeue(out Vector3 nextWaypoint))
            _waypoints.Enqueue(nextWaypoint);

        return nextWaypoint;
    }

    private IEnumerator StartRouteManager(NpcController npc)
    {
        while (true)
        {
            if (npc.IsArrived)
            {
                Vector3 newWaypoint = GetNextWaypoint();
                npc.SetTarget(newWaypoint);
            }

            yield return _wait;
        }
    }

    private void OnEnable()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(StartRouteManager(_npc));
    }

    private void OnDisable()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = null;
    }
}