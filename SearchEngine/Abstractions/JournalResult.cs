using SearchEngine.Indices;
namespace SearchEngine.Interfaces;

public class JournalResult<T> where T : BaseIndex
{

    public JournalResult()
    {

    }

    public JournalResult(T document)
    {
        Document = document;
    }

    public T Document { get; set; }
    public double? Score { get; set; }
}
