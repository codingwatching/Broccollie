using System.Threading;
using UnityEngine;

public interface IScreenFader
{
    Awaitable FadeAsync(float alpha, CancellationToken cancellationToken = default);
}