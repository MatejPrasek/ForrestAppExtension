namespace ProcrastinationBlocker.HostsHelper
{
    public interface IHostsFileEntry
    {
        public string? Address { get; }
        public IList<string>? HostNames { get; }
        public string? Comment { get; }
    }
}
