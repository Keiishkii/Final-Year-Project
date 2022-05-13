using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrystalSequence
{
    public class MiniGameController_CrystalSequence : MonoBehaviour
    {
        [SerializeField] private Transform _crystalFolder;
        [SerializeField] private GameObject[] _crystalPrefabs;
        [SerializeField] private GameObject _sequenceTabletPrefab;
        
        [SerializeField] private Vector3 _sequenceTabletSpawnLocation;

        [Space]
        [SerializeField] private GameObject _completionTextPrefab;
        [SerializeField] private Vector3 _completionTextSpawnLocation;
        [SerializeField] private GameObject _returnTabletPrefab;
        [SerializeField] private Vector3 _returnTabletSpawnLocation;

        [Space]
        [SerializeField] private float _crystalDistanceFromCenter;
        [SerializeField] private float _crystalFloatHeight;

        private readonly List<GameObject> _crystals = new List<GameObject>();
        private GameObject _sequenceTablet;
        private CrystalSequencer _crystalSequencerScript;

        private GameObject _completionText;
        private GameObject _returnTablet;

        private Transform _transform;


        
        private void Awake()
        {
            _transform = transform;
        }

        
        
        private void Start()
        {
            Vector3 center = new Vector3(0, _crystalFloatHeight, 0);

            float incrementValue = (Mathf.PI * 2) / (_crystalPrefabs.Length + 1);
            foreach (var (crystalPrefab, i) in _crystalPrefabs.Select((crystalPrefab, i) => (crystalPrefab, i)))
            {
                float angle = (incrementValue * (i + 1));

                Vector3 position = new Vector3(Mathf.Sin(angle) * _crystalDistanceFromCenter, _crystalFloatHeight, Mathf.Cos(angle) * _crystalDistanceFromCenter);
                Quaternion rotation = Quaternion.LookRotation(Vector3.Normalize(center - position), Vector3.up);

                GameObject gameObject = Instantiate(crystalPrefab, position, rotation, _crystalFolder);
                _crystals.Add(gameObject);
            }

            _sequenceTablet = Instantiate(_sequenceTabletPrefab, _sequenceTabletSpawnLocation, quaternion.Euler(new float3(0, Mathf.PI, 0)), _transform);
            _crystalSequencerScript = _sequenceTablet.GetComponent<CrystalSequencer>();
        }



        public void ActivatedCrystal(CrystalColours_Enum colour)
        {
            if (_crystalSequencerScript.IsCorrectColour(colour))
            {
                _crystalSequencerScript.IncrementIndex();
                if (_crystalSequencerScript.IsEndOfSequence())
                {
                    Destroy(_sequenceTablet);
                    foreach (GameObject crystal in _crystals)
                    {
                        Destroy(crystal);
                    }

                    _completionText = Instantiate(_completionTextPrefab, _completionTextSpawnLocation, Quaternion.Euler(new Vector3(0, 180, 0)), _transform);
                    _returnTablet = Instantiate(_returnTabletPrefab, _returnTabletSpawnLocation, Quaternion.Euler(new Vector3(-20, 180, 0)), _transform);
                    _returnTablet.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                }
            }
            else
            {
                _crystalSequencerScript.ResetSequence();
            }
        }
        
        
        

        private void OnDrawGizmos()
        {
            float incrementValue = (Mathf.PI * 2) / (_crystalPrefabs.Length + 1);
            foreach (var (crystalPrefab, i) in _crystalPrefabs.Select((crystalPrefab, i) => (crystalPrefab, i)))
            {
                Gizmos.color = (i == 0) ? Color.red : Color.green;
                float angle = (incrementValue * (i + 1));
                Gizmos.DrawSphere(new Vector3(Mathf.Sin(angle) * _crystalDistanceFromCenter, _crystalFloatHeight, Mathf.Cos(angle) * _crystalDistanceFromCenter), 0.25f * Mathf.Pow(1.2f, i));
            }

            Gizmos.color = Color.black;
            Gizmos.DrawCube(_completionTextSpawnLocation, new Vector3(5, 1.5f, 0.5f));
            Gizmos.DrawCube(_returnTabletSpawnLocation, new Vector3(1.5f, 1f, 0.5f));
        }
    }
}