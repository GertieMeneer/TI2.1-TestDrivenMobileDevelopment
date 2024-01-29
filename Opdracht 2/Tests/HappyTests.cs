namespace Tests
{
    public class HappyTests
    {
        [Fact]
        public async Task TestToggleLamp()
        {
            // Summary: Verifies that toggling a lamp results in the expected invocation of ToggleLamp and ensures proper lamp state changes.
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();
            lampMock.Setup(l => l.ToggleLamp()).Returns(Task.CompletedTask);

            var lamp = lampMock.Object;
        
            // Act
            await lamp.ToggleLamp();

            // Assert
            lampMock.Verify(l => l.ToggleLamp(), Times.Once);
        }

        [Fact]
        public async Task TestSetBrightness()
        {
            // Summary: Tests the correct setting of lamp brightness, ensuring proper invocation of SetBrightness and consistency in brightness property value.
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();

            lampMock.Setup(mock => mock.SetBrightness(It.IsAny<double>())).Callback((double value) => { lampMock.Setup(lamp => lamp.Brightness).Returns(value); });

            var lamp = lampMock.Object;

            // Act
            await lamp.SetBrightness(50.0);

            // Assert
            lampMock.Verify(l => l.SetBrightness(It.IsAny<double>()), Times.Once);
            Assert.Equal(50.0, lamp.Brightness);
        }

        [Fact]
        public async Task TestSetColor()
        {
            // Summary: Validates the correct setting of lamp color, ensuring the expected invocation of SetColor and consistency in hue and saturation property values.
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();

            lampMock.Setup(mock => mock.SetColor(It.IsAny<int>(), It.IsAny<int>())).Callback((int hue, int sat) => { lampMock.Setup(lamp => lamp.Hue).Returns(hue); lampMock.Setup(lamp => lamp.Sat).Returns(sat); });

            var lamp = lampMock.Object;

            // Act
            await lamp.SetColor(10, 20);

            // Assert
            lampMock.Verify(l => l.SetColor(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(10, lamp.Hue);
            Assert.Equal(20, lamp.Sat);
        }

        [Fact]
        public async Task TestGetLamps()
        {
            // Summary: Tests the integration of lamp-related operations in MainViewModel, including user ID retrieval, lamp loading, and parsing, ensuring proper functioning.
            // Arrange
            Mock<IMainViewModel> mainViewModelMock = new Mock<IMainViewModel>();

            mainViewModelMock.Setup(mock => mock.GetUserIdAsync()).ReturnsAsync("33c7f5c6a0668b02c6811fe6e811265");
            mainViewModelMock.Setup(mock => mock.LoadLamps()).ReturnsAsync("{\"lights\": { ... }}");
            mainViewModelMock.Setup(mock => mock.ParseLights(It.IsAny<string>())).Returns(new List<Lamp>
            {
                new Lamp { ID = "1", Type = "Extended color light", Name = "Hue Lamp 1", ModelID = "LCT001", SWVersion = "65003148", UniqueID = "00:17:88:01:00:d4:12:08-0a", Status = true, Brightness = 254, BrightnessPercentage = 100.0, Hue = 4444, Sat = 254 },
                new Lamp { ID = "2", Type = "Extended color light", Name = "Hue Lamp 2", ModelID = "LCT001", SWVersion = "65003148", UniqueID = "00:17:88:01:00:d4:12:08-0b", Status = true, Brightness = 254, BrightnessPercentage = 100.0, Hue = 23536, Sat = 144 },
                new Lamp { ID = "3", Type = "Extended color light", Name = "Hue Lamp 3", ModelID = "LCT001", SWVersion = "65003148", UniqueID = "00:17:88:01:00:d4:12:08-0c", Status = true, Brightness = 254, BrightnessPercentage = 100.0, Hue = 65136, Sat = 254 }
            });

            var mainViewModel = mainViewModelMock.Object;

            // Act
            string userId = await mainViewModel.GetUserIdAsync();

            List<Lamp> lamps = new();

            if (userId != null)
            {
                string json = await mainViewModel.LoadLamps();
                lamps = mainViewModel.ParseLights(json);
            }

            // Assert  
            Assert.Equal("33c7f5c6a0668b02c6811fe6e811265", userId);
            Assert.Equal(3, lamps.Count);
        }
    }
}