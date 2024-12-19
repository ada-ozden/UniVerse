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
    private Dictionary<string, DnaSetter> dna;

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
        heightSlider.onValueChanged.AddListener(HeightChange);
        weightSlider.onValueChanged.AddListener(WeightChange);
        waistSlider.onValueChanged.AddListener(WaistChange);
    }

    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
        heightSlider.onValueChanged.RemoveListener(HeightChange);
        weightSlider.onValueChanged.RemoveListener(WeightChange);
        waistSlider.onValueChanged.RemoveListener(WaistChange);
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

    public void ChangeSkinColor(Color col)
    {
        avatar.SetColor("Skin", col); // Ensure the method name is correct for your UMA version
        avatar.UpdateColors(true);
    }
}
