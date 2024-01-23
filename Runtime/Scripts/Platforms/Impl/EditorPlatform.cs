#if UNITY_EDITOR
using BloodseekerSDK.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodseekerSDK
{
    public class EditorPlatform : IBloodseekerPlatform
    {
        private List<IEditorTrail> _trails;

        public void SetUpdateUrl(Uri uri)
        {
            //do nothing
        }

        public bool AddTrail(ITrail trail)
        {
            if (!(trail is IEditorTrail androidTrail))
                return false;

            if (_trails == null)
                _trails = new List<IEditorTrail>();

            if (_trails.Contains(androidTrail))
                return false;

            _trails.Add(androidTrail);
            return true;
        }

        public async Task<Report> Seek()
        {
            if (_trails == null)
                return Report.Ok();

            var results = new List<IResult>();
            var exceptions = new List<Exception>();

            foreach (var trail in _trails)
            {
                try
                {
                    await trail.Seek(results, exceptions);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            var result = results.Count > 0 ? Report.Result.Found : Report.Result.Ok;
            var evidence = results.Select(x => x.ToString()).ToArray();
            var errors = exceptions.Select(x => x.ToString()).ToArray();
            return new Report(result, evidence, errors);
        }
    }
}
#endif
