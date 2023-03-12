using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _sprite;

        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private LayerCheck _groundCheck;
        private static readonly int IsRunningKey = Animator.StringToHash("isRunning");
        private static readonly int IsGroundedKey = Animator.StringToHash("isGrounded");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("verticalVelocity");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }
        public void SetDirection(Vector2 directionVector)
        {
            _direction = directionVector;
        }
        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();
            if (isJumping)
            {
                if (IsGrounded())
                {
                    _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
                }
            }
            else if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
            }
            _animator.SetBool(IsRunningKey, _direction.x != 0);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsGroundedKey, isGrounded);

            SpriteDirection();
        }
        
        private void SpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
            }
        }
        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
        public void SayYohoho()
        {
            Debug.Log("Yohoho!");
        }
    }
}
