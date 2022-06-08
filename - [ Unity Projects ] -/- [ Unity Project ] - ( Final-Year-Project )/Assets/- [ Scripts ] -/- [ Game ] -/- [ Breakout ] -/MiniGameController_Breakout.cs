using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// The control script for the mini-game Breakout
// This is used to spawn in all aspects of the mini-game while also maintaining game state throughout play, such as when the board is clear.
public class MiniGameController_Breakout : MonoBehaviour
{
    [SerializeField] private Transform _blocksFolder;
    
    [Space]
    [SerializeField] private GameObject _beamPrefab;
    [SerializeField] private GameObject _emitterPrefab, _doubleEmitterPrefab;
    [SerializeField] private GameObject _paddlePrefab;
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _focusRockPrefab;
    
    private readonly Dictionary<int, GameObject> _blocks = new Dictionary<int, GameObject>();
    private GameObject _leftBeam, _rightBeam, _topBeam;
    private GameObject _bottomLeftEmitter, _topLeftEmitter, _bottomRightEmitter, _topRightEmitter;
    private GameObject _focusRock;
    private GameObject _paddle;
    private GameObject _ball;
    
    [SerializeField] private Vector3 _focusTabletSpawnLocation;
    
    [Space]
    [SerializeField] private GameObject _completionTextPrefab;
    [SerializeField] private GameObject _failedTextPrefab;
    [SerializeField] private Vector3 _completionTextSpawnLocation;
    [SerializeField] private GameObject _returnTabletPrefab;
    [SerializeField] private Vector3 _returnTabletSpawnLocation;
    
    [Space] 
    [SerializeField] private List<int> _breakoutLayers;
    [SerializeField] private Bounds _breakoutBounds;

    public Bounds Bounds => _breakoutBounds;
    [SerializeField] private float _yPositionOfPaddle;
    [SerializeField] private float _yPositionOfBall;
    [SerializeField] private float _curve;
    public float Curve => _curve;

    private Transform _transform;


    
    
    
    private void Awake()
    {
        _transform = transform;
    }

    
    // Calls the spawner functions for all needed game objects, then calls a transition for the players current state.
    private void Start()
    {
        SpawnFocusRock();
        SpawnPaddle();
        SpawnBlocks();
        SpawnBall();

        SpawnDecor();
        
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.SetControlState = player.ControlStateBreakout;
        }
    }

    // Spawns in the focus rock, based on the prefab.
    private void SpawnFocusRock()
    {
        _focusRock = Instantiate(_focusRockPrefab, _focusTabletSpawnLocation, Quaternion.Euler(new Vector3(-20, 180, 0)), _transform);
    }
    
    // Spawns in the particle spawners, and the game objects for decorating the game board.
    private void SpawnDecor()
    {
        Beam beamComponent = null;
        Vector3 min = _breakoutBounds.min, max = _breakoutBounds.max, centre = _breakoutBounds.center;

        float offset = 1.125f;
        float seaLevelOffset = 2.1f;
        
        // Spawns a particle system in which the flow of particles moves from point Start, through point Middle and ends at point End.
        _leftBeam = Instantiate(_beamPrefab, min, Quaternion.identity, _transform);
        beamComponent = _leftBeam.GetComponent<Beam>();
        beamComponent.Start = new Vector3(min.x, seaLevelOffset + min.y + offset, centre.z * Mathf.Cos(min.x * _curve));
        beamComponent.Middle = new Vector3(min.x, centre.y, centre.z * Mathf.Cos(min.x * _curve));
        beamComponent.End = new Vector3(min.x, max.y - offset, centre.z * Mathf.Cos(min.x * _curve));

        // Spawns a particle system in which the flow of particles moves from point Start, through point Middle and ends at point End.
        _rightBeam = Instantiate(_beamPrefab, max, Quaternion.identity, _transform);
        beamComponent = _rightBeam.GetComponent<Beam>();
        beamComponent.Start = new Vector3(max.x, seaLevelOffset + min.y + offset, centre.z * Mathf.Cos(max.x * _curve));
        beamComponent.Middle = new Vector3(max.x, centre.y, centre.z * Mathf.Cos(max.x * _curve));
        beamComponent.End = new Vector3(max.x, max.y - offset, centre.z * Mathf.Cos(max.x * _curve));
        
        // Spawns a particle system in which the flow of particles moves from point Start, through point Middle and ends at point End.
        _topBeam = Instantiate(_beamPrefab, max, Quaternion.identity, _transform);
        beamComponent = _topBeam.GetComponent<Beam>();
        beamComponent.Start = new Vector3(min.x + offset, max.y, centre.z * Mathf.Cos(min.x * _curve));
        beamComponent.Middle = new Vector3(centre.x, max.y, centre.z * Mathf.Cos(centre.x * _curve));
        beamComponent.End = new Vector3(max.x - offset, max.y, centre.z * Mathf.Cos(max.x * _curve));
        
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;

        spawnPosition = new Vector3(min.x, seaLevelOffset + min.y, centre.z * Mathf.Cos(min.x * _curve));
        spawnRotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(min.x * _curve) * 40.0f, 0));
        _bottomLeftEmitter = Instantiate(_emitterPrefab, spawnPosition, spawnRotation, _transform);
        
        spawnPosition = new Vector3(min.x, max.y, centre.z * Mathf.Cos(min.x * _curve));
        spawnRotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(min.x * _curve) * 40.0f, 0)) * Quaternion.Euler(0, 0, 225);
        _topLeftEmitter = Instantiate(_doubleEmitterPrefab, spawnPosition, spawnRotation, _transform);
        
        
        spawnPosition = new Vector3(max.x, seaLevelOffset + min.y, centre.z * Mathf.Cos(max.x * _curve));
        spawnRotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(max.x * _curve) * 40.0f, 0));
        _bottomRightEmitter = Instantiate(_emitterPrefab, spawnPosition, spawnRotation, _transform);
        
        spawnPosition = new Vector3(max.x, max.y, centre.z * Mathf.Cos(max.x * _curve));
        spawnRotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(max.x * _curve) * 40.0f, 0)) * Quaternion.Euler(0, 0, -225);
        _topRightEmitter = Instantiate(_doubleEmitterPrefab, spawnPosition, spawnRotation, _transform);
    }

    // Spawns the ball game object for the mini-game.
    private void SpawnBall()
    {
        Vector3 position = new Vector3(0, _yPositionOfBall, _breakoutBounds.center.z);
        Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(-position), Vector3.up);

        _ball = Instantiate(_ballPrefab, position, rotation, _transform);
    }
    
    // Spawns the paddle game object for the mini-game.
    private void SpawnPaddle()
    {
        Vector3 position = new Vector3(0, _yPositionOfPaddle, _breakoutBounds.center.z);
        Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(-position), Vector3.up);

        _paddle = Instantiate(_paddlePrefab, position, rotation, _transform);
    }

    // Spawns the blocks on top of the game board.
    // This is set up using the input layers list, in which the game can spawn in any set of blocks on the board programmatically.
    // Spawning is also curved around the game boards curve value, this is to make the game board more visible.
    private void SpawnBlocks()
    {
        float stepDistance = 1.5f;
        for (int i = 0; i < _breakoutLayers.Count; i++)
        {
            float yPosition = _breakoutBounds.max.y - (stepDistance * (i + 1)) - 0.5f;
            for (int j = 0; j < _breakoutLayers[i]; j++)
            {
                float xPosition = (_breakoutBounds.min.x) + ((_breakoutBounds.max.x - _breakoutBounds.min.x) * (1.0f / (_breakoutLayers[i] + 1)) * (j + 1));

                Vector3 position = new Vector3(xPosition, yPosition, (_breakoutBounds.center.z * Mathf.Cos(xPosition * _curve)));
                Quaternion rotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(xPosition * _curve) * 40.0f, 0));

                GameObject block = Instantiate(_blockPrefab, position, rotation, _blocksFolder);
                _blocks.Add(block.GetInstanceID(), block);
            }
        }
    }

    
    // Calculates the nearest block from a given position, used by the BreakoutBall script for its auto-targeting cool down.
    // Iterates over each block currently active on the game board and stores its distance.
    public GameObject GetNearestBlock(Vector3 ballPosition)
    {
        GameObject nearestBlock = null;
        
        List<GameObject> blocks = _blocks.Values.ToList();
        if (blocks.Count > 0)
        {
            int closestBlock = 0;
            float closestDistance = Vector3.SqrMagnitude(ballPosition - blocks[0].transform.position);
            for (int i = 1; i < blocks.Count; i++)
            {
                float distance = Vector3.SqrMagnitude(ballPosition - blocks[i].transform.position);
                if (closestDistance > distance)
                {
                    closestBlock = i;
                    closestDistance = distance;
                }
            }

            nearestBlock = blocks[closestBlock];
        }

        return nearestBlock;
    }


    // Uses a block game objects InstanceID to remove it from the dictionary and destroy it.
    public void DestroyBlock(int instanceID)
    {
        if (_blocks.ContainsKey(instanceID))
        {
            Destroy(_blocks[instanceID]);
            _blocks.Remove(instanceID);
        }
        else
        {
            Debug.LogError($"There is no element with the ID: {instanceID}");
        }

        if (_blocks.Count == 0)
        {
            EndMiniGame(true);
        }
    }
    
    
    // Called when all blocks are destroyed or the ball has reached below the game bounds and resulted in the mini-game failure.
    // Destroys all game objects related to the game and then spawns in the main menu tablet and level results.
    public void EndMiniGame(bool winConditionMet)
    {
        Destroy(_ball);
        Destroy(_paddle);
        Destroy(_focusRock);
        
        foreach (GameObject block in _blocks.Values)
        {
            Destroy(block);
        }
        
        _blocks.Clear();

        Destroy(_topBeam);
        Destroy(_leftBeam);
        Destroy(_rightBeam);
        
        
        GameObject completionText = (winConditionMet)
            ? Instantiate(_completionTextPrefab, _completionTextSpawnLocation, Quaternion.Euler(new Vector3(0, 180, 0)), _transform)
            : Instantiate(_failedTextPrefab, _completionTextSpawnLocation, Quaternion.Euler(new Vector3(0, 180, 0)), _transform);
        
        GameObject returnTablet = Instantiate(_returnTabletPrefab, _returnTabletSpawnLocation, Quaternion.Euler(new Vector3(-20, 180, 0)), _transform);
        returnTablet.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
    
    
    // Draws the positions of all objects within the game, used to debug and design the level for playing.
    private void OnDrawGizmos()
    {
        DrawGizmoBreakoutBounds();
        DrawBlocks();
        DrawPaddle();
        DrawBall();
    }

    // Draws the gizmo of the balls spawn position.
    private void DrawBall()
    {
        Gizmos.DrawSphere(new Vector3(0, _breakoutBounds.min.y + _yPositionOfBall, _breakoutBounds.center.z), 0.25f);
    }
    
    // Draws the gizmo of the paddles spawn position.
    private void DrawPaddle()
    {
        Gizmos.DrawCube(new Vector3(0, _breakoutBounds.min.y + _yPositionOfPaddle, _breakoutBounds.center.z),new Vector3(1f, 0.5f, 0.25f));
    }

    // Draws the gizmo of each blocks spawn position.
    private void DrawBlocks()
    {
        float stepDistance = 1.5f;
        for (int i = 0; i < _breakoutLayers.Count; i++)
        {
            float yPosition = _breakoutBounds.max.y - (stepDistance * (i + 1)) - 0.5f;
            for (int j = 0; j < _breakoutLayers[i]; j++)
            {
                float xPosition = (_breakoutBounds.min.x) + ((_breakoutBounds.max.x - _breakoutBounds.min.x) * (1.0f / (_breakoutLayers[i] + 1)) * (j + 1));
                Gizmos.DrawCube(new Vector3(xPosition, yPosition, (_breakoutBounds.center.z * Mathf.Cos(xPosition * _curve))),new Vector3(1f, 0.5f, 0.25f));
            }
        }
    }

    // Renders the bounds of the player area and the curve the player area follows..
    private void DrawGizmoBreakoutBounds()
    {
        int resolution = 50;
        for (int x = 0; x < resolution; x++)
        {
            float delta = x * (_breakoutBounds.max.x - _breakoutBounds.min.x) * (1.0f / (resolution - 1.0f));
            Gizmos.DrawSphere(new Vector3(_breakoutBounds.min.x + delta, _breakoutBounds.min.y, (_breakoutBounds.center.z * Mathf.Cos((_breakoutBounds.min.x + delta) * _curve))), 0.125f);
        }
        for (int x = 0; x < resolution; x++)
        {
            float delta = x * (_breakoutBounds.max.x - _breakoutBounds.min.x) * (1.0f / (resolution - 1.0f));
            Gizmos.DrawSphere(new Vector3(_breakoutBounds.min.x + delta, _breakoutBounds.max.y, (_breakoutBounds.center.z * Mathf.Cos((_breakoutBounds.min.x + delta) * _curve))), 0.125f);
        }
        
        for (int y = 0; y < resolution; y++)
        {
            float delta = y * (_breakoutBounds.max.y - _breakoutBounds.min.y) * (1.0f / (resolution - 1.0f));
            Gizmos.DrawSphere(new Vector3(_breakoutBounds.min.x, _breakoutBounds.min.y + delta, (_breakoutBounds.center.z * Mathf.Cos(_breakoutBounds.min.x * _curve))), 0.125f);
        }
        for (int y = 0; y < resolution; y++)
        {
            float delta = y * (_breakoutBounds.max.y - _breakoutBounds.min.y) * (1.0f / (resolution - 1.0f));
            Gizmos.DrawSphere(new Vector3(_breakoutBounds.max.x, _breakoutBounds.min.y + delta, (_breakoutBounds.center.z * Mathf.Cos(_breakoutBounds.max.x * _curve))), 0.125f);
        }
        
        Gizmos.DrawCube(_focusTabletSpawnLocation, Vector3.one);
    }
}
