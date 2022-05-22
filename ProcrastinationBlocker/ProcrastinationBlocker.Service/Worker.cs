using ProcrastinationBlocker.Forest.API;
using ProcrastinationBlocker.HostsHelper;

namespace ProcrastinationBlocker.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Configuration Configuration { get; }
        private ForestRestApiHelper ForestRestApiHelper{ get; }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            Configuration = Configuration.LoadConfiguration();
            ForestRestApiHelper = new ForestRestApiHelper(Configuration.AuthenticationCookie);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var totalFocusedTime = ForestRestApiHelper.GetFocusedTimeSince(DateTime.Now - TimeSpan.FromDays(1));
                    
                    if (totalFocusedTime.TotalHours > Configuration.HoursRequired )
                    {
                        LiftRestrictions();
                    }
                    else
                    {
                        RestrictAccess();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception occurred during the routine.");
                }
                await Task.Delay(Configuration.WaitTimeMinutes * 60 * 1000, stoppingToken);
            }
        }

        private void RestrictAccess()
        {
            using var hosts = new HostsFileHelper();
            foreach (var host in Configuration.BlockedHosts)
            {
                hosts.Add(Configuration.RedirectTo, host);
            }
        }

        private void LiftRestrictions()
        {
            using var hosts = new HostsFileHelper();
            foreach (var host in Configuration.BlockedHosts)
            {
                hosts.Remove(host);
            }
        }
    }
}