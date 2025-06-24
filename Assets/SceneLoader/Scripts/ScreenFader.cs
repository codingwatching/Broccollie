using System.Threading;
using UnityEngine;

public class ScreenFader : Singleton<ScreenFader>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration = 1f;

    public async Awaitable FadeInAsync(CancellationToken cancellationToken = default)
    {
        _canvasGroup.interactable = true;
        await FadeAsync(1f, cancellationToken);
    }

    public async Awaitable FadeOutAsync(CancellationToken cancellationToken = default)
    {
        await FadeAsync(0f, cancellationToken);
        _canvasGroup.interactable = false;
    }

    private async Awaitable FadeAsync(float alpha, CancellationToken cancellationToken = default)
    {
        float elapsedTime = 0f;
        float startAlpha = _canvasGroup.alpha;

        while (elapsedTime < _fadeDuration)
        {
            cancellationToken.ThrowIfCancellationRequested();

            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, alpha, Mathf.Clamp01(elapsedTime / _fadeDuration));

            await Awaitable.NextFrameAsync(cancellationToken);
        }
        _canvasGroup.alpha = alpha;
    }
}
