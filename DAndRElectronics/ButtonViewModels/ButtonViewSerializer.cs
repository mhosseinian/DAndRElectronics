#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.Helpers;
using Common.Services;
using DAndRElectronics.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DAndRElectronics.ButtonViewModels
{
    public static class ButtonViewSerializer
    {
        public static string Serialize(this ButtonViewModel vm)
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            vm.AssignColorsForSerialization();

            foreach (var vmIgnoreProperty in vm.IgnoreProperties)
            {
                jsonResolver.IgnoreProperty(typeof(ButtonViewModel), vmIgnoreProperty);
            }

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new SubButtonsConverter());
            serializerSettings.ContractResolver = jsonResolver;
            serializerSettings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(vm, serializerSettings);

            return json;
        }

        public static ButtonViewModel Deserialize(string content)
        {
            var service = ServiceDirectory.Instance.GetService<IButtonViewModelFactoryService>();
            var vm = service.CreateViewModelFromString(content);
            vm.InitColors();
            //Handle the subButtons
            vm.SubButtons.Clear();
            vm.SubButtons.Add(vm);
            var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            if (!map.ContainsKey(Constants.JsonSequence))
            {
                return vm;
            }

            if (!(map[Constants.JsonSequence] is JArray seqsArray))
            {
                return vm;
            }

            foreach (var jTokenElement in seqsArray)
            {
                var stringToken = jTokenElement.ToString();
                var subVm = service.CreateViewModelFromString(stringToken);
                vm.SubButtons.Add(subVm);
            }
            return vm;
        }
    }

    public class SubButtonsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            //Only if it is IsSubky we need to serialize it
            var objs = ((IEnumerable<ButtonViewModel>) value).Where(c => c.IsSubKey).ToArray();
            serializer.Serialize(writer, objs);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.Name.Contains("ObservableCollection"))
            {
                return true;
            }
            var ret =  (objectType == typeof(ObservableCollection<>));
            if (ret)
            {

            }
            return ret;
        }
    }

    
}