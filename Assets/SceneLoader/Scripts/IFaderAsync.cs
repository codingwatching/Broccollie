using System.Threading;
using UnityEngine;

public interface IFaderAsync
{
    Awaitable FadeAsync(float targetAlpha, CancellationToken cancellationToken = default);
}
