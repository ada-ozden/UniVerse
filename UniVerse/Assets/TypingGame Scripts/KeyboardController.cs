using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private Image[] keyImages;
    [SerializeField] private Sprite[] whiteKeySprites;
    private Dictionary<string, Image> keyImageDictionary = new Dictionary<string, Image>();
    private Dictionary<string, Sprite> whiteKeyDictionary = new Dictionary<string, Sprite>();

    private void Start()
    {
        // Image ve keys eşleştir
        foreach (Image image in keyImages)
        {
            keyImageDictionary[image.name.ToUpper()] = image;
        }

        // Beyaz tuşları eşleştir
        foreach (Sprite sprite in whiteKeySprites)
        {
            whiteKeyDictionary[sprite.name.ToUpper()] = sprite;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    string keyName = keyCode.ToString();
                    if (keyImageDictionary.TryGetValue(keyName, out Image image) &&
                        whiteKeyDictionary.TryGetValue(keyName, out Sprite whiteSprite))
                    {
                        StartCoroutine(ChangeKeyColorTemporary(image, whiteSprite));
                    }
                }
            }
        }
    }

    private IEnumerator ChangeKeyColorTemporary(Image keyImage, Sprite whiteSprite)
    {
        Sprite originalSprite = keyImage.sprite;
        keyImage.sprite = whiteSprite;

        yield return new WaitForSeconds(0.3f); // 0.5 saniye sonra eski haline döner

        keyImage.sprite = originalSprite;
    }
}