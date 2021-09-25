namespace Tracer
{
    namespace Format
    {
        public static class SerializationConfig
        {
            #region JSON

            public const string JSON_METHOD_NAME = "method";

            public const string JSON_METHOD_CLASS = "class";
            public const string JSON_METHOD_TIME = "time";
            public const string JSON_METHOD_INNER_METHODS = "methods";

            public const string JSON_THREAD_NAME = "name";
            public const string JSON_THREAD_TIME = "time";
            public const string JSON_THREAD_METHODS = "methods";

            #endregion

            #region XML

            public const string XML_ROOT = "root";

            public const string XML_METHOD = "method";
            public const string XML_METHOD_CLASS = "class";
            public const string XML_METHOD_NAME = "name";
            public const string XML_METHOD_TIME = "time";

            public const string XML_THREAD = "thread";
            public const string XML_THREAD_NAME = "name";
            public const string XML_THREAD_TIME = "time";

            #endregion
        }
    }
}
