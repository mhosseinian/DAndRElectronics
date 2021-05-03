using System;
using System.Collections.Generic;
using System.IO;
using Common.Services;
using Newtonsoft.Json;
using PatternBuilderLib.ViewModels;

namespace PatternBuilderLib
{
    public static class SerializerManager
    {
        
        public static string SerializeToJson(this CyclesManageViewModel cyclesVm)
        {
            return JsonConvert.SerializeObject(cyclesVm.Cycles, Formatting.Indented);
        }

        public static IEnumerable<DeviceManagerViewModel> Deserialize(string content)
        {
            var logger = ServiceDirectory.Instance.GetService<ILogService>();
            logger.Info("Deserialing");
            try
            {
                var items = JsonConvert.DeserializeObject<IEnumerable<DeviceManagerViewModel>>(content);
                return items as IEnumerable<DeviceManagerViewModel>;
            }
            catch (Exception e)
            {
                logger.Error("Deserialize failed", e);
                throw e;
            }

            
        }

        public static void SerializeBinaryToFile(this CyclesManageViewModel vm, string filename)
        {
          
            using var binWriter = new BinaryWriter(File.Open(filename, FileMode.Create));
            binWriter.Write((byte)vm.Cycles.Count);
            binWriter.Write((byte)vm.NumDevices);
            foreach (var deviceManager in vm.Cycles)
            {
                binWriter.Write((byte)deviceManager.Delay);
                foreach (var deviceVm in deviceManager.Devices)
                {
                    deviceVm.SerializeBinary(binWriter);
                }
            }
        }
    }
}