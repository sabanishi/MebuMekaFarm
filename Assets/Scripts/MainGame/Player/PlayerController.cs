using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerAnimator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GroundChecker groundCheck;
        
        private Transform _transform;
        private Rigidbody2D _rb;
        private PlayerAnimator _animator;

        private const float WalkSpeed = 3f;
        private const float JumpSpeed = 5.5f;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<PlayerAnimator>();
        }
        
        private void Update()
        {
            Move();
            SwitchAnimation();
        }

        private void Move()
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 speedVector;
            if (horizontal > 0)
            {
                speedVector = new Vector3(WalkSpeed, _rb.linearVelocityY, 0);
            }
            else if (horizontal < 0)
            {
                speedVector = new Vector3(-WalkSpeed, _rb.linearVelocityY, 0);
            }
            else
            {
                speedVector = new Vector3(0, _rb.linearVelocityY, 0);
            }
            if (groundCheck.IsGround())
            {
                if (Input.GetButtonDown("Jump"))
                {
                    speedVector = new Vector3(speedVector.x, JumpSpeed, 0);
                }
            }
           
            _rb.linearVelocity= speedVector;
        }

        private void SwitchAnimation()
        {
            bool isInAir = !groundCheck.IsGround();
            _animator.SetIsInAir(isInAir);
            _animator.SetYSpeed(_rb.linearVelocityY);
            _animator.SetIsWalk(_rb.linearVelocityX != 0);

            if (_rb.linearVelocityX > 0)
            {
                _transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_rb.linearVelocityX < 0)
            {
                _transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}