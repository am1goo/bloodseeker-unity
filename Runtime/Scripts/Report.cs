using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodseekerSDK
{
    public struct Report
    {
        public Result result;
        public string[] evidence;
        public string[] errors;

        public Report(Result result, string[] evidence, string[] errors)
        {
            this.result = result;
            this.evidence = evidence;
            this.errors = errors;
        }

        public static Report NotInitialized()
        {
            return NotInitialized((Exception)null);
        }

        public static Report NotInitialized(Exception exception)
        {
            return NotInitialized(new Exception[] { exception });
        }

        public static Report NotInitialized(IEnumerable<Exception> exceptions)
        {
            var errors = exceptions?.Select(x => x.ToString()).ToArray() ?? null;
            return new Report(Result.NotInitialized, null, errors);
        }

        public static Report UnexpectedError(Exception exception)
        {
            return UnexpectedError(new Exception[] { exception });
        }

        public static Report UnexpectedError(IEnumerable<Exception> exceptions)
        {
            var errors = exceptions.Select(x => x.ToString()).ToArray();
            return new Report(Result.UnexpectedError, null, errors);
        }

        public static Report Ok()
        {
            return new Report(Result.Ok, null, null);
        }

        public enum Result
        {
            NotInitialized = 0,
            Found = 1,
            Ok = 2,
            UnexpectedError = 3,
        }
    }
}
