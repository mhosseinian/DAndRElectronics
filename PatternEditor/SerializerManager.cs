using System.Collections.Generic;
using System.Configuration;
using Common.Helpers;
using Newtonsoft.Json;
using PatternEditor.ViewModels;

namespace PatternEditor
{
    public static class SerializerManager
    {
        public static string SerializeToJson(this CyclesManageViewModel cyclesVm)
        {
            return JsonConvert.SerializeObject(cyclesVm.Cycles, Formatting.Indented);
        }

        public static IEnumerable<DeviceManagerViewModel> Deserialize(string content)
        {
            
            var items = JsonConvert.DeserializeObject<IEnumerable<DeviceManagerViewModel>>(content);
            return items as IEnumerable<DeviceManagerViewModel>;
        }
    }
}