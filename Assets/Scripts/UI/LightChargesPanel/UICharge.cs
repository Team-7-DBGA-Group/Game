using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICharge : MonoBehaviour
{
    public bool IsOn { get; private set; }

    [Header("References")]
    [SerializeField]
    private Image iconImage = null;

    [Header("UI Settings")]
    [SerializeField]
    private Color32 disabledColor = Color.gray;
    [SerializeField]
    private Color32 blinkColor = Color.yellow;
    [SerializeField]
    private Color32 defaultColor = Color.white;
    [SerializeField]
    private float blinkSpeed = 0.2f;
    [SerializeField]
    private int blinkNumber = 2;

    public void SetEnable(bool enable) => iconImage.enabled = enable;

    public void Off()
    {
        if (!IsOn)
            return;

        StopAllCoroutines();
        iconImage.color = disabledColor;
        IsOn = false;
    }

    public void On()
    {
        if (IsOn)
            return;

        IsOn = true;
        StartCoroutine(COBlinkAnimation());
    }

    private void Start()
    {
        iconImage.color = defaultColor;
        IsOn = true;
    }

    private IEnumerator COBlinkAnimation()
    {
        iconImage.color = defaultColor;

        for(int i = 0; i < blinkNumber; ++i)
        {
            iconImage.color = defaultColor;
            yield return new WaitForSeconds(blinkSpeed);
            iconImage.color = blinkColor;
            yield return new WaitForSeconds(blinkSpeed);
        }

        iconImage.color = defaultColor;
    }
}
