using System;

namespace Staytus.Api.Models
{
    // these seem to be hardcoded, not user definable aspects of staytus
    public enum StaytusState
    {
        Unknown = 0,

        Investigating,
        Identified,
        Monitoring,
        Resolved
    }
}
