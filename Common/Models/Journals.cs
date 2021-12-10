using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace Common.Models;

public class Journal : IAuditColumns
{
    [PgName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [PgName("originalid")]
    public string OriginalID { get; set; }

    [PgName("title")]
    public string Title { get; set; }

    [PgName("about")]
    public string About { get; set; }

    [PgName("aimsandscope")]
    public string AimsAndScope { get; set; }

    [PgName("url")]
    public string Url { get; set; }

    [PgName("imgurl")]
    public string ImgUrl { get; set; }

    [PgName("editorial")]
    public string Editorial { get; set; }

    [PgName("version")]
    public int Version { get; set; }

    [PgName("createdat")]
    public DateTime CreatedAt { get; set; }

    [PgName("modifiedat")]
    public DateTime ModifiedAt { get; set; }

    [PgName("modifiedby")]
    public Guid ModifiedBy { get; set; }

    [PgName("createdby")]
    public Guid CreatedBy { get; set; }

    [PgName("impactfactor")]

    public double? ImpactFactor { get; set; } = 0;

    [PgName("keywords")]
    public string Keywords { get; set; }
    public List<JournalMetric> Metrics { get; set; } = new();

    public string Errors { get; set; }

    public override int GetHashCode()
    {
        return this.OriginalID.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        var other = obj as Journal;
        return this.OriginalID.Equals(other.OriginalID);
    }
}

public class JournalMetric 
{
    [PgName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [PgName("journalid")]
    public Guid JournalId { get; set; }
    [PgName("name")]
    public string Name { get; set; }
    [PgName("value")]
    public double Value { get; set; }
}