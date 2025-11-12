using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    [SerializeField] private GameObject _burnOverlay;
    [SerializeField] private EntityStatusHandler _status;

    private void Update()
    {
        switch(_status._currentEffect)
        {
            case ElementType.Fire:
                _burnOverlay.SetActive(true); 
                break;

            default:
                _burnOverlay.SetActive(false);
                break;
        }
    }
}
