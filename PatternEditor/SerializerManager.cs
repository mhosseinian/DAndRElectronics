using System.Collections.Generic;
using System.IO;
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

        public static void SerializeBinaryToFile(this CyclesManageViewModel vm, string filename)
        {
            using var binWriter = new BinaryWriter(File.Open(filename, FileMode.Create));
            binWriter.Write(vm.Cycles.Count);
            foreach (var deviceManager in vm.Cycles)
            {
                binWriter.Write(deviceManager.Delay);
                foreach (var deviceVm in deviceManager.Devices)
                {
                    deviceVm.SerializeBinary(binWriter);
                }
            }
        }
    }
}