using System.Text;

namespace ProcrastinationBlocker.HostsHelper;

internal class DataHostsFileEntry : HostsFileEntry
{
    public DataHostsFileEntry(string address, IList<string> hostNames)
    {
        Address = address;
        HostNames = hostNames;
    }

    public DataHostsFileEntry(string address, IList<string> hostNames, string? comment) : this(address, hostNames)
    {
        Comment = comment;
    }
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(Address);
        stringBuilder.Append(' ');
        stringBuilder.AppendJoin(' ', HostNames!);
        if (Comment != null)
        {
            stringBuilder.Append(" #");
            stringBuilder.Append(Comment);
        }

        return stringBuilder.ToString();
    }
}