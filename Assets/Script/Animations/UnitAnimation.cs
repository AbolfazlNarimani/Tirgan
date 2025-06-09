
using Script.BowAndArrow;
using UnityEngine;


namespace Script.Animations
{
    public class UnitAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        

        public void OnReachArrow()
        {
            PlayerBow.Instance.NockArrow(); // Prepare arrow visually
        }
        
        public void OnReleaseArrow()
        {
            PlayerBow.Instance.ReleaseArrow(); // Actually fire the arrow
        }
    }
}