using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BrickBuilder.Exceptions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrickBuilder.UI
{
    public class InspectorElement : MonoBehaviour
    {
        public Value PropertyValue;

        // Strings, Numbers
        public TMP_InputField[] InputFields;
        
        // Boolean
        public Toggle Toggle;
        
        // Dropdown
        public TMP_Dropdown Dropdown;
        
        // Color
        public Image ColorPreview;
        public Button ColorButton;
        public ColorPicker.ColorPickerMode ColorType;
        
        // ====
        // Init
        // ====

        public void SetColorButtonCallback(Action<Color, ColorPicker.ColorPickerMode> callback)
        {
            ColorButton.onClick.AddListener(delegate
            {
                ColorPicker.ShowColorPicker(ColorPreview.color, ColorType, callback);
            });
        }

        // ===
        // Set
        // ===

        // String
        public void SetValue(string value)
        {
            if (PropertyValue != Value.String) return;
            
            InputFields[0].SetTextWithoutNotify(value);
        }

        // Int & Dropdown
        public void SetValue(int value)
        {
            if (PropertyValue == Value.Integer)
            {
                InputFields[0].SetTextWithoutNotify(value.ToString(CultureInfo.InvariantCulture));
            } else if (PropertyValue == Value.Dropdown)
            {
                Dropdown.SetValueWithoutNotify(value);
            }
        }
        
        // Float
        public void SetValue(float value)
        {
            if (PropertyValue != Value.Float) return;
            
            InputFields[0].SetTextWithoutNotify(value.ToString(CultureInfo.InvariantCulture));
        }
        
        // Vector3
        public void SetValue(Vector3 value)
        {
            if (PropertyValue != Value.Vector3) return;
            
            InputFields[0].SetTextWithoutNotify(value.x.ToString(CultureInfo.InvariantCulture));
            InputFields[1].SetTextWithoutNotify(value.y.ToString(CultureInfo.InvariantCulture));
            InputFields[2].SetTextWithoutNotify(value.z.ToString(CultureInfo.InvariantCulture));
        }

        // Vector3Int
        public void SetValue(Vector3Int value)
        {
            if (PropertyValue != Value.Vector3Int) return;
            
            InputFields[0].SetTextWithoutNotify(value.x.ToString(CultureInfo.InvariantCulture));
            InputFields[1].SetTextWithoutNotify(value.y.ToString(CultureInfo.InvariantCulture));
            InputFields[2].SetTextWithoutNotify(value.z.ToString(CultureInfo.InvariantCulture));
        }

        // Boolean
        public void SetValue(bool value)
        {
            if (PropertyValue != Value.Boolean) return;

            Toggle.isOn = value;
        }
        
        // Color
        public void SetValue(Color value)
        {
            if (PropertyValue != Value.Color) return;

            ColorPreview.color = value;
            Debug.Log(ColorPreview.color);
        }

        // ===
        // Get
        // ===

        public string GetString()
        {
            if (PropertyValue != Value.String)
            {
                throw new InspectorValueMismatchException();
            }
            
            return InputFields[0].text;
        }
        
        public int GetInteger()
        {
            if (PropertyValue != Value.Integer)
            {
                throw new InspectorValueMismatchException();
            }
            
            return int.Parse(InputFields[0].text, CultureInfo.InvariantCulture);
        }
        
        public float GetFloat()
        {
            if (PropertyValue != Value.Float)
            {
                throw new InspectorValueMismatchException();
            }
            
            return float.Parse(InputFields[0].text, CultureInfo.InvariantCulture);
        }
        
        public Vector3 GetVector3()
        {
            if (PropertyValue != Value.Vector3)
            {
                throw new InspectorValueMismatchException();
            }

            return new Vector3(
                float.Parse(InputFields[0].text, CultureInfo.InvariantCulture),
                float.Parse(InputFields[1].text, CultureInfo.InvariantCulture),
                float.Parse(InputFields[2].text, CultureInfo.InvariantCulture)
            );
        }
        
        public Vector3Int GetVector3Int()
        {
            if (PropertyValue != Value.Vector3Int)
            {
                throw new InspectorValueMismatchException();
            }

            return new Vector3Int(
                int.Parse(InputFields[0].text, CultureInfo.InvariantCulture),
                int.Parse(InputFields[1].text, CultureInfo.InvariantCulture),
                int.Parse(InputFields[2].text, CultureInfo.InvariantCulture)
            );
        }
        
        public bool GetBoolean()
        {
            if (PropertyValue != Value.Boolean)
            {
                throw new InspectorValueMismatchException();
            }

            return Toggle.isOn;
        }

        public int GetDropdown()
        {
            if (PropertyValue != Value.Dropdown)
            {
                throw new InspectorValueMismatchException();
            }

            return Dropdown.value;
        }

        public Color GetColor()
        {
            if (PropertyValue != Value.Color)
            {
                throw new InspectorValueMismatchException();
            }

            return ColorPreview.color;
        }
        
        // =====
        // Other
        // =====
        
        public enum Value
        {
            String,
            Integer,
            Float,
            Vector3,
            Vector3Int,
            Boolean,
            Dropdown,
            Color
        }
    }
}