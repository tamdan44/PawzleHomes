using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public Image shirtSprite;
    public Image hatSprite;
    public Image glassSprite;
    public Image propSprite;

    public enum OutfitPart { Shirt, Hat, Glass, Prop }

    public void ApplyOutfit(CharacterOutfitManager outfit, OutfitPart outfitPart)
    {
        switch (outfitPart)
        {
            case OutfitPart.Shirt:
                shirtSprite.sprite = outfit.shirt;
                break;
            case OutfitPart.Hat:
                hatSprite.sprite = outfit.hat;
                break;
            case OutfitPart.Glass:
                glassSprite.sprite = outfit.glass;
                break;
            case OutfitPart.Prop:
                propSprite.sprite = outfit.prop;
                break;
        }
    }


}
