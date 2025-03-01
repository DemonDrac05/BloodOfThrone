using UnityEngine;

public class BaseItem : ScriptableObject
{
    [Header("=== Visual Settings ==========")]
    public Sprite image;

    [Header("=== Pricing Information ==========")]
    public int buyingPrice;
    public int sellingPrice;

    [Header("=== Item Properties ==========")]
    public RarityType RarityType;
    [TextArea] public string itemDescription;
    [TextArea] public string itemMainUse;

    [Header("=== Status Flags ==========")]
    [HideInInspector] public bool isPurchaseable;

    private void OnEnable() => isPurchaseable = buyingPrice != -1 || sellingPrice != -1;

    public virtual int MaxStackable => 999;
}


