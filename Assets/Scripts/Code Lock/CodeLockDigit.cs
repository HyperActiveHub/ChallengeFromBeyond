using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Image))]
public class CodeLockDigit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CodeLock codeLockReference = null;
    [SerializeField] private Image currentDigitImage;
    [SerializeField] private Image nextDigitImage;

    [Header("General Configuration")]
    [SerializeField] private int index = -1;
    [Tooltip("What sprites to use (in order 0-9)")]
    [SerializeField] private List<Sprite> sprites;

    [Header("Animation")]
    [SerializeField] private float totalAnimationLength;
    [SerializeField] private AnimationCurve animationCurve;

    //Animation positions, will lerp between these positions.
    private Vector3 initialPosCurrentDigit;
    private Vector3 posUnderDigit;
    private Vector3 posAboveDigit;

    //If no digit is not currently being animated.
    private bool animationComplete = true;
    [HideInInspector] public int currentDigit = 0;

    private void OnValidate()
    {
        if(codeLockReference == null)
        {
            Debug.LogWarning("Reference to the Code lock object is missing. Please attach one.", this);
        }
    }

    private void Start()
    {
        initialPosCurrentDigit = currentDigitImage.rectTransform.position;
        posUnderDigit = initialPosCurrentDigit + new Vector3(0, -200.0f);
        posAboveDigit = initialPosCurrentDigit + new Vector3(0, 200.0f);
    }

    private IEnumerator AnimIncreaseNumber(Vector3 nextDigitStart, Vector3 currentDigitEnd)
    {
        float timeSinceAnimationStart = 0.0f;

        //Update sprites
        animationComplete = false;
        nextDigitImage.gameObject.SetActive(true);
        nextDigitImage.sprite = sprites[currentDigit % 10];

        //Animate
        while(timeSinceAnimationStart < totalAnimationLength)
        {
            timeSinceAnimationStart += Time.deltaTime;
            float lerpPosition = animationCurve.Evaluate(timeSinceAnimationStart / totalAnimationLength);

            nextDigitImage.rectTransform.anchoredPosition = Vector3.Lerp(nextDigitStart, initialPosCurrentDigit, lerpPosition);
            nextDigitImage.rectTransform.anchoredPosition = new Vector2(0.0f, nextDigitImage.rectTransform.anchoredPosition.y);

            currentDigitImage.rectTransform.anchoredPosition = Vector3.Lerp(initialPosCurrentDigit, currentDigitEnd, lerpPosition);
            nextDigitImage.rectTransform.anchoredPosition = new Vector2(0.0f, nextDigitImage.rectTransform.anchoredPosition.y);

            yield return null;
        }
        //Clean up.
        currentDigitImage.rectTransform.position = initialPosCurrentDigit;
        nextDigitImage.rectTransform.position = posUnderDigit;

        currentDigitImage.sprite = sprites[currentDigit];

        nextDigitImage.gameObject.SetActive(false);
        currentDigitImage.gameObject.SetActive(true);
        animationComplete = true;
    }

    public void Increment()
    {
        if (!animationComplete)
        {
            return;
        }
        
        currentDigit = (currentDigit + 1) % 10;
        StartCoroutine(AnimIncreaseNumber(posUnderDigit, posAboveDigit));
        codeLockReference.onUpdate.Invoke();
    }

    public void Decrease()
    {
        if (!animationComplete)
        {
            return;
        }

        currentDigit = (currentDigit - 1);
        if (currentDigit < 0)
        {
            currentDigit = 9;
        }
        StartCoroutine(AnimIncreaseNumber(posAboveDigit, posUnderDigit));
        codeLockReference.onUpdate.Invoke();
    }
}
