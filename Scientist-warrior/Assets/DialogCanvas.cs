using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class DialogCanvas : MonoBehaviour
{
    public static DialogCanvas Instance;
    [SerializeField] private CanvasRenderer dialogPanel;
    [SerializeField] private Text dialogTextBox;
    [SerializeField] private Image dialogPanelCharacterImage,PlayerImage;
    [SerializeField] Sprite defaultImage;

    public void PrintMessage(string message,[CanBeNull] Sprite sprite, [CanBeNull] Sprite PlayerSprite)
    {
        if (sprite != null)
        {
            dialogPanelCharacterImage.sprite = sprite;
        }

        if (PlayerSprite != null)
        {
            PlayerImage.sprite = PlayerSprite;
        }
        dialogPanel.gameObject.SetActive(true);
        if (message == string.Empty)
            dialogTextBox.text = string.Empty;
        dialogTextBox.text = message;
    }

    public void ResetDialogPanel()
    {
        dialogTextBox.text = string.Empty;
        dialogPanelCharacterImage.sprite = defaultImage;
        dialogPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        Instance = this;
    }
}