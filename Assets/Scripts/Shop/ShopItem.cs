using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public int[] pricePerLevel;
    [HideInInspector]public int level = 0;
    public int[] statPerLevel;

    public Text levelText;
    public Text priceText;
    public Button upgradeButton;
}
