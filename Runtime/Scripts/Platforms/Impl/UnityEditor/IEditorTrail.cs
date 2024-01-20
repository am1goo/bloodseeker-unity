#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodseekerSDK.Editor
{
    public interface IEditorTrail : ITrail
    {
        Task Seek(List<IResult> result, List<Exception> exceptions);
    }
}
#endif
