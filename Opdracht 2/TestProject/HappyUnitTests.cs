using System.Collections.ObjectModel;
using TDMD;
using TestProject.TDMD;

namespace TestProject
{
    public class HappyUnitTests
    {
        [Fact]
        public async Task TestLoadingLamps()
        {
            LampLoader loader = new LampLoader();

            await loader.LoadLamps();

            ObservableCollection<Lamp> Lamps = loader.GetLamps();

            Assert.True(Lamps.Count == 3);
        }

        [Fact]
        public async Task TestGetUserID()
        {
            bool result = await Communicator.GetUserIdAsync();

            Assert.True(result);
        }

        [Fact]
        public async Task TestToggleLamp()
        {
            LampLoader loader = new LampLoader();

            await loader.LoadLamps();

            ObservableCollection<Lamp> Lamps = loader.GetLamps();

            await Lamps[1].ToggleLamp();

            Assert.False(Lamps[1].Status);
        }

        [Fact]
        public async Task TestSetBrightness()
        {
            LampLoader loader = new LampLoader();

            await loader.LoadLamps();

            ObservableCollection<Lamp> Lamps = loader.GetLamps();

            await Lamps[1].SetBrightness(10);

            Assert.True(Lamps[1].Brightness == 10);
        }

        [Fact]
        public async Task TestSetColor()
        {
            LampLoader loader = new LampLoader();

            await loader.LoadLamps();

            ObservableCollection<Lamp> Lamps = loader.GetLamps();

            await Lamps[1].SetColor(400, 34);

            Assert.True(Lamps[1].Hue == 400 && Lamps[1].Sat == 34);
        }
    }
}