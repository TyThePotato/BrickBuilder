using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorElement : MonoBehaviour
{
    public TMP_InputField[] InputFields; // set if applicable
    public Toggle toggle; // set if applicable
    public TMP_Dropdown dropdown; // set if applicable
    public Image image; // set if applicable
    public TMP_Text extraLabel; // set if applicable
    public InspectorElementType Type;

    // inputfield
    public void SetString (string input) {
        InputFields[0].SetTextWithoutNotify(input);
    }

    // inputfield
    public string GetString () {
        return InputFields[0].text;
    }

    // inputfield
    public void SetInt (int input) {
        InputFields[0].SetTextWithoutNotify(input.ToString(CultureInfo.InvariantCulture));
    }

    // inputfield
    public int GetInt () {
        if (string.IsNullOrWhiteSpace(InputFields[0].text)) return 0;
        return int.Parse(InputFields[0].text, CultureInfo.InvariantCulture);
    }

    // inputfield[]
    public void SetVector3(Vector3 input) {
        InputFields[0].SetTextWithoutNotify(input.x.ToString(CultureInfo.InvariantCulture));
        InputFields[1].SetTextWithoutNotify(input.y.ToString(CultureInfo.InvariantCulture));
        InputFields[2].SetTextWithoutNotify(input.z.ToString(CultureInfo.InvariantCulture));
    }

    // inputfield[]
    public Vector3 GetVector3() {
        return new Vector3(float.Parse(InputFields[0].text, CultureInfo.InvariantCulture), float.Parse(InputFields[1].text, CultureInfo.InvariantCulture), float.Parse(InputFields[2].text, CultureInfo.InvariantCulture));
    }

    public void SetColor (Color input, bool ignoreAlpha = false) {
        if (ignoreAlpha) {
            Color alphant = input;
            alphant.a = 1f;
            image.color = alphant;
        } else {
            image.color = input;
        }
    }

    public Color GetColor () {
        return image.color;
    }

    // inputfield
    public void SetFloat(float input) {
        InputFields[0].SetTextWithoutNotify(input.ToString(CultureInfo.InvariantCulture));
    }

    // inputfield
    public float GetFloat() {
        if (string.IsNullOrWhiteSpace(InputFields[0].text)) return 0f;
        return float.Parse(InputFields[0].text, CultureInfo.InvariantCulture);
    }

    // toggle
    public void SetBool (bool input) {
        toggle.SetIsOnWithoutNotify(input);
    }

    // toggle
    public bool GetBool () {
        return toggle.isOn;
    }

    // dropdown
    public void SetDropdown (int input) {
        dropdown.SetValueWithoutNotify(input);
    }

    // dropdown
    public int GetDropdown () {
        return dropdown.value;
    }

    public void SetExtraLabel (string input) {
        extraLabel.SetText(input);
    }

    public enum InspectorElementType {
        String,
        Int,
        Vector3,
        Color,
        Float,
        Bool,
        Dropdown,
        BoolAndFloat
    }
}
