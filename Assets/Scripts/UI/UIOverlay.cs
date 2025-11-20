using UnityEngine;

public class UIOverlay : MonoBehaviour
{


    [Header("Configuration")]
    [SerializeField] private GameObject _burnOverlay;
    [SerializeField] private GameObject _freezeOverlay;
    [SerializeField] private GameObject _poisonOverlay;
    [SerializeField] private EntityStatusHandler _status;

    private void Update()
    {
        _burnOverlay.SetActive(_status._currentEffects.Contains(ElementType.Fire));
        _freezeOverlay.SetActive(_status._currentEffects.Contains(ElementType.Ice));
        _poisonOverlay.SetActive(_status._currentEffects.Contains(ElementType.Poison));
    }

    
}
