using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CodeLockDigit : MonoBehaviour
{
    [SerializeField] private CodeLock codeLockReference = null;
    [SerializeField] private int index = -1;
    [Tooltip("What sprites to use (in order 0-9)")]
    [SerializeField] private List<Sprite> sprites;

    public int currentDigit = 0;
    private Image imageComponent = null;

    private void OnValidate()
    {
        if(codeLockReference == null)
        {
            Debug.LogWarning("Code lock not attached to this code lock number.", this);
        }

        if(imageComponent == null)
        {
            imageComponent = GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        codeLockReference.onUpdate.AddListener(UpdateGraphic);
    }

    private void OnDisable()
    {
        codeLockReference.onUpdate.RemoveListener(UpdateGraphic);
    }

    private void UpdateGraphic()
    {
        imageComponent.sprite = sprites[currentDigit];
    }

    public void Increment()
    {
        currentDigit = (currentDigit + 1) % 10;
        codeLockReference.onUpdate.Invoke();
    }

    public void Decrease()
    {
        currentDigit = (currentDigit - 1);
        if (currentDigit < 0)
        {
            currentDigit = 9;
        }
        codeLockReference.onUpdate.Invoke();
    }
}
