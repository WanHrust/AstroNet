using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroNet.GameElements;

namespace AstroNet
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _asteroidTypes;
        [SerializeField] private Transform _spawnLocaiton;
        [SerializeField] private GestureRecogniser _gestureRecogniser;


        [SerializeField] Vector2 _initialTorqueRange;
        [SerializeField] Vector2 _initialForceRange;

        [SerializeField] Transform _target;
        private GameState _currentState = GameState.Idle;

        private AssembledObject _currentAsteroid;
        public enum GameState
        {
            StartAttack,
            Idle
        }
        // Start is called before the first frame update
        protected void Start()
        {
            _gestureRecogniser.OnShapeRecognised += ShapeRecognized;
            SetGameState(GameState.Idle);
        }

        protected void OnDestroy()
        {
            _gestureRecogniser.OnShapeRecognised -= ShapeRecognized;
        }

        private void SetGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Idle:
                    Invoke("SummonNewAsteroid", 1);
                    break;
                case GameState.StartAttack:
                    var randomIndex = Random.Range(0, _asteroidTypes.Count);
                    var asteroid = Instantiate(_asteroidTypes[randomIndex], _spawnLocaiton);
                    _currentAsteroid = asteroid.GetComponent<AssembledObject>();
                    var direction = _target.position - transform.position;
                    var force = direction * Random.Range(_initialForceRange.x, _initialForceRange.y);
                    _currentAsteroid.ApplyInitialForce(force);
                    var tx = Random.Range(_initialTorqueRange.x, _initialTorqueRange.y);
                    var ty = Random.Range(_initialTorqueRange.x, _initialTorqueRange.y);
                    var tz = Random.Range(_initialTorqueRange.x, _initialTorqueRange.y);
                    var torque = new Vector3(tx, ty, tz);
                    _currentAsteroid.ApplyInitialTorque(_initialTorqueRange);
                    _currentAsteroid.OnAsteroidDestroyed += AsteroidDestroyd;
                    break;
                default:
                    Debug.LogError("State is not handled: " + state);
                    break;

            }
        }

        private void SummonNewAsteroid()
        {
            SetGameState(GameState.StartAttack);
        }

        private void ShapeRecognized(FaceType faceType)
        {
            if (_currentAsteroid == null) return;
            _currentAsteroid.Attack(faceType);
        }

        private void AsteroidDestroyd()
        {
            _currentAsteroid.OnAsteroidDestroyed -= AsteroidDestroyd;
            Destroy(_currentAsteroid.gameObject);
            _currentAsteroid = null;
            SetGameState(GameState.Idle);
        }
    }
}
