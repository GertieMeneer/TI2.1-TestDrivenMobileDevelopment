using System.Collections.ObjectModel;
using System.Diagnostics;
using TDMD;
using TestProject.TDMD;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            LampLoader loader = new LampLoader();

            await loader.LoadLamps();

            ObservableCollection<Lamp> Lamps = loader.GetLamps();

            //Assert.True(Lamps.Count == 2);
            Assert.True(Lamps.Count == 3);
        }
    }
}