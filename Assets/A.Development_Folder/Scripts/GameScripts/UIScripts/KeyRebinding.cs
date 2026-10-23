using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyRebinder : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;

    private const string SaveKey = "Rebinds";

    private InputActionRebindingExtensions.RebindingOperation
        rebindOperation;

    private void Awake()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");

        if (!string.IsNullOrEmpty(json))
            actions.LoadBindingOverridesFromJson(json);
    }

    public void StartRebind(string actionName, int bindingIndex, TMP_Text keyLabel)
    {
        if (rebindOperation != null)
        {
            return;
        }

        InputAction action = actions.FindAction(actionName);

        if (action == null)
        {
            Debug.LogError($"Action not found: {actionName}");
            return;
        }

        keyLabel.text = "Press a key...";

        bool wasEnabled = action.enabled;

        if (wasEnabled)
        {
            action.Disable();
        }

        rebindOperation = action
            .PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();
                rebindOperation = null;

                if (wasEnabled)
                {
                    action.Enable();
                }

                keyLabel.text = action.GetBindingDisplayString(bindingIndex);

                PlayerPrefs.SetString(SaveKey, actions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                rebindOperation = null;

                if (wasEnabled)
                {
                    action.Enable();
                }

                keyLabel.text = action.GetBindingDisplayString(bindingIndex);
            });

        rebindOperation.Start();
    }

    public void RefreshLabel(string actionName, int bindingIndex, TMP_Text keyLabel)
    {
        InputAction action = actions.FindAction(actionName);

        if (action != null)
        {
            keyLabel.text = action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void OnDestroy()
    {
        if (rebindOperation != null)
        {
            rebindOperation.Cancel();
            rebindOperation.Dispose();
            rebindOperation = null;
        }
    }
}