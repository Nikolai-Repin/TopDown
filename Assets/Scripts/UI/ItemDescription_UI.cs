using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescription_UI : MonoBehaviour
{
    public GameObject descriptionPanel;

    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;

    public void Awake() {
        ResetDescription();
    }

    public void Update() {
        // Press B to toggle on Item Description Panel
        if(Input.GetKeyDown(KeyCode.B)) {
            ToggleDescriptionUI();
        }
    }

    public void ToggleDescriptionUI() {
        if(descriptionPanel != null) {
            if(!descriptionPanel.activeSelf) {
                descriptionPanel.SetActive(true);
                ResetDescription();
            }
            else {
                descriptionPanel.SetActive(false);
            }
        }
    }

    public void ResetDescription() {
        this.itemImage.gameObject.SetActive(false);
        this.title.text = "";
        this.description.text = "";
    }

    public void SetDescription(Sprite sprite, string itemName, string itemDescription) {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.title.text = itemName;
        this.description.text = itemDescription;
    }
}
