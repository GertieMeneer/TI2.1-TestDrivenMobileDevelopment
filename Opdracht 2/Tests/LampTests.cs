namespace Tests
{
    public class LampTests
    {
        [Fact]
        public async Task TestToggleLamp()
        {
            // Arrange
            Mock<ILamp> lampMock = new Mock<ILamp>();
            lampMock.Setup(l => l.ToggleLamp()).Returns(Task.CompletedTask);
            Lamp lamp = new Lamp();
            lamp.SetImplementation(lampMock.Object);

            // Act
            await lamp.Implementation.ToggleLamp();

            // Assert
            lampMock.Verify(l => l.ToggleLamp(), Times.Once);
        }

        //[Fact]
        //public void TestSetBrightness()
        //{
        //    // Arrange
        //    Mock<ILamp> lampMock = new Mock<ILamp>();
        //    lampMock.Setup(l => l.SetBrightness(It.IsAny<double>()))
        //    .Callback((double value) => lampMock.Object._brightness = value);
        //    lampMock.Setup(l => l.Brightness).Returns(() => lampMock.Object._brightness);
        //    Lamp lamp = new Lamp(lampMock.Object);

        //    // Act
        //    lamp.SetBrightness(50.0);

        //    // Assert
        //    Assert.Equal(50.0, lamp.Brightness);
        //}

        //[Fact]
        //public void TestSetColor()
        //{
        //    // Arrange
        //    Mock<ILamp> lampMock = new Mock<ILamp>();
        //    lampMock.Setup(l => l.SetColor(It.IsAny<int>(), It.IsAny<int>()))
        //    .Callback((int hue, int sat) => { lampMock.Object._hue = hue; lampMock.Object._sat = sat; });
        //    lampMock.Setup(l => l.Hue).Returns(() => lampMock.Object._hue);
        //    lampMock.Setup(l => l.Sat).Returns(() => lampMock.Object._sat);
        //    Lamp lamp = new Lamp(lampMock.Object);

        //    // Act
        //    lamp.SetColor(10, 20);

        //    // Assert
        //    Assert.Equal(10, lamp.Hue);
        //    Assert.Equal(20, lamp.Sat);
        //}
    }
}
