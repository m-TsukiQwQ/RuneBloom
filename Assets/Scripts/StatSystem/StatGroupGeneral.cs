using System;
using UnityEngine;

[Serializable]
public class StatGroupGeneral
{
    public StatGroupGeneralHarvesting harvesting;
    public StatGroupGeneralRunning running;
    public StatGroupGeneralMining mining;
    public StatGroupGeneralCrafting crafting;
    public StatGroupGeneralCooking cooking;
    public StatGroupGeneralFarming farming;

}

[Serializable]
public class StatGroupGeneralHarvesting
{
    // Farming Stats
    public Stat harvestingSpeed;       // Speed of gathering crops
    public Stat extraCropChance;       // Chance for more crops
    public Stat extraSeedChance;       // Chance for more seeds
    public Stat cropGrowthSpeed;       // Passive bonus to crop growth

    // Woodcutting Stats
    public Stat choppingDamage;        // "Damage" to trees, faster chopping
    public Stat extraWoodChance;       // Chance for more wood
    public Stat rareDropChance;       // Chance for rare drops (sap, pollen, etc.)
}

[Serializable]
public class StatGroupGeneralRunning
{    
    public Stat movementSpeed; // Your base movement speed multiplier    
    public Stat foodDrainReduction; // Percent reduction in food/hunger drain while moving    
    public Stat dodgeChance; // Chance to completely avoid a hit while running    
    public Stat slowResistance; // Reduces the duration and effect of movement-slowing debuffs

}

[Serializable]
public class StatGroupGeneralMining
{   
    public Stat miningDamage; // The "damage" you deal to walls/ores with a pickaxe   
    public Stat extraOreChance; // Chance to get one extra ore from a vein    
    public Stat extraGemChance; // Chance to get one extra gem from a gem deposit 
    public Stat toolDurabilityLossReduction; // Percent reduction in durability loss for mining tools
    public Stat bombDamage; // Bonus damage and radius for bombs used for mining
}
[Serializable]
public class StatGroupGeneralCrafting
{    
    public Stat craftingSpeed;// How fast you craft items at a workbench    
    public Stat resourceSaveChance;// Chance to not consume an ingredient during crafting  
    public Stat craftingQualityChance;// Chance to craft a "Superior" item with bonus stats  
    public Stat extraItemChance;// Chance to create an extra item (for stackable items like ammo, bridges)
}

[Serializable]
public class StatGroupGeneralCooking
{    
    public Stat cookingSpeed; // How fast you cook meals    
    public Stat extraMealChance; // Chance to cook an extra meal for free    
    public Stat buffDurationMultiplier; // Percent multiplier for the duration of buffs from food you cook    
    public Stat buffPotencyMultiplier; // Percent multiplier for the potency (stats) of buffs from food you cook    
    public Stat foodHealingMultiplier; // Percent multiplier for the healing amount from food you cook
}


[Serializable]
public class StatGroupGeneralFarming
{   
    public Stat harvestingSpeed; // How fast you gather crops    
    public Stat extraCropChance; // Chance to get one extra of the primary crop    
    public Stat extraSeedChance; // Chance to get an extra seed back   
    public Stat cropGrowthSpeed; // Passive bonus to crop growth speed    
    public Stat waterSaveChance; // Chance to not use water from the watering can
}
