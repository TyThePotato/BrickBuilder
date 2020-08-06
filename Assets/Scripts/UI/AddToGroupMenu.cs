using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddToGroupMenu : MonoBehaviour
{
    public TMP_Dropdown GroupDropdown;
    public TMP_InputField GroupInputField;
    public GameObject ExistingGroupCover;

    public bool AddToNewGroup () {
        return !string.IsNullOrWhiteSpace(GroupInputField.text);
    }

    public void Show (bool show) {
        gameObject.SetActive(show);

        if (show == false) {
            DisableExistingGroup();
            EditorUI.instance.SetInputEnabled(true);
        }
    }

    public void DisableExistingGroup () {
        ExistingGroupCover.SetActive(true);
    }
}
