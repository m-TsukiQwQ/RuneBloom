using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI chargesText;

    public void UpdateDisplay(Sprite icon, int charges)
    {
        if (iconImage != null)
        {
            iconImage.sprite = icon;
        }


        if (chargesText != null)
        {

            if (charges > 1)
            {
                chargesText.text = charges.ToString();
                chargesText.gameObject.SetActive(true);
            }
            else
            {

                chargesText.gameObject.SetActive(false);
            }
        }
    }
}
