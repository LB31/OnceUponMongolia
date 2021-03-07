using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingController : MonoBehaviour
{
    public int NeededAmountToFinish;
    public ParticleSystem CookingParticleEffect;
    public List<Image> IngredientImages;
    public TextMeshProUGUI MoonLightText;

    private HashSet<string> foodInPot = new HashSet<string>();

    public void PutFoodInPot(GameObject food)
    {
        string foodName = food.name;

        // For food that isn't cut yet
        if (!foodName.Contains("Cut"))
        {
            food.GetComponent<ItemRespawner>().ReturnToOriginalPos();
            return;
        }

        Destroy(food);
        string shortName = foodName.Substring(0, 3);
        foodInPot.Add(shortName);
        CookingParticleEffect.Play();

        // remove food element from ui
        Image foundFoodImage = IngredientImages.First(foodImage => foodImage.name.Contains(shortName));
        foundFoodImage.enabled = false;
        MoonLightText.text = Regex.Replace(MoonLightText.text, "[0-9]", (NeededAmountToFinish - foodInPot.Count).ToString());

        if (foodInPot.Count >= NeededAmountToFinish)
        {
            PlayMakerFSM.BroadcastEvent("ContinueStory");
        }
    }
}
