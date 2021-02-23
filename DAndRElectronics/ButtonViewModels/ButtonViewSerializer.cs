using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public static class ButtonViewSerializer
    {
        public static string Serialize(this ButtonViewModel vm)
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            

            foreach (var vmIgnoreProperty in vm.IgnoreProperties)
            {
                jsonResolver.IgnoreProperty(typeof(ButtonViewModel), vmIgnoreProperty);
            }

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;
            serializerSettings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(vm, serializerSettings);

            return json;
        }
    }
}