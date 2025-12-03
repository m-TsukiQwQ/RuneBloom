using UnityEngine;

public class ObjectFence : MonoBehaviour, IInteractable
{
    private Sprite _closedSprite;
    [SerializeField] private Sprite _openSprite;
    private SpriteRenderer _sr;
    private bool _isOpen = false;
    private BoxCollider2D[] _colliders;


    private void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _closedSprite = _sr.sprite;
        _colliders = GetComponents<BoxCollider2D>();
    }
    public void Intercat()
    {
        if(_isOpen)
        {
            _sr.sprite = _closedSprite;
            _colliders[0].enabled = true;
            _colliders[1].enabled = false;
            _colliders[2].enabled = false;
            _isOpen = false;
        }
        else
        {
            _sr.sprite = _openSprite;
            _colliders[0].enabled = false;
            _colliders[1].enabled = true;
            _colliders[2].enabled = true;
            _isOpen = true;
        }
        
    }

    
}
