﻿using Nest;

namespace SearchEngine.Abstractions;

public static class NestExtensions
{
    public static QueryContainer BuildMultiMatchQuery<T>(string queryValue) where T : class
    {
        var fields = typeof(T).GetProperties().Select(p => p.Name.ToLower()).ToArray();

        return new QueryContainerDescriptor<T>()
            .MultiMatch(c => c
                .Type(TextQueryType.Phrase)
                .Fields(f => f.Fields(fields)).Lenient().Query(queryValue));
    }

    public static double ObterBucketAggregationDouble(AggregateDictionary agg, string bucket)
    {
        return agg.BucketScript(bucket).Value ?? 0;
    }
}