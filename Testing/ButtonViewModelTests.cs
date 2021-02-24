using System;
using System.Collections.Generic;
using System.IO;
using DAndRElectronics;
using DAndRElectronics.ButtonViewModels;
using DAndRElectronics.Enums;
using DAndRElectronics.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace EngineeringTesting
{
    public class ButtonViewModelTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [OneTimeSetUp]
        public void ClassInitialize()
        {
            App.ConfigureServices();
        }

        [Test]
        public void ReadWriteButtonViewModel()
        {

            var vm = new ButtonViewModel($"{Constants.AnalogBaseName}1", 0, 1) { OffBackgroundColor = SupportedColors.Blue, EquipmentType = Constants.DELAY, Pattern = 3, Priority = 3, Name = "MyName" };
            var filename = Path.GetTempFileName();
            File.WriteAllText(filename, vm.Serialize());

            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var vmNew = JsonConvert.DeserializeObject<ButtonViewModel>(json);
                Assert.IsTrue(vmNew.OffBackgroundColor.Equals(SupportedColors.Blue));
                Assert.IsTrue(vmNew.EquipmentType.Equals(Constants.DELAY));
                Assert.IsTrue(vmNew.Pattern.Equals(3));
                Assert.IsTrue(vmNew.Priority.Equals(3));
                Assert.IsTrue(vmNew.Name.Equals("MyName"));

            }

            File.Delete(filename);
        }

        [Test]
        public void CanSerializeSubKeys()
        {
            var vm = new ButtonViewModel($"{Constants.KeyBaseName}_1", 0, 1) { OffBackgroundColor = SupportedColors.Blue, EquipmentType = Constants.SEQUENTIAL, Pattern = 3, Priority = 3, Name = "MyName" };
            vm.NumSequences = 3;
            Assert.AreEqual(vm.SubButtons.Count, 4);
            var content = vm.Serialize();
            var map = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
            var numSequensec = Convert.ToInt32(map[Constants.JsonNumSequence]) ;
            var subButtons = map[Constants.JsonSequence] as JArray;
            Assert.AreEqual(subButtons.Count, 3);
            Assert.AreEqual(numSequensec, 3);
           
        }
    }
}