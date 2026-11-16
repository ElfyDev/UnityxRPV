using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float targetScale = 1.05f;
    public float animationDuration = 0.15f;

    public Color hoverTextColor = new Color(0.333f, 0.667f, 0.333f, 1f);
    public Color normalTextColor = new Color(0.8f, 0.8f, 0.8f, 1f);

    public Sprite HoverIcon;
    public Sprite NormalIcon;

    private Vector3 _originalScale;
    private Text _legacyText;
    private TMP_Text _tmpText;
    private Image _buttonImage;
    private Coroutine _animationCoroutine;

    [SerializeField] private AudioClip hover;
    void Awake()
    {
        _originalScale = transform.localScale;

        _buttonImage = GetComponent<Image>();

        _legacyText = GetComponentInChildren<Text>();
        _tmpText = GetComponentInChildren<TMP_Text>();

        if (_legacyText != null)
        {
            _legacyText.color = normalTextColor;
        }
        else if (_tmpText != null)
        {
            _tmpText.color = normalTextColor;
        }

        if (_buttonImage != null && NormalIcon != null)
        {
            _buttonImage.sprite = NormalIcon;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        _animationCoroutine = StartCoroutine(AnimateButton(true));

        if (_buttonImage != null && HoverIcon != null)
        {
            _buttonImage.sprite = HoverIcon;
            SoundFXManager.instance.playSFXClip(hover, transform);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        _animationCoroutine = StartCoroutine(AnimateButton(false));

        if (_buttonImage != null && NormalIcon != null)
        {
            _buttonImage.sprite = NormalIcon;
        }
    }

    private IEnumerator AnimateButton(bool isHovering)
    {
        float elapsedTime = 0f;

        Vector3 finalScale = isHovering ? _originalScale * targetScale : _originalScale;
        Color finalColor = isHovering ? hoverTextColor : normalTextColor;

        Vector3 startingScale = transform.localScale;

        Color startingColor = normalTextColor;
        if (_legacyText != null) startingColor = _legacyText.color;
        else if (_tmpText != null) startingColor = _tmpText.color;


        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            transform.localScale = Vector3.Lerp(startingScale, finalScale, t);

            if (_legacyText != null)
            {
                _legacyText.color = Color.Lerp(startingColor, finalColor, t);
            }
            else if (_tmpText != null)
            {
                _tmpText.color = Color.Lerp(startingColor, finalColor, t);
            }

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = finalScale;
        if (_legacyText != null)
        {
            _legacyText.color = finalColor;
        }
        else if (_tmpText != null)
        {
            _tmpText.color = finalColor;
        }

        _animationCoroutine = null;
    }
}
