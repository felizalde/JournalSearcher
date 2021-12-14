namespace SearchEngine.Indices;

public static class IndexMapping
{
    public static string GetIndexName(Type documentType)
    {
        if (documentType == typeof(JournalDocumentBM25))
        {
            return "journals-bm25";
        }
        else if (documentType == typeof(JournalDocumentClassic))
        {
            return "journals-classic";
        }
        else
        {
            return "journals";
        }
    }
}