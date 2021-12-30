using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;

namespace Fizz6
{
    public static class Assets
    {
        private class AssetsModifiedProcessor : UnityEditor.Experimental.AssetsModifiedProcessor
        {
            public static event Action<string> ChangedEvent;
            public static event Action<string> AddedEvent;
            public static event Action<string> DeletedEvent;
            public static event Action<string, string> MovedEvent;
            
            protected override void OnAssetsModified(
                string[] changedAssets, 
                string[] addedAssets, 
                string[] deletedAssets,
                AssetMoveInfo[] movedAssets
            )
            {
                foreach (var changedAsset in changedAssets) ChangedEvent?.Invoke(changedAsset);
                foreach (var addedAsset in addedAssets) AddedEvent?.Invoke(addedAsset);
                foreach (var deletedAsset in deletedAssets) DeletedEvent?.Invoke(deletedAsset);
                foreach (var movedAsset in movedAssets) MovedEvent?.Invoke(movedAsset.sourceAssetPath, movedAsset.destinationAssetPath);
            }
        }

        private class AssetPostprocessor : UnityEditor.AssetPostprocessor
        {
            public static event Action<string> ImportedEvent;
            public static event Action<string> DeletedEvent;
            public static event Action<string> MovedEvent;
            public static event Action<string> MovedFromEvent;
            
            private static void OnPostprocessAllAssets(
                string[] importedAssets,
                string[] deletedAssets,
                string[] movedAssets,
                string[] movedFromAssetPaths
            )
            {
                foreach (var importedAsset in importedAssets) ImportedEvent?.Invoke(importedAsset);
                foreach (var deletedAsset in deletedAssets) DeletedEvent?.Invoke(deletedAsset);
                foreach (var movedAsset in movedAssets) MovedEvent?.Invoke(movedAsset);
                foreach (var movedFromAssetPath in movedFromAssetPaths) MovedFromEvent?.Invoke(movedFromAssetPath);
            }
        }

        public interface IMonitor : IDisposable
        {
            event Action<string> AssetsModifiedProcessorChangedEvent;
            event Action<string> AssetsModifiedProcessorAddedEvent;
            event Action<string> AssetsModifiedProcessorDeletedEvent;
            event Action<string, string> AssetsModifiedProcessorMovedEvent;
            event Action<string> AssetPostprocessorImportedEvent;
            event Action<string> AssetPostprocessorDeletedEvent;
            event Action<string> AssetPostprocessorMovedEvent;
            event Action<string> AssetPostprocessorMovedFromEvent;
        }

        public interface IMonitor<out T> : IMonitor where T : UnityEngine.Object
        {
            event Action<T> AssetsModifiedProcessorAssetChangedEvent;
            event Action<T> AssetsModifiedProcessorAssetAddedEvent;
            event Action<T> AssetsModifiedProcessorAssetDeletedEvent;
            event Action<T, T> AssetsModifiedProcessorAssetMovedEvent;
            event Action<T> AssetPostprocessorAssetImportedEvent;
            event Action<T> AssetPostprocessorAssetDeletedEvent;
            event Action<T> AssetPostprocessorAssetMovedEvent;
            event Action<T> AssetPostprocessorAssetMovedFromEvent;
        }

        private interface IMonitorImplementation
        {
            void InvokeAssetsModifiedProcessorChangedEvent(string path);
            void InvokeAssetsModifiedProcessorAddedEvent(string path);
            void InvokeAssetsModifiedProcessorDeletedEvent(string path);
            void InvokeAssetsModifiedProcessorMovedEvent(string fromPath, string toPath);
            void InvokeAssetPostprocessorImportedEvent(string path);
            void InvokeAssetPostprocessorDeletedEvent(string path);
            void InvokeAssetPostprocessorMovedEvent(string path);
            void InvokeAssetPostprocessorMovedFromEvent(string path);
        }

        private class Monitor : IMonitor, IMonitorImplementation
        {
            private Func<string, bool> _filter;
            
            private Action<string> _assetsModifiedProcessorChangedEvent;
            event Action<string> IMonitor.AssetsModifiedProcessorChangedEvent
            {
                add => _assetsModifiedProcessorChangedEvent += value;
                remove => _assetsModifiedProcessorChangedEvent -= value;
            }
            
            private Action<string> _assetsModifiedProcessorAddedEvent;
            event Action<string> IMonitor.AssetsModifiedProcessorAddedEvent
            {
                add => _assetsModifiedProcessorAddedEvent += value;
                remove => _assetsModifiedProcessorAddedEvent -= value;
            }
            
            private Action<string> _assetsModifiedProcessorDeletedEvent;
            event Action<string> IMonitor.AssetsModifiedProcessorDeletedEvent
            {
                add => _assetsModifiedProcessorDeletedEvent += value;
                remove => _assetsModifiedProcessorDeletedEvent -= value;
            }
            
            private Action<string, string> _assetsModifiedProcessorMovedEvent;
            event Action<string, string> IMonitor.AssetsModifiedProcessorMovedEvent
            {
                add => _assetsModifiedProcessorMovedEvent += value;
                remove => _assetsModifiedProcessorMovedEvent -= value;
            }
            
            private Action<string> _assetPostprocessorImportedEvent;
            event Action<string> IMonitor.AssetPostprocessorImportedEvent
            {
                add => _assetPostprocessorImportedEvent += value;
                remove => _assetPostprocessorImportedEvent -= value;
            }
            
            private Action<string> _assetPostprocessorDeletedEvent;
            event Action<string> IMonitor.AssetPostprocessorDeletedEvent
            {
                add => _assetPostprocessorDeletedEvent += value;
                remove => _assetPostprocessorDeletedEvent -= value;
            }
            
            private Action<string> _assetPostprocessorMovedEvent;
            event Action<string> IMonitor.AssetPostprocessorMovedEvent
            {
                add => _assetPostprocessorMovedEvent += value;
                remove => _assetPostprocessorMovedEvent -= value;
            }
            
            private Action<string> _assetPostprocessorMovedFromEvent;
            event Action<string> IMonitor.AssetPostprocessorMovedFromEvent
            {
                add => _assetPostprocessorMovedFromEvent += value;
                remove => _assetPostprocessorMovedFromEvent -= value;
            }

            public Monitor(Func<string, bool> filter = null)
            {
                _filter = filter;
            }

            public void InvokeAssetsModifiedProcessorChangedEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetsModifiedProcessorChangedEvent(path);
            }

            protected virtual void OnAssetsModifiedProcessorChangedEvent(string path)
            {
                _assetsModifiedProcessorChangedEvent?.Invoke(path);
            }

            public void InvokeAssetsModifiedProcessorAddedEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetsModifiedProcessorAddedEvent(path);
            }

            protected virtual void OnAssetsModifiedProcessorAddedEvent(string path)
            {
                _assetsModifiedProcessorAddedEvent?.Invoke(path);
            }

            public void InvokeAssetsModifiedProcessorDeletedEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetsModifiedProcessorDeletedEvent(path);
            }

            protected virtual void OnAssetsModifiedProcessorDeletedEvent(string path)
            {
                _assetsModifiedProcessorDeletedEvent?.Invoke(path);
            }

            public void InvokeAssetsModifiedProcessorMovedEvent(string fromPath, string toPath)
            {
                if ((!_filter?.Invoke(fromPath) ?? false) && (!_filter?.Invoke(toPath) ?? false)) return;
                OnAssetsModifiedProcessorMovedEvent(fromPath, toPath);
            }

            protected virtual void OnAssetsModifiedProcessorMovedEvent(string fromPath, string toPath)
            {
                _assetsModifiedProcessorMovedEvent?.Invoke(fromPath, toPath);
            }
            
            public void InvokeAssetPostprocessorImportedEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetPostprocessorImportedEvent(path);
            }

            protected virtual void OnAssetPostprocessorImportedEvent(string path)
            {
                _assetPostprocessorImportedEvent?.Invoke(path);
            }
            
            public void InvokeAssetPostprocessorDeletedEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetPostprocessorDeletedEvent(path);
            }

            protected virtual void OnAssetPostprocessorDeletedEvent(string path)
            {
                _assetPostprocessorDeletedEvent?.Invoke(path);
            }
            
            public void InvokeAssetPostprocessorMovedEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetPostprocessorMovedEvent(path);
            }

            protected virtual void OnAssetPostprocessorMovedEvent(string path)
            {
                _assetPostprocessorMovedEvent?.Invoke(path);
            }
            
            public void InvokeAssetPostprocessorMovedFromEvent(string path)
            {
                if (!_filter?.Invoke(path) ?? false) return;
                OnAssetPostprocessorMovedFromEvent(path);
            }

            protected virtual void OnAssetPostprocessorMovedFromEvent(string path)
            {
                _assetPostprocessorMovedFromEvent?.Invoke(path);
            }

            public void Dispose()
            {
                Monitors.Remove(this);
            }
        }

        private class Monitor<T> : Monitor, IMonitor<T> where T : UnityEngine.Object
        {
            public event Action<T> AssetsModifiedProcessorAssetChangedEvent;
            public event Action<T> AssetsModifiedProcessorAssetAddedEvent;
            public event Action<T> AssetsModifiedProcessorAssetDeletedEvent;
            public event Action<T, T> AssetsModifiedProcessorAssetMovedEvent;
            public event Action<T> AssetPostprocessorAssetImportedEvent;
            public event Action<T> AssetPostprocessorAssetDeletedEvent;
            public event Action<T> AssetPostprocessorAssetMovedEvent;
            public event Action<T> AssetPostprocessorAssetMovedFromEvent;

            public Monitor(Func<string, bool> filter = null) : base(filter) {}

            protected override void OnAssetsModifiedProcessorChangedEvent(string path)
            {
                base.OnAssetsModifiedProcessorChangedEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetsModifiedProcessorAssetChangedEvent?.Invoke(asset);
            }
            
            protected override void OnAssetsModifiedProcessorAddedEvent(string path)
            {
                base.OnAssetsModifiedProcessorAddedEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetsModifiedProcessorAssetAddedEvent?.Invoke(asset);
            }
            
            protected override void OnAssetsModifiedProcessorDeletedEvent(string path)
            {
                base.OnAssetsModifiedProcessorDeletedEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetsModifiedProcessorAssetDeletedEvent?.Invoke(asset);
            }
            
            protected override void OnAssetsModifiedProcessorMovedEvent(string fromPath, string toPath)
            {
                base.OnAssetsModifiedProcessorMovedEvent(fromPath, toPath);
                var fromAsset = AssetDatabase.LoadAssetAtPath<T>(fromPath);
                var toAsset = AssetDatabase.LoadAssetAtPath<T>(toPath);
                if (fromAsset == null && toAsset == null) return;
                AssetsModifiedProcessorAssetMovedEvent?.Invoke(fromAsset, toAsset);
            }
            
            protected override void OnAssetPostprocessorImportedEvent(string path)
            {
                base.OnAssetPostprocessorImportedEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetPostprocessorAssetImportedEvent?.Invoke(asset);
            }
            
            protected override void OnAssetPostprocessorDeletedEvent(string path)
            {
                base.OnAssetPostprocessorDeletedEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetPostprocessorAssetDeletedEvent?.Invoke(asset);
            }
            
            protected override void OnAssetPostprocessorMovedEvent(string path)
            {
                base.OnAssetPostprocessorMovedEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetPostprocessorAssetMovedEvent?.Invoke(asset);
            }
            
            protected override void OnAssetPostprocessorMovedFromEvent(string path)
            {
                base.OnAssetPostprocessorMovedFromEvent(path);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null) return;
                AssetPostprocessorAssetMovedFromEvent?.Invoke(asset);
            }
        }

        public static string AbsoluteToAssetsRelativePath(string path)
        {
            var assetIndex = path.IndexOf("Assets", StringComparison.Ordinal);
            return path.Substring(assetIndex);
        }
        
        public static Func<string, bool> FileFilter(string filePath) => 
            path => filePath.Trim('/') == path.Trim('/');

        public static Func<string, bool> DirectoryFilter(string directoryPath) =>
            path => path.Trim('/').StartsWith(directoryPath.Trim('/'));

        public static IMonitor Watch(Func<string, bool> filter = null)
        {
            var monitor = new Monitor(filter);
            Monitors.Add(monitor);
            return monitor;
        }

        public static IMonitor<T> Watch<T>(Func<string, bool> filter = null) where T : UnityEngine.Object
        {
            var monitor = new Monitor<T>(filter);
            Monitors.Add(monitor);
            return monitor;
        }
        
        private static readonly List<IMonitorImplementation> Monitors = new List<IMonitorImplementation>();

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            AssetsModifiedProcessor.ChangedEvent += OnAssetsModifiedProcessorChanged;
            AssetsModifiedProcessor.AddedEvent += OnAssetsModifiedProcessorAdded;
            AssetsModifiedProcessor.DeletedEvent += OnAssetsModifiedProcessorDeleted;
            AssetsModifiedProcessor.MovedEvent += OnAssetsModifiedProcessorMoved;
            AssetPostprocessor.ImportedEvent += OnAssetPostprocessorImported;
            AssetPostprocessor.DeletedEvent += OnAssetPostprocessorDeleted;
            AssetPostprocessor.MovedEvent += OnAssetPostprocessorMoved;
            AssetPostprocessor.MovedFromEvent += OnAssetPostprocessorMovedFrom;
        }

        private static void OnAssetsModifiedProcessorChanged(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetsModifiedProcessorChangedEvent(path);
        }

        private static void OnAssetsModifiedProcessorAdded(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetsModifiedProcessorAddedEvent(path);
        }

        private static void OnAssetsModifiedProcessorDeleted(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetsModifiedProcessorDeletedEvent(path);
        }

        private static void OnAssetsModifiedProcessorMoved(string fromPath, string toPath)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetsModifiedProcessorMovedEvent(fromPath, toPath);
        }

        private static void OnAssetPostprocessorImported(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetPostprocessorImportedEvent(path);
        }

        private static void OnAssetPostprocessorDeleted(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetPostprocessorDeletedEvent(path);
        }

        private static void OnAssetPostprocessorMoved(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetPostprocessorMovedEvent(path);
        }

        private static void OnAssetPostprocessorMovedFrom(string path)
        {
            foreach (var monitor in Monitors) monitor.InvokeAssetPostprocessorMovedFromEvent(path);
        }
    }
}