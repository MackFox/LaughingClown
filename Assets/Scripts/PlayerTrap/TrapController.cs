using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private static TrapController instance;
    [Header("References")]
    [SerializeField] private GameObject _killTrigger;
    [Tooltip("Needs correct Order from CollectableType Enum!")]
    [SerializeField] private List<MeshRenderer> ropeParts;
    [SerializeField] private List<MeshRenderer> nails;
    [SerializeField] private MeshRenderer _anivl;

    [Header("Materials")]
    [SerializeField] private Material _holoMaterial;
    [SerializeField] private Material _nailsMaterial;
    [SerializeField] private Material _ropesMaterial;
    [SerializeField] private Material _anvilMaterial;
    [Header("Animation")]
    [SerializeField] private Transform _ropeWithAnvil;
    [SerializeField] private AnimationCurve _animCurve;
    [SerializeField] private float _animDuration = 1f;
    [SerializeField] private Vector3 _targetAnvilPosition;
    private float _animTimer;
    private bool _animFinished;
    private Vector3 _animStartPosition;
    [Header("Kill Animation")]
    [SerializeField] private Vector3 _targetKillPosition;
    [SerializeField] private float _killAniDuration = 1f;
    [Header("Sound")]
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioClip _ropeSound;
    [SerializeField] private AudioClip _anvilSound;
    [SerializeField] private AudioClip _nailSound;
    [SerializeField] private AudioClip _anvilFalling;

    private Dictionary<CollectableType, bool> _collectableStatus = new Dictionary<CollectableType, bool>();
    [SerializeField] private bool _trapPrepared;
    [SerializeField] private bool _trapActivated;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _collectableStatus.Add(CollectableType.Anvil, false);
        _collectableStatus.Add(CollectableType.Rope, false);
        _collectableStatus.Add(CollectableType.Nail, false);
        _animStartPosition = _ropeWithAnvil.localPosition;
    }

    private void Start()
    {
        SetHoloMaterial();
    }

    private void Update()
    {
        // Anvil Animation
        if (_trapPrepared && !_animFinished)
        {
            _animTimer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(_animTimer / _animDuration);
            float curveValue = _animCurve.Evaluate(normalizedTime);

            Vector3 newPosition = Vector3.Lerp(_animStartPosition, _targetAnvilPosition, curveValue);

            _ropeWithAnvil.localPosition = newPosition;

            if (normalizedTime >= 1.0f)
            {
                _animFinished = true;
                _killTrigger.SetActive(true);
                _animTimer = 0.0f;
            }
        }

        if (_trapPrepared && _animFinished && _trapActivated)
        {
            PlayEnemyKillAnimation();
        }
    }

    private void CheckIfAllItemAreAssemblied()
    {
        if (AllCollecteablesAssembeld())
        {
            Debug.Log("Player has all Items collected!");
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        Debug.Log("Activation of Trap!");
        _trapPrepared = true;
    }

    private void SetHoloMaterial()
    {
        _anivl.material = _holoMaterial;
        ropeParts.ForEach(renderer => renderer.material = _holoMaterial);
        nails.ForEach(renderer => renderer.material = _holoMaterial);
    }

    public void AddCollectable(CollectableType newCollectable)
    {
        Debug.Log($"Player has add the {newCollectable} to the Trap!");
        _collectableStatus[newCollectable] = true;

        switch (newCollectable)
        {
            case CollectableType.Anvil:
                _anivl.material = _anvilMaterial;
                PlaySoundEffect(_anvilSound);
                break;
            case CollectableType.Rope:
                ropeParts.ForEach(renderer => renderer.material = _ropesMaterial);
                PlaySoundEffect(_ropeSound);
                break;
            case CollectableType.Nail:
                nails.ForEach(renderer => renderer.material = _nailsMaterial);
                PlaySoundEffect(_nailSound);
                break;
            default:
                break;
        }

        Player.GetInstance().RemoveItemFromPlayerHand();
        CheckIfAllItemAreAssemblied();
    }

    private void PlayEnemyKillAnimation()
    {
        _animTimer += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(_animTimer / _killAniDuration);
        Vector3 newPosition = Vector3.Lerp(_targetAnvilPosition, _targetKillPosition, normalizedTime);

        _ropeWithAnvil.localPosition = newPosition;
    }

    private void PlaySoundEffect(AudioClip targetClip)
    {
        _effectSource.Stop();
        _effectSource.clip = targetClip;
        _effectSource.Play();
    }

    public void TrapActivated()
    {
        _trapActivated = true;
        ClownAgent.GetInstance().MoveToDeathDestination(_killTrigger.transform.position);
        PlaySoundEffect(_anvilFalling);
    }

    bool AllCollecteablesAssembeld()
    {
        foreach (var pair in _collectableStatus)
        {
            if (!pair.Value)
                return false;
        }
        return true;
    }

    public static TrapController GetInstance()
    {
        return instance;
    }
}
