using System.Collections.Generic;
using Common;
using Newtonsoft.Json;
using PatternBuilderLib.Models;

namespace PatternBuilderLib.ViewModels.OutPattern
{
    public class OutPatternModelViewModel : ViewModel
    {
        [JsonIgnore] public OutPatternModel Model { get; private set; }
        

        #region Contructors

        public OutPatternModelViewModel(OutPatternModelViewModel vm)
        {
            Model = new OutPatternModel(vm.Model);
            Initialize();
        }
        public OutPatternModelViewModel(OutPatternModel model)
        {
            Model = model;
            Initialize();
        }

        #endregion

        private void Initialize()
        {
            ViewModels = new List<SingleOutPatternViewModel>();
            for(var index = 0; index < Model.Outs.Length; index++)
            {
                ViewModels.Add(new SingleOutPatternViewModel(index, Model));
            }
        }

        #region Properties

        [JsonIgnore]public List<SingleOutPatternViewModel> ViewModels { get; set; }

        public int CycleNumber { get; set; }

        #endregion

        public void Preview(bool isPreview)
        {
            foreach (var singleOutPatternViewModel in ViewModels)
            {
                singleOutPatternViewModel.IsInPreview = isPreview;
            }
        }
    }
}