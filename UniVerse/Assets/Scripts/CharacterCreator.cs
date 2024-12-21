using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UMA;
using UMA.CharacterSystem;

public class CharacterCreator : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider weightSlider;
    [SerializeField] private Slider waistSlider;
    [SerializeField] private Button[] skinColorButtons;
    [SerializeField] private Button[] eyeColorButtons;
    [SerializeField] private Button[] hairColorButtons;
    private Dictionary<string, DnaSetter> dna;

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
        heightSlider.onValueChanged.AddListener(HeightChange);
        weightSlider.onValueChanged.AddListener(WeightChange);
        waistSlider.onValueChanged.AddListener(WaistChange);
        for (int i = 0; i < skinColorButtons.Length; i++)
        {
            int index = i; // Local copy for the closure
            skinColorButtons[index].onClick.AddListener(() => ChangeSkinColor(index));
        }
        for (int i = 0; i < eyeColorButtons.Length; i++)
        {
            int index = i; // Local copy for the closure
            eyeColorButtons[index].onClick.AddListener(() => ChangeEyeColor(index));
        }
        for (int i = 0; i < hairColorButtons.Length; i++)
        {
            int index = i; // Local copy for the closure
            hairColorButtons[index].onClick.AddListener(() => ChangeHairColor(index));
        }
    }

    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
        heightSlider.onValueChanged.RemoveListener(HeightChange);
        weightSlider.onValueChanged.RemoveListener(WeightChange);
        waistSlider.onValueChanged.RemoveListener(WaistChange);
        foreach (var button in skinColorButtons)
        {
            button.onClick.RemoveAllListeners();
        }
        foreach (var button in eyeColorButtons)
        {
            button.onClick.RemoveAllListeners();
        }
        foreach (var button in hairColorButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    void Updated(UMAData data)
    {
        dna = avatar.GetDNA();
        heightSlider.value = dna["height"].Get();
        weightSlider.value = dna["belly"].Get();
        waistSlider.value = dna["waist"].Get();
    }

    public void HeightChange(float val)
    {
        dna["height"].Set(val);
        avatar.BuildCharacter();
    }

    public void WeightChange(float val)
    {
        dna["belly"].Set(val); // Update belly value
        avatar.BuildCharacter(); // Rebuild the avatar
    }

    public void WaistChange(float val)
    {
        dna["waist"].Set(val);
        avatar.BuildCharacter();
    }
    public void ChangeSkinColor(int index)
    {
        // Define your predefined skin colors
        Color[] skinColors = new Color[]
{
    new Color(1f, 1f, 1f),          // #FFFFFF
    new Color(0.98f, 0.95f, 0.9f),  // #F8F2E5
    new Color(0.94f, 0.85f, 0.74f), // #F0D7BD
    new Color(0.88f, 0.75f, 0.62f), // #E0BF9E
    new Color(0.83f, 0.62f, 0.46f), // #D3A370
    new Color(0.65f, 0.47f, 0.32f),  // #A85C52
    new Color(0.49f, 0.30f, 0.18f), // #7D4C2F
    new Color(0.35f, 0.22f, 0.15f), // #5A3825
    new Color(0.24f, 0.14f, 0.08f), // #3D2314
    new Color(0.15f, 0.07f, 0.05f)  // #26110D
};

        if (index < 0 || index >= skinColors.Length) return;
        avatar.SetColor("Skin", skinColors[index]);
        avatar.UpdateColors(true);
    }
public void ChangeEyeColor(int index)
    {
        // Define your predefined eye colors
        Color[] eyeColors = new Color[]
{
    new Color(0.68f, 0.84f, 0.90f),  // #ADD8E6 (Light Blue)
    new Color(0f, 0f, 1f),           // #0000FF (Blue)
    new Color(0.74f, 0.74f, 0.74f),  // #BEBEBE (Gray)
    new Color(0f, 1f, 0f),           // #00FF00 (Green)
    new Color(0.56f, 0.46f, 0.09f),  // #8E7618 (Hazel)
    new Color(0.77f, 0.64f, 0.52f),  // #C4A484 (Light Brown)
    new Color(0.44f, 0.31f, 0.22f),  // #6F4E37 (Brown)
    new Color(1f, 0.75f, 0f),        // #FFBF00 (Amber)
    new Color(0.54f, 0.17f, 0.89f),  // #8A2BE2 (Violet)
    new Color(0f, 0f, 0f)            // #000000 (Black)
};

        if (index < 0 || index >= eyeColors.Length) return;
        avatar.SetColor("Eyes", eyeColors[index]);
        avatar.UpdateColors(true);
    }
    public void ChangeHairColor(int index)
    {
        // Define your predefined eye colors
        Color[] hairColors = new Color[]
{
    new Color(1.0f, 1.0f, 1.0f),         // #FFFFFF (White)
    new Color(0.74f, 0.74f, 0.74f),      // #BEBEBE (Gray)
    new Color(0.5f, 0.5f, 0.5f),         // #808080 (Silver)
    new Color(0.87f, 0.72f, 0.53f),      // #DEC6AA (Dirty Blonde)
    new Color(0.77f, 0.64f, 0.52f),      // #C4A484 (Light Brown)
    new Color(0.93f, 0.53f, 0.18f),      // #ED8E32 (Copper)
    new Color(0.82f, 0.41f, 0.12f),      // #D2691E (Chestnut)
    new Color(0.55f, 0.27f, 0.07f),       // #8B4513 (Dark Brown)
    new Color(0.44f, 0.31f, 0.22f),      // #6F4E37 (Brown)
    new Color(0.0f, 0.0f, 0.0f),         // #000000 (Black)
    new Color(1.0f, 0.75f, 0.8f),        // #FFBFD0 (Pastel Pink)
    new Color(0.8f, 0.47f, 0.65f),       // #CD79A4 (Mauve)
    new Color(1.0f, 0.84f, 0.0f),        // #FFD700 (Blonde)
    new Color(1.0f, 0.0f, 0.0f),         // #FF0000 (Red)
    new Color(0.7f, 0.13f, 0.13f),       // #B22222 (Auburn)  
    new Color(0.0f, 1.0f, 0.0f),         // #00FF00 (Green)
    new Color(0.68f, 0.84f, 0.90f),      // #ADD8E6 (Light Blue)
    new Color(0.0f, 0.0f, 1.0f),         // #0000FF (Blue)
    new Color(0.54f, 0.17f, 0.89f),      // #8A2BE2 (Violet)
    new Color(0.29f, 0.0f, 0.51f)       // #4B0082 (Indigo)    
};

        if (index < 0 || index >= hairColors.Length) return;
        avatar.SetColor("Hair", hairColors[index]);
        avatar.UpdateColors(true);
    }
}
