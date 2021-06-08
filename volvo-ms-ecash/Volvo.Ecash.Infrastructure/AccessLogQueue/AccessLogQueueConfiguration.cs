
namespace Volvo.Ecash.Infrastructure.AccessLogQueue
{
    public class LogQueueConfiguration
    {
        public string MonitorApi { get; set; }
        public string UrlAccessLogMonitorSendPost { get; set; }                
        public string UrlAuditLogMonitorSendPost { get; set; }        
        public string UrlEngagementLogMonitorSendPost { get; set; }        
    }
}
