using UnityEngine;
using UnityEngine.UI;

public class StateColorOutlineFeedback : MonoBehaviour
{
    [SerializeField]
    private Outline _outline;
    private Image _img;
    private float _previousFillAmount;
    private float _stableTimer;
    private const float STABILITY_TIME = 0.5f; 

    private void Awake()
    {
        TryGetComponent(out _img);
        _previousFillAmount = _img.fillAmount;
    }

    private void Update()
    {
        float delta = _img.fillAmount - _previousFillAmount;

        if (delta > 0.000001f)
        {
            _outline.effectColor = Color.greenYellow;
            _stableTimer = STABILITY_TIME;
        }
        else
        {
            _stableTimer -= Time.deltaTime;

            if (_stableTimer <= 0f)
            {
                _outline.effectColor = Color.clear;
            }
        }

        _previousFillAmount = _img.fillAmount;
    }
}
