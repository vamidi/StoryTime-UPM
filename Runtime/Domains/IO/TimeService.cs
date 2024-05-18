using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoryTime.Domains.IO
{
    using Resource;
    
    public class TimeService
    {
        public async Task WriteUptime(string destination, string groupName = "")
        {
            var timestamp = DateTime.Now.Ticks;

            await File.WriteAllTextAsync(destination, timestamp.ToString());

            if (!string.IsNullOrEmpty(groupName))
            {
                // Add the uptime file to the addressable group
                ResourceHelper.AddFileToAddressable(groupName, destination);
            }
        }
        
        public async Task<Int64> GetUptime(string destination)
        {
            Int64 timestamp = 0;
            if (File.Exists(destination))
            {
                var timestampRes = await File.ReadAllTextAsync(destination, Encoding.UTF8);
                timestamp = Int64.Parse(timestampRes);
            }

            return timestamp;
        }
    }
}