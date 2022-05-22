namespace ProcrastinationBlocker.HostsHelper;

public abstract class HostsFileEntry : IHostsFileEntry
{
    public string? Address { get; protected set; }
    public IList<string>? HostNames { get; protected set; }
    public string? Comment { get; protected set; }
        
    public static IHostsFileEntry Parse(string entry)
    {
        var trimmedEntry = entry.Trim();
        if (string.IsNullOrEmpty(trimmedEntry) || trimmedEntry.StartsWith('#'))
        {
            return new CommentOnlyHostsFileEntry(trimmedEntry.TrimStart('#'));
        }

        var commentStartIndex = entry.IndexOf('#');

        var data = commentStartIndex >= 0 ? entry.Substring(0, commentStartIndex) : entry;

        var records = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (records.Length < 2)
        {
            throw new ArgumentException("Invalid hosts file entry passed");
        }
        var comment = commentStartIndex >= 0 ? entry.Substring(commentStartIndex + 1) : null;
        return new DataHostsFileEntry(records[0], records.Skip(1).ToList(), comment);

    }
}