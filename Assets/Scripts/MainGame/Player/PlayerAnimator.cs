using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int YSpeedId = Animator.StringToHash("ySpeed");
        private static readonly int IsHoldId = Animator.StringToHash("isHold");
        private static readonly int IsWalkId = Animator.StringToHash("isWalk");
        private static readonly int IsInAirId = Animator.StringToHash("isInAir");
        
        public void SetYSpeed(float value)
        {
            animator.SetFloat(YSpeedId, value);
        }

        public void SetIsHold(bool value)
        {
            animator.SetBool(IsHoldId, value);
        }

        public void SetIsWalk(bool value)
        {
            animator.SetBool(IsWalkId, value);
        }

        public void SetIsInAir(bool value)
        {
            animator.SetBool(IsInAirId, value);
        }
    }
}