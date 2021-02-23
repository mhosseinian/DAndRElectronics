using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAndRElectronics.Helpers;
using DAndRElectronics.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DAndRElectronics.ButtonViewModels
{
    public class ButtonViewModelFactoryService: IButtonViewModelFactoryService
    {
        
        private Dictionary<string, Func<string, int, int, ButtonViewModel>> _mappings = new Dictionary<string, Func<string, int, int, ButtonViewModel>>
        {
            {Constants.KeyBaseName,(label, col, row) => new ButtonViewModel(label, col, row)},
            {Constants.InputBaseName,(label, col, row) => new InputButtonViewModel(label, col, row)},
            {Constants.EventBaseName,(label, col, row) => new EventButtonViewModel(label, col, row)},
            {Constants.SlideBaseName,(label, col, row) => new SlideButtonViewModel(label, col, row)},
            {Constants.AnalogBaseName,(label, col, row) => new AnalogButtonViewModel(label, col, row)},
            {Constants.TimerBaseName,(label, col, row) => new TimerButtonViewModel(label, col, row)},
            {Constants.TemperatureBaseName,(label, col, row) => new TemperatureButtonViewModel(label, col, row)},
            {Constants.SensorBaseName,(label, col, row) => new SensorButtonViewModel(label, col, row)},
        };
        
        private Dictionary<string, Func<string, ButtonViewModel>> _mappingsForJson = new Dictionary<string, Func<string, ButtonViewModel>>
        {
            {Constants.KeyBaseName,JsonConvert.DeserializeObject<ButtonViewModel> },
            {Constants.InputBaseName,JsonConvert.DeserializeObject< InputButtonViewModel>},
            {Constants.EventBaseName,JsonConvert.DeserializeObject< EventButtonViewModel>},
            {Constants.SlideBaseName,JsonConvert.DeserializeObject< SlideButtonViewModel>},
            {Constants.AnalogBaseName,JsonConvert.DeserializeObject< AnalogButtonViewModel>},
            {Constants.TimerBaseName,JsonConvert.DeserializeObject< TimerButtonViewModel>},
            {Constants.TemperatureBaseName,JsonConvert.DeserializeObject< TemperatureButtonViewModel>},
            {Constants.SensorBaseName,JsonConvert.DeserializeObject< SensorButtonViewModel>},
        };

        public ButtonViewModel CreateViewModel(string buttonName, int col, int row)
        {
            var key = _mappings.Keys.First(k => buttonName.StartsWith(k));
            return _mappings[key](buttonName, col, row);
        }

        public IEnumerable<ButtonViewModel> ReadFile(string fileName)
        {
            var content = File.ReadAllText(fileName);
            var jsonArray = JArray.Parse(content);
            foreach (var jToken in jsonArray)
            {
                var str = jToken.ToString();
                var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
                var buttonName = map[Constants.JsonButtonName].ToString();
                var key = _mappingsForJson.Keys.First(k => buttonName.StartsWith(k));
                var vm = _mappingsForJson[key](str);
                yield return vm;
            }
        }
    }
}