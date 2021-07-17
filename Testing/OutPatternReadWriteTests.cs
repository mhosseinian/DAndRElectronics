using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DAndRElectronics;
using DAndRElectronics.ButtonViewModels;
using Common.Enums;
using Common.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using PatternBuilderLib.Models;
using PatternBuilderLib.ViewModels.OutPattern;


namespace EngineeringTesting
{
    public class OutPatternReadWriteTests
    {
        [Test]
        public void CanSaveAndReadJson()
        {
            var models = new OutPatternModels();
            var outModel = models.NewOutPatternModel();
            outModel.Outs[0] = 0;
            outModel.Outs[1] = 15;
            outModel.Outs[2] = 127;
            var vm = new OutPatternManagerViewModel(models);
            Assert.AreEqual(1, vm.Cycles.Count);
            vm._savePath = Path.GetTempFileName();
            vm.SaveInternal();
            //Delete the first line
            vm.DeleteCommand.Execute(vm.Cycles.First());
            Assert.AreEqual(0, vm.Cycles.Count);
            //Now open the file
            vm.OpenInternal();
            Assert.AreEqual(1, vm.Cycles.Count);
        }
        [Test]
        public void CanSaveAndReadBinary()
        {
            var models = new OutPatternModels();
            var outModel = models.NewOutPatternModel();
            outModel.Outs[0] = 0;
            outModel.Outs[1] = 15;
            outModel.Outs[2] = 127;
            var vm = new OutPatternManagerViewModel(models);
            Assert.AreEqual(1, vm.Cycles.Count);

            //Clone
            vm.CloneCommand.Execute(vm.Cycles.First());
            Assert.AreEqual(2, vm.Cycles.Count);
            vm._savePath = Path.GetTempFileName();
            vm.SaveInternal();
            //Get the bin file
            var binaryFilename = vm._savePath.Replace(".tmp", ".bin");
            models.Models.Clear();
            using (var reader = new BinaryReader(File.Open(binaryFilename, FileMode.Open)))
            {
                models.Read(reader);
            }
            Assert.AreEqual(2, models.Models.Count);
            var model_1 = vm.Cycles.First().Model.Outs;
            var model_2 = vm.Cycles.Skip(1).First().Model.Outs;
            for (var i = 0; i < model_1.Length; i++)
            {
                Assert.AreEqual(model_1[i], model_2[i]);
            }
            
        }

        [Test]
        public void CanAddDeleteClone()
        {
            var models = new OutPatternModels();
            var outModel = models.NewOutPatternModel();
            outModel.Outs[0] = 0;
            outModel.Outs[1] = 15;
            outModel.Outs[2] = 127;
            var vm = new OutPatternManagerViewModel(models);
            Assert.AreEqual(1, vm.Cycles.Count);
            //Clone
            vm.CloneCommand.Execute(vm.Cycles.First());
            var model_1 = vm.Cycles.First().Model.Outs;
            var model_2 = vm.Cycles.Skip(1).First().Model.Outs;
            for (var i = 0; i < model_1.Length; i++)
            {
                Assert.AreEqual(model_1[i], model_2[i]);
            }

            //Add
            vm.AddCommand.Execute(null);
            Assert.AreEqual(vm.SelectedItem, vm.Cycles.Last());
            Assert.AreEqual(3, vm.Cycles.Count);
            Assert.AreEqual(3, models.Models.Count);
           

            //Delete the first line
            vm.DeleteCommand.Execute(vm.Cycles.Skip(1).First());
            Assert.AreEqual(2, vm.Cycles.Count);
            Assert.AreEqual(2, models.Models.Count);
        }

    }
}