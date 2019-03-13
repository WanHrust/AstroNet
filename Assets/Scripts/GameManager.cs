using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AstroNet.GameElements;
using AstroNet.UI;

namespace AstroNet
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _asteroidTypes;
        [SerializeField] private Transform _spawnLocaiton;
        [SerializeField] private GestureRecogniser _gestureRecogniser;


        [SerializeField] private Vector2 _initialTorqueRange;
        [SerializeField] private Vector2 _initialForceRange;

        [SerializeField] private Transform _target;

        [SerializeField] private UIController _uIController;

        [SerializeField] private GameObject _targetExplosion;

        private GameState _currentState = GameState.Game;

        private AssembledObject _currentAsteroid;
        private float _score;
        private bool addScore;
        public enum GameState
        {
            StartAttack,
            Game,
            Menu,
            Pause,
            GameOver
        }
        // Start is called before the first frame update
        protected void Start()
        {
            _gestureRecogniser.OnShapeRecognised += ShapeRecognized;
            _uIController.OnStartButtoClicked += StartButtonClicked;
            _uIController.OnReturnButtoClicked += ReturnButtonClicked;
            _uIController.OnPauseButtoClicked += PauseButtonClicked;
            _uIController.OnContinueButtonClicked += ContinueButtonClicked;
            SetGameState(GameState.Menu);
        }
        protected void Update()
        {
            if (addScore)
            {
                _score += Time.deltaTime;
                _uIController.SetScore(_score);
            }
        }
        private void StartButtonClicked()
        {
            _uIController.ShowMenu(false);
            _uIController.ShowGameCanvas(true, 0f);
            SetGameState(GameState.Game);
        }
        private void ReturnButtonClicked()
        {
            SetGameState(GameState.Menu);
        }
        private void PauseButtonClicked()
        {
            SetGameState(GameState.Pause);
        }
        private void ContinueButtonClicked()
        {
            SetGameState(GameState.Game);
        }
        protected void OnDestroy()
        {
            _gestureRecogniser.OnShapeRecognised -= ShapeRecognized;
            _uIController.OnStartButtoClicked -= StartButtonClicked;
            _uIController.OnReturnButtoClicked -= ReturnButtonClicked;
            _uIController.OnPauseButtoClicked -= PauseButtonClicked;
            _uIController.OnContinueButtonClicked -= ContinueButtonClicked;
        }

        private void SetGameState(GameState state)
        {
            _currentState = state;
            switch (state)
            {
                case GameState.Menu:
                    if (_currentAsteroid != null)
                        Destroy(_currentAsteroid.gameObject);
                    _uIController.ShowMenu(true);
                    _uIController.ShowGameCanvas(false, 0f);
                    _score = 0f;
                    _targetExplosion.SetActive(false);
                    addScore = false;
                    break;
                case GameState.Pause:
                    addScore = false;
                    Time.timeScale = 0f;
                    break;
                case GameState.Game:
                    addScore = true;
                    Time.timeScale = 1f;
                    if(_currentAsteroid == null)
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
                    _currentAsteroid.OnAsteroidDestroyed += AsteroidDestroyed;
                    _currentAsteroid.OnTargetReached += TargetReached;
                    break;
                case GameState.GameOver:
                    addScore = false;
                    _targetExplosion.SetActive(true);
                    _uIController.ShowGameCanvas(false);
                    Invoke("GoToMenu", 15f);
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

        private void AsteroidDestroyed()
        {
            _currentAsteroid.OnAsteroidDestroyed -= AsteroidDestroyed;
            Destroy(_currentAsteroid.gameObject);
            _currentAsteroid = null;
            SetGameState(GameState.Game);
        }
        private void TargetReached()
        {
            _currentAsteroid.OnTargetReached -= AsteroidDestroyed;
            SetGameState(GameState.GameOver);

        }
        private void GoToMenu()
        {
            SetGameState(GameState.Menu);
        }
    }
}
