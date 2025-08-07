using UnityEngine;

public class ButtonOutfit : MonoBehaviour
{
    [Header("Outfit Data")]
    public CharacterOutfitManager outfitManager;
    public ItemList.OutfitPart outfitPart;
    public ItemList item;

    public void ChangeOutfit()
    {
        if (outfitManager != null && item != null)
        {
            item.ApplyOutfit(outfitManager, outfitPart);
        }
        else
        {
            Debug.Log("Cannot find the reference to the database or the script");
        }
    }

}
