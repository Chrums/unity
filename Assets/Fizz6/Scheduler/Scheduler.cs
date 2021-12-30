using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fizz6.Scheduler
{
    public class Scheduler : SingletonMonoBehaviour<Scheduler>
    {
        private enum Queue
        {
            Update,
            LateUpdate,
            EndOfFrame
        }
        
        #region Tasks
        
        private class SchedulerTask
        {
            private readonly bool _contextual;
            private readonly Object _context;
            private readonly TaskCompletionSource<object> _taskCompletionSource;
            public Task Task => _taskCompletionSource.Task;

            public SchedulerTask(CancellationToken cancellationToken, Object context = null)
            {
                _context = context;
                _contextual = _context != null;
                _taskCompletionSource = new TaskCompletionSource<object>(null);
                cancellationToken.Register(_taskCompletionSource.SetCanceled);
            }

            public void Invoke()
            {
                if (_taskCompletionSource.Task.IsCompleted) return;
                
                if (_contextual && _context == null)
                {
                    _taskCompletionSource.SetCanceled();
                    return;
                }

                Execute();
            }

            protected virtual void Execute()
            {
                _taskCompletionSource.SetResult(null);
            }
        }
        
        private class FrameSchedulerTask : SchedulerTask
        {
            private readonly int _frame;
            private readonly int _frames;

            public FrameSchedulerTask(int frames, CancellationToken cancellationToken, Object context = null) : 
                base(cancellationToken, context)
            {
                _frame = Time.frameCount;
                _frames = frames;
            }

            protected override void Execute()
            {
                if (Time.frameCount - _frame < _frames)
                {
                    return;
                }
                
                base.Execute();
            }
        }
        
        private class TimeSchedulerTask : SchedulerTask
        {
            private readonly float _time;
            private readonly float _delay;

            public TimeSchedulerTask(float delay, CancellationToken cancellationToken, Object context = null) : 
                base(cancellationToken, context)
            {
                _time = Time.time;
                _delay = delay;
            }

            protected override void Execute()
            {
                if (Time.time - _time < _delay)
                {
                    return;
                }
                
                base.Execute();
            }
        }
        
        private static readonly List<SchedulerTask> ImmediateSchedulerTasks = new List<SchedulerTask>();
        private static readonly List<SchedulerTask> ImmediateDelaySchedulerTasks = new List<SchedulerTask>();
        
        private readonly Dictionary<Enum, List<SchedulerTask>> _schedulerTasks = new Dictionary<Enum, List<SchedulerTask>>();

        public Task WaitUntilUpdate(CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(Queue.Update, cancellationToken, context);
        }

        public Task WaitUntilUpdate(int frames, CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(frames, Queue.Update, cancellationToken, context);
        }

        public Task WaitUntilUpdate(float delay, CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(delay, Queue.Update, cancellationToken, context);
        }

        public Task WaitUntilLateUpdate(CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(Queue.LateUpdate, cancellationToken, context);
        }

        public Task WaitUntilLateUpdate(int frames, CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(frames, Queue.LateUpdate, cancellationToken, context);
        }

        public Task WaitUntilLateUpdate(float delay, CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(delay, Queue.LateUpdate, cancellationToken, context);
        }

        public Task WaitUntilEndOfFrame(CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(Queue.EndOfFrame, cancellationToken, context);
        }

        public Task WaitUntilEndOfFrame(int frames, CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(frames, Queue.EndOfFrame, cancellationToken, context);
        }

        public Task WaitUntilEndOfFrame(float delay, CancellationToken cancellationToken, Object context = null)
        {
            return WaitUntil(delay, Queue.EndOfFrame, cancellationToken, context);
        }

        public Task WaitUntil(Enum queue, CancellationToken cancellationToken, Object context = null)
        {
            var waitItem = new SchedulerTask(cancellationToken, context);

            var waitItems = FindSchedulerTasksByQueue(queue);
            waitItems.Add(waitItem);
            
            return waitItem.Task;
        }
        
        public Task WaitUntil(int frames, Enum queue, CancellationToken cancellationToken, Object context = null)
        {
            var waitItem = new FrameSchedulerTask(frames, cancellationToken, context);

            var waitItems = FindSchedulerTasksByQueue(queue);
            waitItems.Add(waitItem);
            
            return waitItem.Task;
        }
        
        public Task WaitUntil(float delay, Enum queue, CancellationToken cancellationToken, Object context = null)
        {
            var waitItem = new TimeSchedulerTask(delay, cancellationToken, context);

            var waitItems = FindSchedulerTasksByQueue(queue);
            waitItems.Add(waitItem);
            
            return waitItem.Task;
        }

        public void InvokeSchedulerTasks(Enum queue)
        {
            var waitItems = FindSchedulerTasksByQueue(queue);
            while (waitItems.Count > 0)
            {
                ImmediateSchedulerTasks.AddRange(waitItems);
                waitItems.Clear();
                
                foreach (var waitItem in ImmediateSchedulerTasks)
                {
                    waitItem.Invoke();
                    if (!waitItem.Task.IsCompleted)
                    {
                        ImmediateDelaySchedulerTasks.Add(waitItem);
                    }
                }
                
                ImmediateSchedulerTasks.Clear();
            }
            
            waitItems.AddRange(ImmediateDelaySchedulerTasks);
            ImmediateDelaySchedulerTasks.Clear();
        }

        private List<SchedulerTask> FindSchedulerTasksByQueue(Enum queue)
        {
            if (!_schedulerTasks.ContainsKey(queue))
            {
                _schedulerTasks.Add(queue, new List<SchedulerTask>());
            }
        
            return _schedulerTasks[queue];
        }
        
        #endregion
        
        private void Update()
        {
            InvokeSchedulerTasks(Queue.Update);
            StartCoroutine(EndOfFrame());
        }
        
        private void LateUpdate()
        {
            InvokeSchedulerTasks(Queue.LateUpdate);
        }

        private IEnumerator EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            InvokeSchedulerTasks(Queue.EndOfFrame);
        }
    }
}