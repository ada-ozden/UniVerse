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
}
