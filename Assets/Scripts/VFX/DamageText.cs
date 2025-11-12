using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;

    [Header("Random X postition")]
    [SerializeField] private bool radomOffset = true;
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;


    private void ApplyRandomYOffset()
    {
        if (radomOffset == false)
        {
            return;
        }
        float xOffset = Random.Range(xMinOffset,xMaxOffset);
        transform.position = transform.position + new Vector3(xOffset, 0 );
    }

    public void SetDamageText(float damage, Color color)
    {

        ApplyRandomYOffset();
        _damageText.color = color;
        _damageText.text = damage.ToString();
    }

    public void SetDodgeText(Color color)
    {

        _damageText.color = color;
        _damageText.text = "dodge";
    }

    public void DestroyText()
    {
        Destroy(this.gameObject, 0.3f);
    }
}
