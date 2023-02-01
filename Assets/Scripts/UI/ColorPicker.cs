using BrickBuilder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BrickBuilder.UI
{
    public class ColorPicker : MonoBehaviour
    {
        public static ColorPicker instance;

        public static Color CurrentColor;
        public static Color[] SavedColors;

        public static ColorPickerMode CurrentMode;

        // Images
        [Space(5)]
        [Header("Images")]
        public Image ColorWheelBackground;
        public Image BrightnessSliderBackground;
        public Image[] SavedColorImages;
        
        // HSV Controls
        [Space(5)]
        [Header("HSV Controls")]
        public RectTransform ColorDot; // Controls H and S
        public Slider BrightnessSlider; // Controls V

        public TMP_InputField HueField;
        public TMP_InputField SaturationField;
        public TMP_InputField ValueField;
        
        // RGBA Controls
        [Space(5)]
        [Header("RGBA Controls")]
        public TMP_InputField RedField;
        public TMP_InputField GreenField;
        public TMP_InputField BlueField;
        public TMP_InputField AlphaField;
        public Slider AlphaSlider;
        
        // Hex Controls
        [Space(5)]
        [Header("Hex Controls")]
        public TMP_InputField HexField;

        private static Action<Color, ColorPickerMode> _colorChangedCallback;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            CurrentColor = new Color(1, 1, 1, 1);
            SavedColors = new Color[16];
        }

        private void Start()
        {
            UpdateColorWheel();
            UpdateAlpha();
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            HideColorPicker();
        }

        public static void ShowColorPicker(Color startColor, ColorPickerMode mode, Action<Color, ColorPickerMode> colorChangedCallback)
        {
            ShowColorPicker(colorChangedCallback);
            
            // set color
            CurrentColor = startColor;
            
            // set mode
            CurrentMode = mode;
            
            // update fields
            instance.UpdateColorWheel();
            instance.UpdateAlpha();
            instance.UpdateRGB();
            instance.UpdateHSV();
            instance.UpdateHex();
        }
        
        public static void ShowColorPicker(Action<Color, ColorPickerMode> colorChangedCallback)
        {
            // set up callback for color change
            _colorChangedCallback = colorChangedCallback;
            
            // display color picker
            instance.gameObject.SetActive(true);
        }

        public static void HideColorPicker()
        {
            // remove callback
            _colorChangedCallback = null;
            
            // reset mode
            CurrentMode = ColorPickerMode.None;
            
            // hide picker
            instance.gameObject.SetActive(false);
        }
        
        // stupid unity button onclick cant be bound to static functions from inspector
        public void HideColorPickerUI()
        {
            HideColorPicker();
        }

        public static void LoadSavedColors(Color[] savedColors)
        {
            // Firstly set saved colors array
            if (savedColors.Length == 16)
            {
                SavedColors = savedColors;
            }
            else
            {
                Debug.LogWarning("WARNING: Saved Color Array Length Wrong! (" + savedColors.Length.ToString() +")");
            }

            // Next update saved colors preview
            for (int i = 0; i < SavedColors.Length; i++)
            {
                instance.SavedColorImages[i].color = SavedColors[i];
            }
        }

        // Calculates the current wheel color
        public void CalculateWheelColor()
        {
            float colorWheelSize = ColorWheelBackground.rectTransform.sizeDelta.x;
            Vector2 dotPosition = ColorDot.anchoredPosition / (colorWheelSize / 2f);

            // Hue = Angle created from (0,0), (1,0), and the dot's position
            float hue = Vector2.SignedAngle(Vector2.right, dotPosition).Modulo(360) / 360f;

            // Saturation = distance from dot to center of circle
            float saturation = Mathf.Clamp01(Vector2.Distance(dotPosition, Vector2.zero));

            // Value = value of the slider. ez :]
            float value = BrightnessSlider.value;
            
            // Set CurrentColor using calculated values
            float currentAlpha = CurrentColor.a;
            CurrentColor = Color.HSVToRGB(hue, saturation, value);
            CurrentColor.a = currentAlpha;
            
            // Recolor color stuff
            ColorWheelBackground.color = new Color(value, value, value);
            BrightnessSliderBackground.color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, 1f); // no alpha
        }

        // Called when color wheel "dot" is dragged
        public void ColorWheelDotDrag(BaseEventData data)
        {
            PointerEventData ped = (PointerEventData) data;

            // move dot
            float colorWheelSize = ColorWheelBackground.rectTransform.sizeDelta.x;
            Vector2 relativeMousePosition = ped.position - ((Vector2)ColorWheelBackground.rectTransform.position +
                                                            new Vector2(colorWheelSize / 2f, -colorWheelSize / 2f));
            ColorDot.anchoredPosition = relativeMousePosition;
            
            // clamp magnitude of anchored position
            ColorDot.anchoredPosition = Vector2.ClampMagnitude(ColorDot.anchoredPosition, colorWheelSize / 2f);
            
            // calculate new wheel color
            CalculateWheelColor();
            
            // update color values
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            ColorChanged();
        }

        // Updates the color wheel & brightness slider to reflect current color values
        public void UpdateColorWheel()
        {
            // get hsv values for easy calculations
            Color.RGBToHSV(CurrentColor, out float h, out float s, out float v);
            
            // calculate color dot H position using fancy pants trig
            float angle = h * Mathf.PI * 2; // angle in radians
            Vector2 dotPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            
            // calculate color dot S position by literally just multiplying dotPos by s
            dotPos *= s;

            // no need for any wild V calculations
            BrightnessSlider.SetValueWithoutNotify(v);
            
            // apply color dot position
            float colorWheelSize = ColorWheelBackground.rectTransform.sizeDelta.x;
            ColorDot.anchoredPosition = dotPos * (colorWheelSize / 2f);
            
            // Recolor color stuff
            ColorWheelBackground.color = new Color(v, v, v);
            BrightnessSliderBackground.color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, 1f); // no alpha
        }

        // Called when the brightness slider is changed
        public void BrightnessSliderChange()
        {
            CalculateWheelColor();
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            ColorChanged();
        }

        // Called when the alpha slider is changed
        public void AlphaSliderChange()
        {
            CurrentColor.a = AlphaSlider.value;
            UpdateAlpha();
            UpdateHex();
            
            ColorChanged();
        }

        // Called when the alpha inputfield is updated
        public void AlphaInput()
        {
            float alphaInput = ParseFieldText(AlphaField.text);
            if (alphaInput != -1.0f)
            {
                // :]
                CurrentColor.a = alphaInput;
            }

            UpdateAlpha();
            
            ColorChanged();
        }

        // Updates the alpha slider & inputfield to reflect current color values
        public void UpdateAlpha()
        {
            int alpha255 = (int)(CurrentColor.a * 255);
            AlphaField.SetTextWithoutNotify(alpha255.ToString());
            AlphaSlider.SetValueWithoutNotify(CurrentColor.a);;
        }

        // Called when the rgb inputfield is updated
        public void RGBInput()
        {
            float rInput = ParseFieldText(RedField.text);
            if (rInput != -1.0f)
            {
                CurrentColor.r = rInput;

            }

            float gInput = ParseFieldText(GreenField.text);
            if (gInput != -1.0f)
            {
                CurrentColor.g = gInput;

            }

            float bInput = ParseFieldText(BlueField.text);
            if (bInput != -1.0f)
            {
                CurrentColor.b = bInput;
            }

            UpdateColorWheel();
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            ColorChanged();
        }

        // updates rgb inputfield to reflect current color values
        public void UpdateRGB()
        {
            int r255 = (int) (CurrentColor.r * 255);
            int g255 = (int) (CurrentColor.g * 255);
            int b255 = (int) (CurrentColor.b * 255);
            
            RedField.SetTextWithoutNotify(r255.ToString());
            GreenField.SetTextWithoutNotify(g255.ToString());
            BlueField.SetTextWithoutNotify(b255.ToString());
        }

        // Called when the hsv inputfield is updated
        public void HSVInput()
        {
            float hInput = ParseFieldText(HueField.text);
            if (hInput == -1.0f)
            {
                // invalid
                hInput = 0f;

            }

            float sInput = ParseFieldText(SaturationField.text);
            if (sInput == -1.0f)
            {
                // invalid
                sInput = 1f;

            }

            float vInput = ParseFieldText(ValueField.text);
            if (vInput == -1.0f)
            {
                // Invalid
                vInput = 1f;
            }

            float currentAlpha = CurrentColor.a;
            CurrentColor = Color.HSVToRGB(hInput, sInput, vInput);
            CurrentColor.a = currentAlpha;

            UpdateColorWheel();
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            ColorChanged();
        }

        // updates hsv inputfield to reflect current color values
        public void UpdateHSV()
        {
            Color.RGBToHSV(CurrentColor, out float h, out float s, out float v);

            int h255 = (int) (h * 255);
            int s255 = (int) (s * 255);
            int v255 = (int) (v * 255);
            
            HueField.SetTextWithoutNotify(h255.ToString());
            SaturationField.SetTextWithoutNotify(s255.ToString());
            ValueField.SetTextWithoutNotify(v255.ToString());
        }

        // Called when the hex inputfield is updated
        public void HexInput()
        {
            ColorUtility.TryParseHtmlString(HexField.text, out CurrentColor);

            UpdateColorWheel();
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            ColorChanged();
        }

        // updates hex inputfield to reflect current color values
        public void UpdateHex()
        {
            string color = ColorUtility.ToHtmlStringRGBA(CurrentColor);
            HexField.SetTextWithoutNotify(color);
        }

        // Saves CurrentColor to first color slot
        public void SaveColor()
        {
            // shift all colors back
            for (int i = SavedColors.Length - 1; i > 0; i--)
            {
                SavedColors[i] = SavedColors[i - 1];
            }
            
            // set first color to newly saved color
            SavedColors[0] = CurrentColor;
            
            // update color previews
            for (int i = 0; i < SavedColors.Length; i++)
            {
                SavedColorImages[i].color = SavedColors[i];
            }
        }
        
        // Sets CurrentColor to selected color slot
        public void SelectSavedColor(int index)
        {
            CurrentColor = SavedColors[index];
            
            UpdateColorWheel();
            UpdateAlpha();
            UpdateRGB();
            UpdateHSV();
            UpdateHex();
            
            ColorChanged();
        }

        // Attempts to parse string to float, returns -1 if unsuccessful
        private float ParseFieldText(string text)
        {
            if (int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out int fieldInt))
            {
                fieldInt = Mathf.Clamp(fieldInt, 0, 255);
                return fieldInt / 255f;
            }
            else
            {
                return -1.0f;
            }
        }

        private void ColorChanged()
        {
            _colorChangedCallback?.Invoke(CurrentColor, CurrentMode);
        }
        
        public enum ColorPickerMode
        {
            None,
            Ambient,
            Baseplate,
            Sky,
            Brick,
            Paintbrush
        }
    }
}