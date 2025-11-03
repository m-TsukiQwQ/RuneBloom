using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;

    public void SetDamageText(float damage, Color color)
    {
        _damageText.color = color;
        _damageText.text = damage.ToString();
    }

    public void DestroyText()
    {
        Destroy(this.gameObject, 0.3f);
    }
}
