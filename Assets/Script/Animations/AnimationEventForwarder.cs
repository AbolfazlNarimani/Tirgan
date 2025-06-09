using UnityEngine;

namespace Script.Animations
{
    public class NewMonoBehaviourScript : MonoBehaviour
    {
        [Header("Assign in Inspector")]
        public UnitAnimation receiver; // Drag your parent object here

        // Must match EXACTLY what's in the animation event
        public void OnReachArrow() => receiver?.OnReachArrow();
    
        public void OnReleaseArrow() => receiver?.OnReleaseArrow();
    }
}
