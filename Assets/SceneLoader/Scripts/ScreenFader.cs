using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour, IFaderAsync
{
    [SerializeField] private Image _bgImage;
    [SerializeField] private float _fadeDuration = 1f;

    public async Awaitable FadeAsync(float targetAlpha, CancellationToken cancellationToken = default)
    {
        float elapsedTime = 0f;
        float startAlpha = _bgImage.color.a;
        Color color = _bgImage.color;

        while (elapsedTime < _fadeDuration)
        {
            cancellationToken.ThrowIfCancellationRequested();

            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, Mathf.Clamp01(elapsedTime / _fadeDuration));
            _bgImage.color = color;

            await Awaitable.NextFrameAsync(cancellationToken);
        }
        color.a = targetAlpha;
        _bgImage.color = color;
    }
}
