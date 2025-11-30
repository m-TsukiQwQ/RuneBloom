using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Instrument item", fileName = "Instrument data - ")]
public class InstrumentDataSO : ItemDataSO
{
    [Header("Item modifiers")]
    public ItemModifier[] modifiers;

}
