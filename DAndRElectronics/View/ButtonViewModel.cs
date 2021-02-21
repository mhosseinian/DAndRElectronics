using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.View
{
    public enum SupportedColors
    {
        Black,
        Red,
        Green,
        Blue,
        Yellow,
        White
    }

    public class ButtonViewModel:ViewModel
    {
        [JsonProperty(PropertyName = "buttonName")] private string _buttonName;
        [JsonProperty(PropertyName = "name")] private string _name;
        [JsonProperty(PropertyName = "equipmentType")] private string _equipmentType;
        [JsonProperty(PropertyName = "priority")] private long _priority;
        [JsonProperty(PropertyName = "backgroundColor")] private string _backgroundColor;
        [JsonProperty(PropertyName = "pattern")] private string _pattern;

        
        [JsonIgnore]public string Name { get => _name; set => _name = value; }

        [JsonIgnore]public string EquipmentType { get => _equipmentType; set => _equipmentType = value; }
        
        [JsonIgnore]public long Priority { get => _priority; set => _priority = value; }

        [JsonIgnore]public string BackgroundColor { get => _backgroundColor; set => _backgroundColor = value; }

        [JsonIgnore]public string Pattern { get => _pattern; set => _pattern = value; }


        [JsonIgnore] public ICommand OnEdit { get; }

        [JsonIgnore] public string ButtonName { get => _buttonName; set => _buttonName = value; }

        [JsonIgnore] public int Column { get; set; }
        [JsonIgnore] public int Row { get; set; }

        #region Contructors

        public ButtonViewModel(string buttonName, int col, int row)
        {
            ButtonName = buttonName;
            Column = col;
            Row = row;
            OnEdit = new RelayCommand(OnButtonEdit, o => true);
           
        }

        #endregion

        private void OnButtonEdit(object obj)
        {
            
        }

        public static ButtonViewModel Deserialize(string content)
        {
            var vmNew = JsonConvert.DeserializeObject<ButtonViewModel>(content);
            return vmNew;
        }

    }
}