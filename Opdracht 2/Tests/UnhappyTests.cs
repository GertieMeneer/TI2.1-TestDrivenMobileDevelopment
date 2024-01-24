namespace Tests
{
    public class UnhappyTests
    {
        [Fact]
        public async Task TestToggleLamp()
        {
            // Summary: Verifies that toggling a lamp multiple times results in only one invocation of ToggleLamp, ensuring consistent lamp state.
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();
            lampMock.Setup(l => l.ToggleLamp()).Returns(Task.CompletedTask);

            Lamp lamp = new Lamp();
            lamp.SetImplementation(lampMock.Object);

            // Act
            await lamp.Implementation.ToggleLamp();
            await lamp.Implementation.ToggleLamp();

            // Assert
            lampMock.Verify(l => l.ToggleLamp(), Times.Once);
        }

        [Fact]
        public async Task TestSetBrightness()
        {
            // Summary: Tests the behavior of setting lamp brightness, ensuring the correct invocation of SetBrightness and property value consistency.
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();

            lampMock.Setup(mock => mock.SetBrightness(It.IsAny<double>())).Callback((double value) => { lampMock.Setup(lamp => lamp.Brightness).Returns(value); });

            var lamp = lampMock.Object;

            // Act
            await lamp.SetBrightness(92.0);

            // Assert
            lampMock.Verify(l => l.SetBrightness(It.IsAny<double>()), Times.Once);
            Assert.Equal(50.0, lamp.Brightness);
        }

        [Fact]
        public async Task TestSetColor()
        {
            // Summary: Evaluates the behavior of setting lamp color, verifying the correct invocation of SetColor and property value consistency.
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();

            lampMock.Setup(mock => mock.SetColor(It.IsAny<int>(), It.IsAny<int>())).Callback((int hue, int sat) => { lampMock.Setup(lamp => lamp.Hue).Returns(hue); lampMock.Setup(lamp => lamp.Sat).Returns(sat); });

            var lamp = lampMock.Object;

            // Act
            await lamp.SetColor(444, 33);

            // Assert
            lampMock.Verify(l => l.SetColor(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(10, lamp.Hue);
            Assert.Equal(20, lamp.Sat);
        }

        [Fact]
        public async Task TestGetLamps()
        {
            // Summary: Tests error handling in MainViewModel during user ID retrieval and lamp loading, ensuring proper handling of error scenarios.
            // Arrange
            Mock<IMainViewModel> mainViewModelMock = new Mock<IMainViewModel>();

            mainViewModelMock.Setup(mock => mock.GetUserIdAsync()).ReturnsAsync("link button not pressed");
            mainViewModelMock.Setup(mock => mock.LoadLamps()).ReturnsAsync("Error: no userid");
            mainViewModelMock.Setup(mock => mock.ParseLights(It.IsAny<string>())).Returns(new List<Lamp>());

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