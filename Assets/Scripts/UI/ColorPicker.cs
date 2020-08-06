using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

public class ColorPicker : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform ColorWheel;
    public Image ColorWheelImage;
    public Outline ColorWheelOutline;
    public RectTransform ColorWheelSelector;
    public int ColorWheelSelectorRange;
    public RectTransform VSlider;
    public Image VSliderImage;
    public RectTransform VSliderSelector;
    public int VSliderSelectorRange;
    public TMP_InputField[] ColorFields;
    //public int InputMethod = 0; // 0:RGB, 1:HSV, 2:Hexadecimal
    public Image[] SavedColors;

    public Color CurrentColor;

    public static ColorChangedEvent colorChanged = new ColorChangedEvent();

    public bool DraggingWheelSelector = false;
    public bool DraggingVSliderSelector = false;

    private void Start() {
        SaveColors(SettingsManager.Settings.SavedColors);
    }

    private void Update() {
        float scaleFactor = canvas.scaleFactor;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (DraggingWheelSelector) {
            Vector2 startPos = ColorWheel.position;
            Vector2 targetPos = mousePosition;
            float dist = Vector2.Distance(startPos, targetPos);

            float range = ColorWheelSelectorRange * scaleFactor;
            if (dist > range) {
                Vector2 fromOriginToObject = targetPos - startPos;
                fromOriginToObject *= range / dist;
                targetPos = startPos + fromOriginToObject;
            }

            ColorWheelSelector.position = targetPos;
        }

        if (DraggingVSliderSelector) {
            float range = VSliderSelectorRange * scaleFactor;
            VSliderSelector.position = new Vector2(VSlider.position.x, Mathf.Clamp(mousePosition.y, VSlider.position.y - range, VSlider.position.y + range));
        }

        if (DraggingWheelSelector || DraggingVSliderSelector) {
            CalculateColor();
        }
    }

    // calculate color based on selector positions
    public void CalculateColor () {
        float h = getColorAngle().Round(10)/360;
        float s = Vector2.Distance(Vector2.zero, ColorWheelSelector.anchoredPosition) / ColorWheelSelectorRange;
        s = s.Round(100);
        float v = (VSliderSelector.anchoredPosition.y + VSliderSelectorRange) / (VSliderSelectorRange * 2);
        v = v.Round(100);

        ColorWheelImage.color = new Color(v,v,v);
        ColorWheelOutline.effectColor = Color.HSVToRGB(0, 0, v / 2);
        VSliderImage.color = Color.HSVToRGB(h, s, 1);
        CurrentColor = Color.HSVToRGB(h, s, v);

        // set inputfields
        //rgb
        ColorFields[0].SetTextWithoutNotify(((int)(CurrentColor.r * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[1].SetTextWithoutNotify(((int)(CurrentColor.g * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[2].SetTextWithoutNotify(((int)(CurrentColor.b * 255)).ToString(CultureInfo.InvariantCulture));
        //hsv
        ColorFields[3].SetTextWithoutNotify(((int)(h * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[4].SetTextWithoutNotify(((int)(s * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[5].SetTextWithoutNotify(((int)(v * 255)).ToString(CultureInfo.InvariantCulture));
        //hex
        ColorFields[6].SetTextWithoutNotify(ColorUtility.ToHtmlStringRGB(CurrentColor));

        // invoke event
        colorChanged.Invoke(CurrentColor);
    }

    // calculate selector positions based on color
    public void CalculateSelectorPositions (bool invokeColorChangedEvent = true) {
        float h, s, v;
        Color.RGBToHSV(CurrentColor, out h, out s, out v);

        // set h and s
        float angle = h * 360;
        float rad = angle * (Mathf.PI / 180);
        Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        ColorWheelSelector.anchoredPosition = pos * (ColorWheelSelectorRange * s);
        ColorWheelImage.color = new Color(v, v, v);
        ColorWheelOutline.effectColor = Color.HSVToRGB(0, 0, v / 2);

        // set v
        VSliderSelector.anchoredPosition = new Vector2(0,v * (VSliderSelectorRange * 2) - VSliderSelectorRange);
        VSliderImage.color = Color.HSVToRGB(h, s, 1);

        // set inputfields
        //rgb
        ColorFields[0].SetTextWithoutNotify(((int)(CurrentColor.r * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[1].SetTextWithoutNotify(((int)(CurrentColor.g * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[2].SetTextWithoutNotify(((int)(CurrentColor.b * 255)).ToString(CultureInfo.InvariantCulture));
        //hsv
        ColorFields[3].SetTextWithoutNotify(((int)(h * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[4].SetTextWithoutNotify(((int)(s * 255)).ToString(CultureInfo.InvariantCulture));
        ColorFields[5].SetTextWithoutNotify(((int)(v * 255)).ToString(CultureInfo.InvariantCulture));
        //hex
        ColorFields[6].SetTextWithoutNotify(ColorUtility.ToHtmlStringRGB(CurrentColor));

        // invoke event
        if (invokeColorChangedEvent)
            colorChanged.Invoke(CurrentColor);
    }

    // set color to input field values
    public void UpdateColorFromInputfield (int InputMethod) {
        if (InputMethod == 0) {
            // parse values
            int r, g, b;
            try { r = int.Parse(ColorFields[0].text, CultureInfo.InvariantCulture); } catch (FormatException) { r = 0; ColorFields[0].SetTextWithoutNotify("0"); }
            try { g = int.Parse(ColorFields[1].text, CultureInfo.InvariantCulture); } catch (FormatException) { g = 0; ColorFields[1].SetTextWithoutNotify("0"); }
            try { b = int.Parse(ColorFields[2].text, CultureInfo.InvariantCulture); } catch (FormatException) { b = 0; ColorFields[2].SetTextWithoutNotify("0"); }

            // clamp values
            r = Mathf.Clamp(r, 0, 255);
            g = Mathf.Clamp(g, 0, 255);
            b = Mathf.Clamp(b, 0, 255);

            // set values
            CurrentColor.r = r / 255f;
            CurrentColor.g = g / 255f;
            CurrentColor.b = b / 255f;
        } else if (InputMethod == 1) {
            // parse values
            int h, s, v;
            try { h = int.Parse(ColorFields[3].text, CultureInfo.InvariantCulture); } catch (FormatException) { h = 0; ColorFields[3].SetTextWithoutNotify("0"); }
            try { s = int.Parse(ColorFields[4].text, CultureInfo.InvariantCulture); } catch (FormatException) { s = 0; ColorFields[4].SetTextWithoutNotify("0"); }
            try { v = int.Parse(ColorFields[5].text, CultureInfo.InvariantCulture); } catch (FormatException) { v = 0; ColorFields[5].SetTextWithoutNotify("0"); }

            // clamp values
            h = Mathf.Clamp(h, 0, 255);
            s = Mathf.Clamp(s, 0, 255);
            v = Mathf.Clamp(v, 0, 255);

            // set values
            CurrentColor = Color.HSVToRGB(h/255f, s/255f, v/255f);
        } else {
            // parse and set
            if(ColorUtility.TryParseHtmlString("#" + ColorFields[6].text, out Color hexColor)) {
                hexColor.a = 1;
                CurrentColor = hexColor;
            } else {
                CurrentColor = Color.white;
            }
        }

        // update selector positions
        CalculateSelectorPositions();
    }

    public void SetColor (Color color, bool invokeColorChangedEvent) {
        CurrentColor = color;
        CalculateSelectorPositions(invokeColorChangedEvent);
    }

    public void SaveCurrentColor () {
        // shift all colors
        for (int i = SavedColors.Length-1; i >= 0; i--) {
            if (i == 0) continue; // skip first color
            SavedColors[i].color = SavedColors[i-1].color;
        }
        SavedColors[0].color = CurrentColor;
        SaveSettings();
    }

    public void SaveColors (Color[] colors) {
        for (int i = 0; i < colors.Length; i++) {
            if (i >= 8) break;
            SavedColors[i].color = colors[i];
        }
    }

    public void SelectSavedColor (Image image) {
        SetColor(image.color, true);
    }

    public Color[] GetSavedColors () {
        Color[] colors = new Color[8];
        for (int i = 0; i < SavedColors.Length; i++) {
            colors[i] = SavedColors[i].color;
        }
        return colors;
    }

    public void PressingWheelSelector (bool value) {
        DraggingWheelSelector = value;
    }

    public void PressingVSliderSelector (bool value) {
        DraggingVSliderSelector = value;
    }

    public void SaveSettings (bool saveToFile = true) {
        SettingsManager.Settings.SavedColors = GetSavedColors();
        if (saveToFile) SettingsManager.SaveSettings();
    }

    private float getColorAngle () {
        float rad = Mathf.Atan2(ColorWheelSelector.anchoredPosition.y, ColorWheelSelector.anchoredPosition.x);
        return (rad * (180f / Mathf.PI) + 360) % 360; // convert radians to degrees with a range of 0-360
    }
}

public class ColorChangedEvent : UnityEvent<Color> { }
