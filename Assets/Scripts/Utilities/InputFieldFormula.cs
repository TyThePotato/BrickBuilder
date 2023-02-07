using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

using NCalc;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldFormula : MonoBehaviour {
    
    public NumberType numberType;
    public EvalEvent OnEvaluate = new EvalEvent();
    
    [NonSerialized]
    public TMP_InputField InputField;

    private void Awake() {
        InputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable() {
        InputField.onSubmit.AddListener(InputFieldOnSubmit);
    }

    private void OnDisable() {
        InputField.onSubmit.RemoveListener(InputFieldOnSubmit);
    }

    private void InputFieldOnSubmit(string content) {
        // attempt to evaluate formula
        string modifiedContent = content;
        try {
            // eval
            float result = Convert.ToSingle(new Expression(content).Evaluate());
            
            // disallow infinity/nan
            if (!float.IsFinite(result))
                result = 0f;
            
            // round if integer
            if (numberType == NumberType.Integer)
                result = Mathf.RoundToInt(result);
            
            // set content
            modifiedContent = result.ToString(CultureInfo.InvariantCulture);
        }
        catch (Exception e) { // TODO: only ignore expected exceptions
            // failed
            modifiedContent = "0";
        }

        // modify field
        InputField.SetTextWithoutNotify(modifiedContent);
        
        // invoke event finally
        OnEvaluate.Invoke();
    }
    
    public enum NumberType {
        Float,
        Integer
    }
}

[Serializable]
public class EvalEvent : UnityEvent { }