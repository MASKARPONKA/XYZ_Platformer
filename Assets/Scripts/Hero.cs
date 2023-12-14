using UnityEngine;
using PixelCrew.Components;
using UnityEditor;
using UnityEditor.Animations;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;
        private bool _isFalling;

        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private int _damage;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerCheck _groundCheck;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [SerializeField] private CheckCircleOverlap _attackRange;

        [SerializeField] private SpawnComponent _spawnStepParticles;
        [SerializeField] private SpawnComponent _spawnJumpParticles;
        [SerializeField] private SpawnComponent _spawnFallParticles;
        [SerializeField] private ParticleSystem _DamageParcticles;
        [SerializeField] private ParticleSystem _BrokenShieldParticles;

        private Collider2D[] _interactionResult = new Collider2D[1];
        private static readonly int IsRunningKey = Animator.StringToHash("isRunning");
        private static readonly int IsGroundedKey = Animator.StringToHash("isGrounded");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("verticalVelocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        private GameSession _session;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.HP = currentHealth;
        }
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.HP);
            UpdateHeroWeapon();
        }
        public void ScoreAlter(int value)
        {
            _session.Data.Coins += value;
            if (_session.Data.Coins <= 0)
            {
                _session.Data.Coins = 0;
            }
            Debug.Log("Your score:" + _session.Data.Coins);
        }
        public void SetDirection(Vector2 directionVector)
        {
            _direction = directionVector;
        }
        private void Update()
        {
            _isGrounded = IsGrounded();
        }
        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsRunningKey, _direction.x != 0);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsGroundedKey, _isGrounded);

            SpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
                SpawnFallDust();
            }
            else isFalling();
            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.05f;
            }
            return yVelocity;
        }
        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                SpawnJumpDust();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                SpawnJumpDust();
            }
            return yVelocity;
        }
        private void SpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
#endif
        public void SayYohoho()
        {
            Debug.Log("Yohoho!");
        }
        public void Attack()
        {
            if (!_session.Data.IsArmed) return;

            _animator.SetTrigger(AttackKey);
        }
        public void OnSwordAttack()
        {
            var gos = _attackRange.GetObjectInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();
                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ModifyHealth(-_damage);
                }
            }
        }
        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }
        public void ShieldBreak()
        {
            _isJumping = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
            _BrokenShieldParticles.gameObject.SetActive(true);
            _BrokenShieldParticles.Play();
        }
        public void SpawnCoins()
        {
            var numCoinsToDrop = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToDrop;

            var burst = _DamageParcticles.emission.GetBurst(0);
            burst.count = numCoinsToDrop;
            _DamageParcticles.emission.SetBurst(0, burst);

            _DamageParcticles.gameObject.SetActive(true);
            _DamageParcticles.Play();
            Debug.Log("Your score:" + _session.Data.Coins);
        }
        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
        public void SpawnFootDust()
        {
            _spawnStepParticles.Spawn();
        }
        public void SpawnJumpDust()
        {
            _spawnJumpParticles.Spawn();
        }
        public void isFalling()
        {
            if(_rigidbody.velocity.y < -13f)
            {
                _isFalling = true;
            }
        }
        public void SpawnFallDust()
        {
            if (_isFalling)
            {
                _isFalling = false;
                _spawnFallParticles.Spawn();
            }
        }
        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            _animator.runtimeAnimatorController = _armed;
        }
        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }
    }
}
