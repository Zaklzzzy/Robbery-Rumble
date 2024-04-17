using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorManager : NetworkBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _buttonImage;

    [Header("Player")]
    [SerializeField] private Material _playerMaterial;

    [Header("Pallete")]
    [SerializeField] private RawImage _paletteImage;
    [SerializeField] private GameObject _pointer;
    private Texture2D _colorTexture;

    [Header("Color Sync")]
    [SerializeField] private LobbyManager _lobbyManager;
    [SerializeField] private int _slotIndex;

    private void Start()
    {
        _colorTexture = GenerateColorTexture(256, 256);
        _paletteImage.texture = _colorTexture;
        _playerMaterial.color = Color.white;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_panel.activeSelf) SelectColor(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_panel.activeSelf) SelectColor(eventData);
    }

    private void SelectColor(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_paletteImage.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float x = Mathf.Clamp((localPoint.x / _paletteImage.rectTransform.rect.width) + 0.5f, 0f, 1f);
            float y = Mathf.Clamp((localPoint.y / _paletteImage.rectTransform.rect.height) + 0.5f, 0f, 1f);

            _pointer.transform.localPosition = localPoint;

            int texX = Mathf.RoundToInt(x * (_colorTexture.width - 1));
            int texY = Mathf.RoundToInt(y * (_colorTexture.height - 1));

            Color selectedColor = _colorTexture.GetPixel(texX, texY);
            _buttonImage.color = selectedColor;
            _playerMaterial.color = selectedColor;
            SyncColorChange(selectedColor);
        }
    }

    public void TogglePalette()
    {
        _paletteImage.gameObject.SetActive(!_paletteImage.gameObject.activeSelf);
    }

    private Texture2D GenerateColorTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float h = i / (float)width;
                float s = j / (float)height;
                Color color = Color.HSVToRGB(h, s, 1.0f);
                texture.SetPixel(i, j, color);
            }
        }
        texture.Apply();
        return texture;
    }

    public void UIActive(bool switcher)
    {
        _panel.SetActive(switcher);
    }

    private void SyncColorChange(Color newColor)
    {
        _lobbyManager._roomManager.roomSlots[_slotIndex].GetComponent<RoomPlayerUI>().CmdSetColor(ColorUtility.ToHtmlStringRGB(newColor));
    }
}
