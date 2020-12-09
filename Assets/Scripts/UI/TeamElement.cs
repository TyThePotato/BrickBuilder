using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamElement : MonoBehaviour
{
    public string TeamName;
    public Color TeamColor;

    public Image ElementPanel;
    public TMP_InputField NameField;
    public Button ColorButton;
    public Image ColorImage;
    public Button DeleteButton;

    public void ApplySettings (bool onlyColor = false) {
        if (!onlyColor) NameField.SetTextWithoutNotify(TeamName);
        ElementPanel.color = new Color(TeamColor.r, TeamColor.g, TeamColor.b, 0.3f);
        ColorImage.color = TeamColor;
    }
}
