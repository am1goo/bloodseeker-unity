using System;

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
            return NotInitialized(null);
        }

        public static Report NotInitialized(Exception exception)
        {
            var errors = exception != null ? new string[] { exception.ToString() } : null;
            return new Report(Result.NotInitialized, null, errors);
        }

        public static Report UnexpectedError(Exception exception)
        {
            var errors = exception != null ? new string[] { exception.ToString() } : null;
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
