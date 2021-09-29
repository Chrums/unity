using System;
using System.Threading;
using UnityEngine;

namespace Fizz6
{
    public class SchedulerTest : MonoBehaviour
    {
        private enum TestQueue
        {
            FixedUpdate
        }
        
        private void Awake()
        {
            FixedUpdateTest();
            UpdateTest();
            LateUpdateTest();
            EndOfFrameTest();
            CancelTest();
            ContextTest();
            FrameTest();
            TimeTest();
        }

        private void FixedUpdate()
        {
            Scheduler.Instance.InvokeSchedulerTasks(TestQueue.FixedUpdate);
        }

        private static async void FixedUpdateTest()
        {
            await Scheduler.Instance.WaitUntil(TestQueue.FixedUpdate, CancellationToken.None);
            Debug.LogError("FixedUpdate");
        }

        private static async void UpdateTest()
        {
            await Scheduler.Instance.WaitUntilUpdate(CancellationToken.None);
            Debug.LogError("Update");
        }

        private static async void LateUpdateTest()
        {
            await Scheduler.Instance.WaitUntilLateUpdate(CancellationToken.None);
            Debug.LogError("LateUpdate");
        }

        private static async void EndOfFrameTest()
        {
            await Scheduler.Instance.WaitUntilEndOfFrame(CancellationToken.None);
            Debug.LogError("EndOfFrame");
        }

        private static async void CancelTest()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var task = Scheduler.Instance.WaitUntilEndOfFrame(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            try
            {
                await task;
            }
            catch (Exception _)
            {
                Debug.LogError("Cancel");
            }
        }

        private static async void ContextTest()
        {
            var gameObject = new GameObject();
            var task = gameObject.WaitUntilUpdate(CancellationToken.None);
            DestroyImmediate(gameObject);
            
            try
            {
                await task;
            }
            catch (Exception _)
            {
                Debug.LogError("Cancel");
            }
        }

        private static async void FrameTest()
        {
            Debug.LogError($"FrameTest: Called @ {Time.frameCount}");
            await Scheduler.Instance.WaitUntilUpdate(3, CancellationToken.None);
            Debug.LogError($"FrameTest: Invoked @ {Time.frameCount}");
        }

        private static async void TimeTest()
        {
            Debug.LogError($"TimeTest: Called @ {Time.time}");
            await Scheduler.Instance.WaitUntilUpdate(5.0f, CancellationToken.None);
            Debug.LogError($"TimeTest: Invoked @ {Time.time}");
        }
    }
}