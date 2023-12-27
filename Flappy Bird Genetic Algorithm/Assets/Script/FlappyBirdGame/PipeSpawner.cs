using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public static PipeSpawner Instance { get; private set; }

    [SerializeField] private float _minY = -3f;
    [SerializeField] private float _maxY = 3f;
    [SerializeField] private GameObject _pipe;
    [SerializeField] private float _pipeOffset = 2f;

    private Vector3 _lastPipePosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SpawnPipe();
    }

    private void Update()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        if (pipes.Length < 5)
            SpawnPipe();
    }

    private void SpawnPipe()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        if (pipes.Length > 0)
        {
            _lastPipePosition = pipes[pipes.Length - 1].transform.position;
        }
        float randomHeight = Random.Range(_minY, _maxY);
        Vector3 spawnPos = _lastPipePosition + new Vector3(_pipeOffset, randomHeight);
        spawnPos.y = randomHeight;
        GameObject pipe = Instantiate(_pipe, spawnPos, Quaternion.identity);
    }
    public void ResetLevel()
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject pipe in pipes)
        {
            Destroy(pipe);
        }
        _lastPipePosition = new Vector3(0, 0, 0);
    }
}
