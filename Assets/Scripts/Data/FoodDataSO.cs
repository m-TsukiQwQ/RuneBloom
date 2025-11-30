using UnityEngine;

[CreateAssetMenu(menuName = "Game Setup/Item Data/Food item", fileName = "Food data - ")]
public class FoodDataSO : ItemDataSO
{
    [Header("Restores")]
    public int health;
    public int hunger;
    public int magicPower;
}
