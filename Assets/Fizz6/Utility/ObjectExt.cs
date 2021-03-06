using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Fizz6.Utility
{
    public static class ObjectExt
    {
        public static Task WaitUntilUpdate(this Object context, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilUpdate(cancellationToken, context);
        }
        
        public static Task WaitUntilUpdate(this Object context, int frames, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilUpdate(frames, cancellationToken, context);
        }
        
        public static Task WaitUntilUpdate(this Object context, float delay, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilUpdate(delay, cancellationToken, context);
        }

        public static Task WaitUntilLateUpdate(this Object context, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilLateUpdate(cancellationToken, context);
        }
        
        public static Task WaitUntilLateUpdate(this Object context, int frames, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilLateUpdate(frames, cancellationToken, context);
        }
        
        public static Task WaitUntilLateUpdate(this Object context, float delay, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilLateUpdate(delay, cancellationToken, context);
        }

        public static Task WaitUntilEndOfFrame(this Object context, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilEndOfFrame(cancellationToken, context);
        }
        
        public static Task WaitUntilEndOfFrame(this Object context, int frames, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilEndOfFrame(frames, cancellationToken, context);
        }
        
        public static Task WaitUntilEndOfFrame(this Object context, float delay, CancellationToken cancellationToken)
        {
            return Scheduler.Scheduler.Instance.WaitUntilEndOfFrame(delay, cancellationToken, context);
        }
    }
}