using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorManager : NetworkBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Color UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TMP_InputField _inputHEX;

    [Header("Edit Name UI")]
    [SerializeField] private GameObject _nameText;
    [SerializeField] private GameObject _inputName;
    [SerializeField] private Button _editButton;
    private bool switcher = true;

    [Header("Player")]
    [SerializeField] private Material _playerMaterial;

    [Header("Pallete")]
    [SerializeField] private RawImage _paletteImage;
    [SerializeField] private GameObject _pointer;
    private Texture2D _colorTexture;

    [Header("Sync")]
    [SerializeField] private LobbyManager _lobbyManager;
    [SerializeField] private int _slotIndex;

    private void Start()
    {
        _colorTexture = GenerateColorTexture(256, 256);
        _paletteImage.texture = _colorTexture;
        _playerMaterial.color = Color.white;
    }

    #region Color
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_panel.activeSelf) SelectColor(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_panel.activeSelf) SelectColor(eventData);
    }

    //Pointer Color Switch Method
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
            ColorChange(selectedColor);
        }
    }

    //HEX Color Switch Method
    public void HEXColorSwitch()
    {
        var newcolor = ColorUtility.TryParseHtmlString("#"+_inputHEX.text, out Color color);
        if (newcolor)
        {
            Debug.Log("Try Parse");
            ColorChange(color); 
        }
        else
        {
            Debug.Log("Error");
        }
    }
    
    private void ColorChange(Color newColor)
    {
        _buttonImage.color = newColor;
        _playerMaterial.color = newColor;
        _lobbyManager._roomManager.roomSlots[_slotIndex].GetComponent<RoomPlayerUI>().CmdSetColor(ColorUtility.ToHtmlStringRGB(newColor));
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

    public void ColorUIActive(bool switcher)
    {
        _panel.SetActive(switcher);
    }

    

    #endregion

    #region Edit Name

    public void EditNameUISwitcher()
    {
        _nameText.SetActive(!switcher);
        _inputName.SetActive(switcher);
        if (switcher)
        {
            //swap image
        }
        else
        {
            //swap image
            SwitchName();
        }

        switcher = !switcher;
    }

    private void SwitchName()
    {
        _lobbyManager._roomManager.roomSlots[_slotIndex].GetComponent<RoomPlayerUI>().CmdSetName(_inputName.GetComponent<TMP_InputField>().text);
    }

    #endregion
}
