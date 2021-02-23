using System.IO;
using DAndRElectronics.ButtonViewModels;
using DAndRElectronics.Enums;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EngineeringTesting
{
    public class ButtonViewModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadWriteButtonViewModel()
        {

            var vm = new ButtonViewModel("Masud", 0, 1) { OffBackgroundColor = SupportedColors.Blue, EquipmentType = "SF42", Pattern = 3, Priority = 3, Name = "MyName" };
            var filename = Path.GetTempFileName();
            File.WriteAllText(filename, JsonConvert.SerializeObject(vm, Formatting.Indented));

            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var vmNew = JsonConvert.DeserializeObject<ButtonViewModel>(json);
                Assert.IsTrue(vmNew.OffBackgroundColor.Equals(SupportedColors.Blue));
                Assert.IsTrue(vmNew.EquipmentType.Equals("SF42"));
                Assert.IsTrue(vmNew.Pattern.Equals(3));
                Assert.IsTrue(vmNew.Priority.Equals(3));
                Assert.IsTrue(vmNew.Name.Equals("MyName"));

            }

            File.Delete(filename);
        }
    }
}