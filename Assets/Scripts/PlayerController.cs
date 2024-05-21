using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using Pokemon;
using Utilities;

namespace Pokemon
{
    public class PlayerController : MonoBehaviour
    {

#region Private Properties

        [Header("References")]
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected CinemachineVirtualCameraBase _vCam;
        [SerializeField] protected InputController _input;

        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _damping = 0.5f;
        [SerializeField] private float _smoothTime = 0.1f;
        [SerializeField] private float _groundDistance = 0.08f;
        [SerializeField] private LayerMask _groundLayer;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _jumpCooldown = 0f;
        [SerializeField] private float _jumpDuration = 0.5f;
        [SerializeField] private float _gravityMultiplier = 3f;

        [Header("Animations")]
        protected readonly int _Moveblend = Animator.StringToHash("MoveBlend");
        protected readonly int _Jumpblend = Animator.StringToHash("JumpBlend");

        private Vector3 _movement;
        private Transform _camera;
        private float _speed;
        private float _velocity;
        private Vector3 _currentVeclocity;
        private float _jumpVelocity;
        private List<Timer> _timers;
        private CountDownTimer _jumpTimer;
        private CountDownTimer _jumpCooldownTimer;

        protected StateMachine _stateMachine;


#endregion

#region Public Properties

        public bool _grounded;

    #endregion

#region Unity Functions

        protected virtual void OnEnable()
        {
            _input.Jump += OnJump;
        }
        protected virtual void OnDisable()
        {
            _input.Jump -= OnJump;
        }
        protected virtual void Awake()
        {
            _camera = Camera.main.transform;

            _rigidbody.freezeRotation = true;

            _jumpTimer = new CountDownTimer(_jumpDuration);
            _jumpCooldownTimer = new CountDownTimer(_jumpCooldown);
            _timers = new List<Timer>(2) {_jumpTimer, _jumpCooldownTimer};

            _jumpTimer.OnTimerStart += () => _jumpVelocity = _jumpForce;
            _jumpTimer.OnTimerStop += () => _jumpCooldownTimer.Start();

            _stateMachine = new StateMachine();

            var moveState = new MoveState(this, _animator);
            var jumpState = new JumpState(this, _animator);


            At(moveState, new FuncPredicate(() => _jumpTimer._isRunning), jumpState);
            At(jumpState, new FuncPredicate(() => _grounded && !_jumpTimer._isRunning), moveState);

            _stateMachine.SetDefaultState(moveState);
        }
        protected virtual void Start()
        {
            _input.EnablePlayerActions();
        }
        protected void Update()
        {
            _grounded = Physics.SphereCast(transform.position, _groundDistance, Vector3.down, out _, _groundDistance, _groundLayer);
            _movement = new Vector3(_input.Direction.x, 0, _input.Direction.y).normalized;

            UpdateAnimator();
            UpdateTimers();

            _stateMachine.Update();
        } 
        protected void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

    #endregion

#region Public Methods
        
        public void UpdateJump()
        {
            if (!_jumpTimer._isRunning && _grounded)
            {
                _jumpVelocity = 0f;
                _jumpTimer.Stop();
                return;
            }

            if (!_jumpTimer._isRunning)
            {
                _jumpVelocity += Physics.gravity.y * _gravityMultiplier * Time.fixedDeltaTime;
            }

            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpVelocity, _rigidbody.velocity.z);
            Debug.Log(_rigidbody.velocity.y);
        }
        public void UpdateMovement()
        {
            Vector3 targetDir = Quaternion.AngleAxis(_camera.eulerAngles.y, Vector3.up) * _movement;

            if (targetDir.magnitude > 0f)
            {
                RotatePlayer(targetDir);
                MovePlayerHorizontal(targetDir);

                SmoothSpeed(targetDir.magnitude);
            }
            else
            {
                SmoothSpeed(0f);
                _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
            }
        }

#endregion

#region Private Methods

        private void OnJump(bool performed)
        {
            if (performed && !_jumpTimer._isRunning && !_jumpCooldownTimer._isRunning && _grounded) 
            {
                _jumpTimer.Start();
            }
            else if (!performed &&_jumpTimer._isRunning)
            {
                _jumpTimer.Stop();
            }
        }
        private void UpdateTimers()
        {
            foreach (Timer timer in _timers) timer.Tick(Time.deltaTime);
        }
        protected virtual void UpdateAnimator()
        {
            _animator.SetFloat(_Moveblend, _speed);
            _animator.SetFloat(_Jumpblend, 0f);
        }
        private void MovePlayerHorizontal(Vector3 direction)
        {
            Vector3 targetVelocity = direction * _moveSpeed;
            
            if (Vector3.Angle(_currentVeclocity, targetVelocity) < 100)
                _currentVeclocity = Vector3.Slerp(_currentVeclocity, targetVelocity, Damper.Damp(1, _damping, Time.fixedDeltaTime));
            else
                _currentVeclocity += Damper.Damp(targetVelocity - _currentVeclocity, _damping, Time.fixedDeltaTime);
            
            _rigidbody.velocity = new Vector3(_currentVeclocity.x, _rigidbody.velocity.y, _currentVeclocity.z);
        }
        private void RotatePlayer(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Damper.Damp(1, _damping, Time.deltaTime));
        }
        private void SmoothSpeed(float speed)
        {
            _speed = Mathf.SmoothDamp(_speed, speed, ref _velocity, _smoothTime);
        }
        protected void At(IState from, IPredicate condition, IState to) => _stateMachine.AddTransition(from, condition, to);
        protected void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

#endregion
    }
}