//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/SYSTEM/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Inputs"",
            ""id"": ""02a9caf8-a00d-4959-9691-ae87c7d2ca3e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""daf41ca6-87c1-46f1-a54b-a845bcb66112"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""dc22f7c9-c906-44d6-a39b-8b9d1603f26f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseAim"",
                    ""type"": ""Value"",
                    ""id"": ""2349ac66-8fda-4fd5-99d4-88e6126b701b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""StickAim"",
                    ""type"": ""Value"",
                    ""id"": ""5d448300-c42d-486b-a2ee-4ef9026cafbc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""fa7396ed-d26a-4cb6-99f3-c99fc1449d0b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ea4488e6-7c84-4ae1-a54e-97dfea07759d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""18be0ee7-087a-4dca-9f6e-8202215cc958"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c2aa8bbe-0a6a-4c0d-8e71-3eed4120b9b6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""68b871d3-8840-4c54-93c4-84646831031d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d3f9f209-fa49-40e4-92e5-5e05fe105595"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""711ed00b-c7bc-416b-b147-f3aa2654bff0"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b04a0248-c6ff-4ce2-a689-ef85d04a21b9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""817c4273-603d-4078-b243-20171bf69b9e"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""318533a5-fa7e-4f11-8281-3b448ec75860"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fab34219-edbd-4a92-b127-e2746784f661"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StickAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Inputs
        m_Inputs = asset.FindActionMap("Inputs", throwIfNotFound: true);
        m_Inputs_Move = m_Inputs.FindAction("Move", throwIfNotFound: true);
        m_Inputs_Shoot = m_Inputs.FindAction("Shoot", throwIfNotFound: true);
        m_Inputs_MouseAim = m_Inputs.FindAction("MouseAim", throwIfNotFound: true);
        m_Inputs_StickAim = m_Inputs.FindAction("StickAim", throwIfNotFound: true);
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

    // Inputs
    private readonly InputActionMap m_Inputs;
    private List<IInputsActions> m_InputsActionsCallbackInterfaces = new List<IInputsActions>();
    private readonly InputAction m_Inputs_Move;
    private readonly InputAction m_Inputs_Shoot;
    private readonly InputAction m_Inputs_MouseAim;
    private readonly InputAction m_Inputs_StickAim;
    public struct InputsActions
    {
        private @PlayerControls m_Wrapper;
        public InputsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Inputs_Move;
        public InputAction @Shoot => m_Wrapper.m_Inputs_Shoot;
        public InputAction @MouseAim => m_Wrapper.m_Inputs_MouseAim;
        public InputAction @StickAim => m_Wrapper.m_Inputs_StickAim;
        public InputActionMap Get() { return m_Wrapper.m_Inputs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InputsActions set) { return set.Get(); }
        public void AddCallbacks(IInputsActions instance)
        {
            if (instance == null || m_Wrapper.m_InputsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InputsActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @MouseAim.started += instance.OnMouseAim;
            @MouseAim.performed += instance.OnMouseAim;
            @MouseAim.canceled += instance.OnMouseAim;
            @StickAim.started += instance.OnStickAim;
            @StickAim.performed += instance.OnStickAim;
            @StickAim.canceled += instance.OnStickAim;
        }

        private void UnregisterCallbacks(IInputsActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @MouseAim.started -= instance.OnMouseAim;
            @MouseAim.performed -= instance.OnMouseAim;
            @MouseAim.canceled -= instance.OnMouseAim;
            @StickAim.started -= instance.OnStickAim;
            @StickAim.performed -= instance.OnStickAim;
            @StickAim.canceled -= instance.OnStickAim;
        }

        public void RemoveCallbacks(IInputsActions instance)
        {
            if (m_Wrapper.m_InputsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInputsActions instance)
        {
            foreach (var item in m_Wrapper.m_InputsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InputsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InputsActions @Inputs => new InputsActions(this);
    public interface IInputsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnMouseAim(InputAction.CallbackContext context);
        void OnStickAim(InputAction.CallbackContext context);
    }
}
