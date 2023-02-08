//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.1
//     from Assets/Scripts/Input/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Controls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""0e68fb02-6e24-435e-ba5c-d5bf9ddaae71"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""2c2f374c-5aa7-43b1-bdd9-d498c7e3530f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Vertical"",
                    ""type"": ""Button"",
                    ""id"": ""bd9605d3-3c21-4f56-a3c2-d7c05c975181"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""ebccbfc1-cd49-4198-a838-49ad70ed5790"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""00c158f3-27cd-4eae-9b9c-9335e11b8c7c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""f3e34cbe-538c-4e60-9fe2-b8958d139b0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""3405ddfc-4cae-4bd6-99b2-5ee1a4b42b96"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LookButton"",
                    ""type"": ""Button"",
                    ""id"": ""81931c80-9aa4-4dd1-8f02-0e5d378eb0d1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PanButton"",
                    ""type"": ""Button"",
                    ""id"": ""e6cd546e-50df-430e-bae7-32c2b5d67534"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Selection Add"",
                    ""type"": ""Button"",
                    ""id"": ""b9972208-7ff4-4463-bb14-df902e701a4d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hotkey Modifier"",
                    ""type"": ""Button"",
                    ""id"": ""0d88c471-3a53-4069-9128-28350b184f5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""10737621-83f7-47d3-9b82-245270d05417"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""713b00a7-aa63-430e-8b8f-ff117cc9b476"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""48ebff9d-2df4-4b09-87b7-24566ee8ed8e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""78369b7b-916d-43e8-81db-9aa887ceb707"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""11c53e21-d126-4488-9beb-a9e7e1ef2704"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""QE"",
                    ""id"": ""e5083f7c-c7d0-47f3-a62c-875b013dacb8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""98495041-0ead-491b-b31d-36424f4cc9a7"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0e4b6d42-1d1c-47e3-94dd-e6ded19aa32e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""85a36fe6-b4f7-48ec-a12b-f4e7adb8e18b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62cc2c7e-a6e6-4438-a9b8-7a4a09ff6d7e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed576f39-9c59-438b-b6f0-2aa565a7c271"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e503e66f-50a6-4e9b-87aa-a5be3d5899c9"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3337828f-c8fe-4c3d-847d-84fb4cfb8e9c"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Add"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a6b492b-d4ce-4f02-94d7-fd62bd235041"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ebc5b57c-d70e-40f0-8b83-a02bba45eea6"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hotkey Modifier"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb04c80c-da32-4420-8a75-2c6325bb754e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PanButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_Movement = m_Main.FindAction("Movement", throwIfNotFound: true);
        m_Main_Vertical = m_Main.FindAction("Vertical", throwIfNotFound: true);
        m_Main_Look = m_Main.FindAction("Look", throwIfNotFound: true);
        m_Main_Boost = m_Main.FindAction("Boost", throwIfNotFound: true);
        m_Main_Select = m_Main.FindAction("Select", throwIfNotFound: true);
        m_Main_Cancel = m_Main.FindAction("Cancel", throwIfNotFound: true);
        m_Main_LookButton = m_Main.FindAction("LookButton", throwIfNotFound: true);
        m_Main_PanButton = m_Main.FindAction("PanButton", throwIfNotFound: true);
        m_Main_SelectionAdd = m_Main.FindAction("Selection Add", throwIfNotFound: true);
        m_Main_HotkeyModifier = m_Main.FindAction("Hotkey Modifier", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_Movement;
    private readonly InputAction m_Main_Vertical;
    private readonly InputAction m_Main_Look;
    private readonly InputAction m_Main_Boost;
    private readonly InputAction m_Main_Select;
    private readonly InputAction m_Main_Cancel;
    private readonly InputAction m_Main_LookButton;
    private readonly InputAction m_Main_PanButton;
    private readonly InputAction m_Main_SelectionAdd;
    private readonly InputAction m_Main_HotkeyModifier;
    public struct MainActions
    {
        private @Controls m_Wrapper;
        public MainActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Main_Movement;
        public InputAction @Vertical => m_Wrapper.m_Main_Vertical;
        public InputAction @Look => m_Wrapper.m_Main_Look;
        public InputAction @Boost => m_Wrapper.m_Main_Boost;
        public InputAction @Select => m_Wrapper.m_Main_Select;
        public InputAction @Cancel => m_Wrapper.m_Main_Cancel;
        public InputAction @LookButton => m_Wrapper.m_Main_LookButton;
        public InputAction @PanButton => m_Wrapper.m_Main_PanButton;
        public InputAction @SelectionAdd => m_Wrapper.m_Main_SelectionAdd;
        public InputAction @HotkeyModifier => m_Wrapper.m_Main_HotkeyModifier;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMovement;
                @Vertical.started -= m_Wrapper.m_MainActionsCallbackInterface.OnVertical;
                @Vertical.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnVertical;
                @Vertical.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnVertical;
                @Look.started -= m_Wrapper.m_MainActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnLook;
                @Boost.started -= m_Wrapper.m_MainActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnBoost;
                @Select.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSelect;
                @Cancel.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCancel;
                @LookButton.started -= m_Wrapper.m_MainActionsCallbackInterface.OnLookButton;
                @LookButton.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnLookButton;
                @LookButton.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnLookButton;
                @PanButton.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPanButton;
                @PanButton.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPanButton;
                @PanButton.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPanButton;
                @SelectionAdd.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionAdd;
                @SelectionAdd.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionAdd;
                @SelectionAdd.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionAdd;
                @HotkeyModifier.started -= m_Wrapper.m_MainActionsCallbackInterface.OnHotkeyModifier;
                @HotkeyModifier.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnHotkeyModifier;
                @HotkeyModifier.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnHotkeyModifier;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Vertical.started += instance.OnVertical;
                @Vertical.performed += instance.OnVertical;
                @Vertical.canceled += instance.OnVertical;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @LookButton.started += instance.OnLookButton;
                @LookButton.performed += instance.OnLookButton;
                @LookButton.canceled += instance.OnLookButton;
                @PanButton.started += instance.OnPanButton;
                @PanButton.performed += instance.OnPanButton;
                @PanButton.canceled += instance.OnPanButton;
                @SelectionAdd.started += instance.OnSelectionAdd;
                @SelectionAdd.performed += instance.OnSelectionAdd;
                @SelectionAdd.canceled += instance.OnSelectionAdd;
                @HotkeyModifier.started += instance.OnHotkeyModifier;
                @HotkeyModifier.performed += instance.OnHotkeyModifier;
                @HotkeyModifier.canceled += instance.OnHotkeyModifier;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    public interface IMainActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnVertical(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnLookButton(InputAction.CallbackContext context);
        void OnPanButton(InputAction.CallbackContext context);
        void OnSelectionAdd(InputAction.CallbackContext context);
        void OnHotkeyModifier(InputAction.CallbackContext context);
    }
}
