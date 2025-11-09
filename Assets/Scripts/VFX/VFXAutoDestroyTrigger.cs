using UnityEngine;

public class VFXAutoDestroyTrigger : MonoBehaviour
{
    public void DestroyTrigger()
    {
        Destroy(gameObject, 3f);
    }
}
