using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            serializerSettings.Converters.Add(new SubButtonsConverter());
            serializerSettings.ContractResolver = jsonResolver;
            serializerSettings.Formatting = Formatting.Indented;

            var json = JsonConvert.SerializeObject(vm, serializerSettings);

            return json;
        }
    }

    public class SubButtonsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
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