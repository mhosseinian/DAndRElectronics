
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Common.Converters;
using DAndRElectronics.ButtonViewModels;
using Common.Enums;
using Common.Helpers;
using Common.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using PatternBuilderLib;
using PatternBuilderLib.ViewModels;

namespace EngineeringTesting
{
    public class SerializeManagerTests
    {
        private IntToSolidColorBrushValueConverter _converter = new IntToSolidColorBrushValueConverter();

        private int ColorToInt(Color color)
        {
            var obj = _converter.ConvertBack(new SolidColorBrush(color), typeof(int), null, CultureInfo.CurrentCulture);
            return (int) obj;
        }

        private Color IntToColor(int color)
        {
            var brush = _converter.Convert(color, typeof(int), null, CultureInfo.CurrentCulture) as SolidColorBrush;
            return brush.Color;
        }


        [OneTimeSetUp]
        public void ClassInitialize()
        {
            Assert.IsNotNull(ServiceDirectory.Instance);
        }

        [Test]
        public void CanReadBinaryOutput()
        {
            var vm = new CyclesManageViewModel(12, false);
            var device = vm.SelectedItem.Devices.First();

            vm.SelectedItem.Delay = 22;
            device.Color = ColorToInt(Colors.Red);

            device = vm.SelectedItem.Devices.Skip(1).First();
            device.Color = ColorToInt(Colors.Yellow);

            //Add another cycle
            vm.AddCommand.Execute(null);
            vm.SelectedItem.Delay = 44;
            device = vm.SelectedItem.Devices.First();

            device.Color = ColorToInt(Colors.Green);

            device = vm.SelectedItem.Devices.Skip(1).First();
            device.Color = ColorToInt(Colors.Blue);

            var file = Path.GetTempFileName();
            vm.SerializeBinaryToFile(file);
            using BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open));

            var nCycles = reader.ReadInt32();
            Assert.AreEqual(nCycles, 2);

            var nDevices = reader.ReadInt32();
            Assert.AreEqual(nDevices, 12);

            //First cycle
            var delay = reader.ReadInt32();
            Assert.AreEqual(22, delay);
            //Read the colors
                    var expectedColor = Colors.Red;
            CheckColor(vm, reader, expectedColor);
            
            delay = reader.ReadInt32();
            Assert.AreEqual(44, delay);
                    expectedColor = Colors.Green;
            CheckColor(vm, reader, expectedColor);


        }

        private void CheckColor(CyclesManageViewModel vm, BinaryReader reader, Color expectedColor)
        {
            for (var i = 0; i < vm.NumDevices; i++)
            {
                var r = reader.ReadByte();
                var g = reader.ReadByte();
                var b = reader.ReadByte();
                if (i == 0)
                {
                    var intColor = r << 16 | g << 8 | b;
                    var color = IntToColor(intColor);
                    Assert.AreEqual(expectedColor, color);
                }
            }
        }
    }
}