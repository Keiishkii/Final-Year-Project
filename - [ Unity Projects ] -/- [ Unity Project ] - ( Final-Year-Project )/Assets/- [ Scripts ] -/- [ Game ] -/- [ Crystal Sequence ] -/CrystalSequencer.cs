using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrystalSequence
{
    // The script used on the game object displaying the crystal sequence.
    // Used to create the initial sequence and validate the inputs given in by the player.
    public class CrystalSequencer : MonoBehaviour
    {
        [SerializeField] private Transform _sequenceFolder;
        [SerializeField] private int _sequenceLength;

        [SerializeField] private Vector3 _curveStart, _curveEnd, _curveAnchor;
        
        [Space] 
        
        [SerializeField] private GameObject _sequenceElementPrefab;
        [SerializeField] private GameObject _sequenceLockPrefab;
        [SerializeField] private Material[] _crystalMaterials;

        [SerializeField] private float _lockAnimationDuration;

        private Transform _transform;
        
        private readonly List<CrystalColours_Enum> _sequence = new List<CrystalColours_Enum>();
        private readonly List<GameObject> _sequenceGameObjects = new List<GameObject>();
        private readonly List<GameObject> _sequenceLockGameObjects = new List<GameObject>();

        private readonly Dictionary<int, IEnumerator> lockingCoroutines = new Dictionary<int, IEnumerator>();
        
        private int _currentIndex;
        

        private void Awake()
        {
            _transform = transform;
            
            GenerateSequence();

            for (int i = 0; i < _sequenceLockGameObjects.Count; i++)
            {
                UnlockElement(i);
            }

            _currentIndex = 0;
        }



        // Function for correctness of the players input for the sequences next input.
        public bool IsCorrectColour(CrystalColours_Enum colour)
        {
            return (_sequence.Count > _currentIndex && colour == _sequence[_currentIndex]);
        }
        
        // Checks if the sequence has been fully inputted.
        public bool IsEndOfSequence()
        {
            return (_sequence.Count <= _currentIndex);
        }

        // Increments the index of the sequence.
        public void IncrementIndex()
        {
            LockElement(_currentIndex);
            _currentIndex++;
        }

        // Unlocks all indices resetting the sequence.
        public void ResetSequence()
        {
            _currentIndex = 0;
            
            for (int i = 0; i < _sequenceLockGameObjects.Count; i++)
            {
                UnlockElement(i);
            }
        }
        
        
        
        
        // Generates a sequence of a specified length, then using a given Bezier curve spawns in game objects as a visual representation of this sequence.
        private void GenerateSequence()
        {
            _sequence.Clear();
            for (int i = 0; i < _sequenceLength; i++)
            {
                _sequence.Add((CrystalColours_Enum) Random.Range(0, 5));
            }

            if (_sequenceLength > 1)
            {
                Vector3 position = _transform.position;
                Vector3 lockOffset = new Vector3(0, 0.7f, 0);

                float incrementValue = (1.0f) / (_sequenceLength - 1.0f);
                foreach (var (colour, i) in _sequence.Select((colour, i) => (colour, i)))
                {
                    float lerpValue = incrementValue * i;
                    Vector3 spawnPosition = Vector3.Lerp(
                        Vector3.Lerp(position + _curveStart, position + _curveAnchor, lerpValue), 
                        Vector3.Lerp(position + _curveAnchor, position + _curveEnd, lerpValue), 
                        lerpValue);
                    
                    GameObject sequenceElement = Instantiate(_sequenceElementPrefab, spawnPosition, Quaternion.identity, _sequenceFolder);

                    MeshRenderer renderer = sequenceElement.GetComponent<MeshRenderer>();
                    renderer.material = _crystalMaterials[(int) colour];
                    
                    _sequenceGameObjects.Add(sequenceElement);
                    
                    
                    GameObject sequenceLock = Instantiate(_sequenceLockPrefab, spawnPosition + lockOffset, Quaternion.identity, _sequenceFolder);
                    _sequenceLockGameObjects.Add(sequenceLock);
                }
            }
        }

        // Resets the visual side of sequence index, moves the game object that is used to hide the crystal colour away so that the crystal is once again visable.
        public void UnlockElement(int index)
        {
            if (_sequenceLockGameObjects.Count > index)
            {
                if (lockingCoroutines.ContainsKey(index))
                {
                    StopCoroutine(lockingCoroutines[index]);
                    lockingCoroutines.Remove(index);
                }

                Transform lockTransform = _sequenceLockGameObjects[index].transform;
                IEnumerator lockingCoroutine = UnlockingCoroutine(lockTransform);

                lockingCoroutines.Add(index, lockingCoroutine);
                StartCoroutine(lockingCoroutines[index]);
            }
        }
        
        // Scales up an elements lock game object so that it hides the colour of the indexes crystal. 
        public void LockElement(int index)
        {
            if (_sequenceLockGameObjects.Count > index)
            {
                if (lockingCoroutines.ContainsKey(index))
                {
                    StopCoroutine(lockingCoroutines[index]);
                    lockingCoroutines.Remove(index);
                }

                Transform lockTransform = _sequenceLockGameObjects[index].transform;
                IEnumerator lockingCoroutine = LockingCoroutine(lockTransform);

                lockingCoroutines.Add(index, lockingCoroutine);
                StartCoroutine(lockingCoroutines[index]);
            }
        }
        
        // Coroutine for moving and scaling the index lock down, used to make the transition between raising the lock smoother.
        private IEnumerator UnlockingCoroutine(Transform lockTransform)
        {
            Vector3 baseScale = lockTransform.localScale;

            float startingValue = Mathf.InverseLerp(0.75f, 0, baseScale.y);
            float increment = Time.fixedDeltaTime / _lockAnimationDuration;
            for (float i = startingValue; i < 1; i += increment)
            {
                Vector3 scale = new Vector3(baseScale.x, Mathf.Lerp(0.75f, 0, i), baseScale.z);
                lockTransform.localScale = scale;
                
                yield return null;
            }

            lockTransform.localScale = new Vector3(baseScale.x, 0, baseScale.z);
            yield return null;
        }
        
        // Coroutine for moving and scaling the index lock up, used to make the transition between lowering the lock smoother.
        private IEnumerator LockingCoroutine(Transform lockTransform)
        {
            Vector3 baseScale = lockTransform.localScale;

            float startingValue = Mathf.InverseLerp(0, 0.75f, baseScale.y);
            float increment = Time.fixedDeltaTime / _lockAnimationDuration;
            for (float i = startingValue; i < 1; i += increment)
            {
                Vector3 scale = new Vector3(baseScale.x, Mathf.Lerp(0, 0.75f, i), baseScale.z);
                lockTransform.localScale = scale;
                
                yield return null;
            }

            lockTransform.localScale = new Vector3(baseScale.x, 0.75f, baseScale.z);
            yield return null;
        }
        
        
        
        // Draws the position of the bezier curve used to spawn the crystals.
        private void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(position + _curveStart, 0.25f);
            Gizmos.DrawSphere(position + _curveEnd, 0.25f);
            Gizmos.DrawSphere(position + _curveAnchor, 0.25f);

            for (float i = 0; i < 1; i += 0.05f)
            {
                Vector3 position_1 = Vector3.Lerp(position + _curveStart, position + _curveAnchor, i); 
                Vector3 position_2 = Vector3.Lerp(position + _curveAnchor, position + _curveEnd, i);

                Vector3 dotPosition = Vector3.Lerp(position_1, position_2, i);
                
                Gizmos.DrawSphere(dotPosition, 0.125f);
            }

            if (_sequenceLength > 1)
            {
                float incrementValue = (1.0f) / (_sequenceLength - 1.0f);
                for (int i = 0; i < _sequenceLength; i++)
                {
                    float lerpValue = incrementValue * i;

                    Vector3 position_1 = Vector3.Lerp(position + _curveStart, position + _curveAnchor, lerpValue);
                    Vector3 position_2 = Vector3.Lerp(position + _curveAnchor, position + _curveEnd, lerpValue);

                    Vector3 spawnPosition = Vector3.Lerp(position_1, position_2, lerpValue);

                    Gizmos.DrawCube(spawnPosition, new Vector3(0.5f, 1.3f, 0.5f));
                }
            }
        }
    }
}