using TMPro;
using UnityEngine;

public class KeyBindingButton : MonoBehaviour
{
    [SerializeField] private KeyRebinder rebinder;
    [SerializeField] private string displayName;
    [SerializeField] private string actionName;
    [SerializeField] private int bindingIndex;
    [SerializeField] private TMP_Text actionLabel;
    [SerializeField] private TMP_Text keyLabel;

    private void Start()
    {
        if (actionLabel != null)
        {
            actionLabel.text = displayName;
        }

        if (keyLabel == null)
        {
            keyLabel = GetComponent<TMP_Text>();
        }
        rebinder.RefreshLabel(actionName, bindingIndex, keyLabel);
    }

    public void Rebind()
    {
        rebinder.StartRebind(
            actionName,
            bindingIndex,
            keyLabel
        );
    }
}