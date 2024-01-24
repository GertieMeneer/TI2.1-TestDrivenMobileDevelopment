namespace Tests
{
    public class HappyTests
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

        [Fact]
        public async Task TestSetBrightness()
        {
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
            // Arrange
            Mock<IMainViewModel> mainViewModelMock = new Mock<IMainViewModel>();

            mainViewModelMock.Setup(mock => mock.GetUserIdAsync()).ReturnsAsync("33c7f5c6a0668b02c6811fe6e811265");
            mainViewModelMock.Setup(mock => mock.LoadLamps()).ReturnsAsync("{\"lights\":{\"1\":{\"state\":{\"on\":true,\"bri\":254,\"hue\":4444,\"sat\":254,\"xy\":[0.0,0.0],\"ct\":0,\"alert\":\"none\",\"effect\":\"none\",\"colormode\":\"hs\",\"reachable\":true},\"type\":\"Extended color light\",\"name\":\"Hue Lamp 1\",\"modelid\":\"LCT001\",\"swversion\":\"65003148\",\"uniqueid\":\"00:17:88:01:00:d4:12:08-0a\",\"pointsymbol\":{\"1\":\"none\",\"2\":\"none\",\"3\":\"none\",\"4\":\"none\",\"5\":\"none\",\"6\":\"none\",\"7\":\"none\",\"8\":\"none\"}},\"2\":{\"state\":{\"on\":true,\"bri\":254,\"hue\":23536,\"sat\":144,\"xy\":[0.346,0.3568],\"ct\":201,\"alert\":\"none\",\"effect\":\"none\",\"colormode\":\"hs\",\"reachable\":true},\"type\":\"Extended color light\",\"name\":\"Hue Lamp 2\",\"modelid\":\"LCT001\",\"swversion\":\"65003148\",\"uniqueid\":\"00:17:88:01:00:d4:12:08-0b\",\"pointsymbol\":{\"1\":\"none\",\"2\":\"none\",\"3\":\"none\",\"4\":\"none\",\"5\":\"none\",\"6\":\"none\",\"7\":\"none\",\"8\":\"none\"}},\"3\":{\"state\":{\"on\":true,\"bri\":254,\"hue\":65136,\"sat\":254,\"xy\":[0.346,0.3568],\"ct\":201,\"alert\":\"none\",\"effect\":\"none\",\"colormode\":\"hs\",\"reachable\":true},\"type\":\"Extended color light\",\"name\":\"Hue Lamp 3\",\"modelid\":\"LCT001\",\"swversion\":\"65003148\",\"uniqueid\":\"00:17:88:01:00:d4:12:08-0c\",\"pointsymbol\":{\"1\":\"none\",\"2\":\"none\",\"3\":\"none\",\"4\":\"none\",\"5\":\"none\",\"6\":\"none\",\"7\":\"none\",\"8\":\"none\"}}},\"schedules\":{\"1\":{\"time\":\"2012-10-29T12:00:00\",\"description\":\"\",\"name\":\"schedule\",\"command\":{\"body\":{\"scene\":null,\"on\":true,\"xy\":null,\"bri\":null,\"transitiontime\":null},\"address\":\"/api/newdeveloper/groups/0/action\",\"method\":\"PUT\"}}},\"config\":{\"portalservices\":false,\"gateway\":\"192.168.2.1\",\"mac\":\"00:00:88:00:bb:ee\",\"swversion\":\"01005215\",\"linkbutton\":false,\"ipaddress\":\"192.168.12.24:80\",\"proxyport\":0,\"swupdate\":{\"text\":\"\",\"notify\":false,\"updatestate\":0,\"url\":\"\"},\"netmask\":\"255.255.255.0\",\"name\":\"Philips hue\",\"dhcp\":true,\"proxyaddress\":\"\",\"whitelist\":{\"newdeveloper\":{\"name\":\"test user\",\"last use date\":\"2012-10-29T12:00:00\",\"create date\":\"2012-10-29T12:00:00\"},\"f4ba21a791c1bb0a7b51a55a13a7460\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:07:54\",\"create date\":\"2024-01-24T12:07:54\"},\"36756fe1076a37c5490f6a84499598e\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:09:02\",\"create date\":\"2024-01-24T12:09:01\"},\"20f6b4103f6858862231bc0a3ea4970\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:12:48\",\"create date\":\"2024-01-24T12:12:48\"},\"724e85f2cb4cf31f35939cae99f7062\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:13:34\",\"create date\":\"2024-01-24T12:13:22\"},\"fbfaa71134eb6f554a78312b44211bf\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:14:25\",\"create date\":\"2024-01-24T12:14:24\"},\"b5621199f0288b90508c9e06ab98934\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:15:16\",\"create date\":\"2024-01-24T12:15:15\"},\"53163b9299a67fe39f2271135722cce\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:24:54\",\"create date\":\"2024-01-24T12:24:53\"},\"33c7f5c6a0668b02c6811fe6e811265\":{\"name\":\"my_hue_app#gertiemeneer\",\"last use date\":\"2024-01-24T12:25:21\",\"create date\":\"2024-01-24T12:25:21\"}},\"UTC\":\"2012-10-29T12:05:00\"},\"groups\":{\"1\":{\"name\":\"Group 1\",\"action\":{\"on\":true,\"bri\":254,\"hue\":33536,\"sat\":144,\"xy\":[0.346,0.3568],\"ct\":201,\"alert\":null,\"effect\":\"none\",\"colormode\":\"xy\",\"reachable\":null},\"lights\":[\"1\",\"2\"]}},\"scenes\":{}}");
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
