namespace ProcrastinationBlocker.HostsHelper
{
    public class HostsFileHelper : IDisposable
    {
        private IList<IHostsFileEntry> Entries { get; }
        private readonly string hostsFilePath = Path.Combine(Environment.SystemDirectory, "drivers", "etc", "hosts");
        public HostsFileHelper()
        {
            Entries = new List<IHostsFileEntry>();
            foreach (var line in File.ReadAllLines(hostsFilePath))
            {
                Entries.Add(HostsFileEntry.Parse(line));
            }
        }

        public void Add(string address, string hostname)
        {
            Remove(hostname);
            Entries.Add(new DataHostsFileEntry(address, new []{hostname}));
        }

        public void Remove(string hostname)
        {
            var toRemove = Entries.Where(entry => entry.HostNames != null && entry.HostNames.Contains(hostname)).ToList();
            foreach (var hostsFileEntry in toRemove)
            {
                if (hostsFileEntry.HostNames == null)
                {
                    continue;
                }

                if (hostsFileEntry.HostNames.Count() == 1)
                {
                    Entries.Remove(hostsFileEntry);
                    continue;
                }

                hostsFileEntry.HostNames.Remove(hostname);
            }
        }

        public void Dispose()
        {
            File.WriteAllLines(hostsFilePath, Entries.Select(entry => entry.ToString())!);
        }
    }
}
