using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using Common.Helpers;
using DAndRElectronics;
using DAndRElectronics.ButtonViewModels;
using DAndRElectronics.View;
using NUnit.Framework;

namespace EngineeringTesting
{
    public class BinarySerializationTests
    {
        [OneTimeSetUp]
        public void ClassInitialize()
        {
            App.ConfigureServices();
        }

        [Test]
        public void CanDeserialize()
        {
            var vm = new KeyboardViewModel();
            vm.OpenJson("DAnRElectronicsTest.json");
            foreach (var buttonList in vm.AllButtons)
            {
                foreach (var viewModel in buttonList)
                {
                    for (var i = 0; i < viewModel.Outs.Count(); i++)
                    {
                        var outValue = viewModel.Outs.Skip(i).First();
                        if (!outValue.On)
                        {
                            outValue.Percent = 0;
                        }
                    }
                }
            }
            var binaryFilename = Path.GetTempFileName();
            using (var binWriter = new BinaryWriter(File.Open(binaryFilename, FileMode.Create)))
            {
                vm.Serialize(binWriter);
            }

            //Read back
            //Three bytes reserved
            var vmBinary = new KeyboardViewModel();
            vmBinary.OnAddEvent(null);
            using (var reader = new BinaryReader(File.Open(binaryFilename, FileMode.Open)))
            {
                vmBinary.Deserialize(reader);
            }

            //Create temp jso
            vmBinary._savePath = @"C:\Test\MasudBinary.json";
            vmBinary.SaveInternal();

            //Compare
            var allButtonsOriginal = vm.AllButtons.ToList();
            var allButtonsBinary = vmBinary.AllButtons.ToList();
            for (var i = 0; i < allButtonsBinary.Count; i++)
            {
                var binaryButtons = allButtonsBinary[i].ToList();
                var originalButtons = allButtonsOriginal[i].ToList();
                CompareButtonLists(originalButtons, binaryButtons);
            }

        }

        private void CompareButtonLists(List<ButtonViewModel> orig, List<ButtonViewModel> binary)
        {
            var properties = typeof(ButtonViewModel).GetProperties();
            for (var i = 0; i < orig.Count; i++)
            {
                var origBtn = orig[i];
                var binaryBtn = binary[i];
                CompareButtons(properties, origBtn, binaryBtn);
            }
        }

        private List<string> _ignoredProps = new List<string>
        {
            nameof(ButtonViewModel.SelectedViewModel),
            nameof(ButtonViewModel.OffColorPickerCommand),
            nameof(ButtonViewModel.OnColorPickerCommand),
            nameof(ButtonViewModel.AllOnOffCommand),
            nameof(ButtonViewModel.VoltageSignCommand),
            nameof(ButtonViewModel.TemperatureSignCommand),
            nameof(ButtonViewModel.Outs),
        };

        private void CompareButtons(PropertyInfo[] properties, ButtonViewModel origBtn, ButtonViewModel binaryBtn)
        {
            foreach (var prp in properties)
            {
                if (_ignoredProps.Contains(prp.Name))
                {
                    continue;
                }
                var origValue = prp.GetValue(origBtn, new object[] { });
                var binaryValue = prp.GetValue(binaryBtn, new object[] { });
                if (origValue == null && binaryValue == null)
                {
                    continue;
                }

                if (origValue == null)
                {
                    Assert.Fail(prp.Name);
                }

                if (binaryValue == null)
                {
                    Assert.Fail(prp.Name);
                }


                var origList = ToList(origValue);
                var binList = ToList(binaryValue);
                if (CompareEnumerables(properties,origList, binList))
                {
                    continue;
                }

                //We land here if there is a "SubButton" property with zero elements
                if (prp.Name == nameof(ButtonViewModel.SubButtons))
                {
                    continue;
                }

                Assert.IsTrue(origValue.Equals(binaryValue));
            }
        }


        private IEnumerable<object> ToList(object observable)
        {

            var arr = observable as IEnumerable;
            if (arr == null)
            {
                yield break;
            }
            foreach (var item in arr)
            {
                yield return item;
            }
        }

        private bool CompareEnumerables(PropertyInfo[] properties, IEnumerable<object> orig, IEnumerable<object> binary)
        {
            if (orig == null || !orig.Any())
            {
                return false;
            }
            if (binary == null || !binary.Any())
            {
                return false;
            }
            var origList = orig.ToList();
            var binaryList = binary.ToList();
            for (var i = 0; i < origList.Count; i++)
            {
                if (origList[i] is ButtonViewModel)
                {
                    if (i == 0)
                    {
                        continue;
                    }
                    CompareButtons(properties, origList[i] as ButtonViewModel, binaryList[i] as ButtonViewModel);
                    return true;
                }
                Assert.IsTrue(origList[i].Equals(binaryList[i]));
            }

            return true;
        }
    }
}