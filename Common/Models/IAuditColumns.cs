using System;

namespace Common.Models;

internal interface IAuditColumns
{
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
    Guid ModifiedBy { get; set; }
    Guid CreatedBy { get; set; }

    int Version { get; set; }
}