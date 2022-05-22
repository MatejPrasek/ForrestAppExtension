namespace ProcrastinationBlocker.HostsHelper;

internal class CommentOnlyHostsFileEntry : HostsFileEntry
{
    public CommentOnlyHostsFileEntry(string comment)
    {
        Comment = comment;
    }
    public override string ToString()
    {
        return $"#{Comment}";
    }
}