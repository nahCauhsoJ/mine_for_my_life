// GENERATED AUTOMATICALLY FROM 'Assets/Input/Control.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Control : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Control()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Control"",
    ""maps"": [
        {
            ""name"": ""AimBar"",
            ""id"": ""8d86f4fa-b6d8-474e-8be1-75f45e163343"",
            ""actions"": [
                {
                    ""name"": ""StopBar"",
                    ""type"": ""Button"",
                    ""id"": ""5abbc5a4-093d-42c0-9ccc-2d5f13b7ca4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""06773e02-3132-45b1-9875-ff21ae2a51d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6a209ff9-8014-4f81-968d-a8b65526e938"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StopBar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""017cf544-c904-4758-94fb-bb52b30c2214"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // AimBar
        m_AimBar = asset.FindActionMap("AimBar", throwIfNotFound: true);
        m_AimBar_StopBar = m_AimBar.FindAction("StopBar", throwIfNotFound: true);
        m_AimBar_Pause = m_AimBar.FindAction("Pause", throwIfNotFound: true);
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

    // AimBar
    private readonly InputActionMap m_AimBar;
    private IAimBarActions m_AimBarActionsCallbackInterface;
    private readonly InputAction m_AimBar_StopBar;
    private readonly InputAction m_AimBar_Pause;
    public struct AimBarActions
    {
        private @Control m_Wrapper;
        public AimBarActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @StopBar => m_Wrapper.m_AimBar_StopBar;
        public InputAction @Pause => m_Wrapper.m_AimBar_Pause;
        public InputActionMap Get() { return m_Wrapper.m_AimBar; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AimBarActions set) { return set.Get(); }
        public void SetCallbacks(IAimBarActions instance)
        {
            if (m_Wrapper.m_AimBarActionsCallbackInterface != null)
            {
                @StopBar.started -= m_Wrapper.m_AimBarActionsCallbackInterface.OnStopBar;
                @StopBar.performed -= m_Wrapper.m_AimBarActionsCallbackInterface.OnStopBar;
                @StopBar.canceled -= m_Wrapper.m_AimBarActionsCallbackInterface.OnStopBar;
                @Pause.started -= m_Wrapper.m_AimBarActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_AimBarActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_AimBarActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_AimBarActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StopBar.started += instance.OnStopBar;
                @StopBar.performed += instance.OnStopBar;
                @StopBar.canceled += instance.OnStopBar;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public AimBarActions @AimBar => new AimBarActions(this);
    public interface IAimBarActions
    {
        void OnStopBar(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
