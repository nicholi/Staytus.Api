using System;

namespace Staytus.Api
{
    public static class SystemMessages
    {
        public const String SUCCESS = "success";
        public const String ERROR = "error";


        // not returned by staytus, our defined status
        public const String PARSE_ERROR = "parsing-error";
    }
}
