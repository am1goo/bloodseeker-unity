using System.Threading.Tasks;
using UnityEngine;

namespace BloodseekerSDK
{
    public class Bloodseeker
    {
        private IBloodseekerPlatform _platform;

        private Bloodseeker()
        {
#if UNITY_EDITOR
            _platform = new EditorPlatform();
#elif UNITY_ANDROID
            _platform = new AndroidPlatform();
#else
            _platform = new StubPlatform();
#endif
        }

        public static Bloodseeker Create()
        {
            return new Bloodseeker();
        }

        public Bloodseeker AddTrail(ITrail trail)
        {
            if (trail == null)
                return this;

            _platform.AddTrail(trail);
            return this;
        }

        public Task<Report> Seek()
        {
           return _platform.Seek();
        }

        public ReportAsyncOperation SeekAsync()
        {
            var task = Seek();
            return new ReportAsyncOperation(task);
        }

        public class ReportAsyncOperation : CustomYieldInstruction
        {
            public override bool keepWaiting => !_task.IsCompleted;

            public Report report => _task.Result;
            private Task<Report> _task;

            public ReportAsyncOperation(Task<Report> task)
            {
                this._task = task;
            }
        }
    }
}
