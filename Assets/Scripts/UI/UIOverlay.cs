using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    [SerializeField] private GameObject _burnOverlay;
    [SerializeField] private EntityStatusHandler _status;

    private void Update()
    {
        _burnOverlay.SetActive(_status._currentEffects.Contains(ElementType.Fire));
    }
}
