﻿namespace FitnessTracker.Common.Filters
{
    public partial class HttpGlobalExceptionFilter
    {
        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }

            public object DeveloperMessage { get; set; }
        }
    }
}