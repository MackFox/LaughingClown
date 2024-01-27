using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownAnimator : MonoBehaviour
{
    private static ClownAnimator instance;
    private const string Walking = "walking";
    private const string Watching = "watching";
    private const string Running = "running";
    private const string WalkingSpeed = "walkSpeed";
    private const string Death = "death";

    [SerializeField] private Animator _animator;
    private float _defaultWalikingSpeed;

    private Dictionary<AnimationStates, string> _states = new Dictionary<AnimationStates, string>();

    public enum AnimationStates
    {
        Idle = 0,
        Walking = 1,
        Watching = 2,
        Running = 3,
        Death = 4,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _defaultWalikingSpeed = _animator.GetFloat(WalkingSpeed);

        //_states.Add(AnimationStates.Idle, "");
        _states.Add(AnimationStates.Walking, Walking);
        _states.Add(AnimationStates.Watching, Watching);
        _states.Add(AnimationStates.Running, Running);
        //_states.Add(AnimationStates.Death, Death);
    }

    public void SetAnimationState(AnimationStates newAnimationState)
    {
        // Iteriere durch die AnimationStates im Dictionary
        foreach (var state in _states)
        {
            // Aktiviere/deaktiviere den bool-Wert basierend auf dem aktuellen Zustand
            bool isActive = state.Key == newAnimationState;
            _animator.SetBool(state.Value, isActive);
        }
    }

    public void SetWalkingSpeed(float speedMultiply)
    {
        _animator.SetFloat(WalkingSpeed, _defaultWalikingSpeed + speedMultiply);
    }

    public static ClownAnimator GetInstance()
    {
        return instance;
    }
}
