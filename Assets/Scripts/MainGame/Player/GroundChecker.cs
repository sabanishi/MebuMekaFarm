using Sabanihi.MebuMekaFarm;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame.Player
{
    /// <summary>
    /// 接地判定を行うためのクラス
    /// </summary>
    public class GroundChecker : MonoBehaviour
    {
        private Transform _transform;
        
        private bool _isGround = false;
        private bool _isGroudEnter, _isGroundStay, _isGroundExit;

        private void Awake()
        {
            _transform = transform;
        }
        
        public bool IsGround()
        {
            if (_isGroundExit)
            {
                _isGround = false;
            }
            else if (_isGroudEnter || _isGroundStay)
            {
                _isGround = true;
            }

            _isGroudEnter = false;
            _isGroundStay = false;
            _isGroundExit = false;
            return _isGround;
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsHitTarget(collision))
            {
                _isGroudEnter = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (IsHitTarget(collision))
            {
                _isGroundStay = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsHitTarget(collision))
            {
                _isGroundExit = true;

            }
        }

        private bool IsHitTarget(Collider2D collision)
        {
            return collision.CompareTag(TagName.Block) 
                   || collision.CompareTag(TagName.Box);
        }
    }
}