namespace Tests
{
    public class UnHappyUnitTests
    {
        [Fact]
        public async Task TestLoadingLamps_Unhappy()
        {
            // Arrange
            var mockMainViewModel = new Mock<IMainViewModel>();

            // Set up the behavior of the methods
            mockMainViewModel.Setup(mock => mock.LoadLamps()).ReturnsAsync("error");
            mockMainViewModel.Setup(mock => mock.ParseLights(It.IsAny<string>()))
                .Callback<string>(jsonResponse =>
                {
                    List<Lamp> lamps = new List<Lamp>();

                    try
                    {
                        JObject jsonObject = JObject.Parse(jsonResponse);
                        JObject lightsObject = jsonObject["lights"].ToObject<JObject>();

                        foreach (var keyValuePair in lightsObject)
                        {
                            string key = keyValuePair.Key;
                            JObject lightObject = keyValuePair.Value.ToObject<JObject>();

                            string id = key;
                            string name = lightObject["name"].ToString();
                            bool isOn = lightObject["state"]["on"].ToObject<bool>();
                            string type = lightObject["type"].ToString();
                            string swversion = lightObject["swversion"].ToString();
                            string uniqueid = lightObject["uniqueid"].ToString();
                            int brightness = lightObject["state"]["bri"].ToObject<int>();
                            int hue = lightObject["state"]["hue"].ToObject<int>();
                            int sat = lightObject["state"]["sat"].ToObject<int>();

                            Lamp lamp = new Lamp
                            {
                                ID = key,
                                Type = type,
                                Name = name,
                                ModelID = id,
                                SWVersion = swversion,
                                UniqueID = uniqueid,

                                Status = isOn,
                                Brightness = brightness,
                                BrightnessPercentage = brightness / 254.0 * 100.0,
                                Hue = hue,
                                Sat = sat
                            };

                            lamps.Add(lamp);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine($"Error: {e.Message}");
                    }

                    mockMainViewModel.Setup(x => x.ParseLights(It.IsAny<string>())).Returns(lamps);
                    mockMainViewModel.Setup(lamps => lamps.Lamps).Returns(lamps);
                })
                .Returns((string jsonResponse) => mockMainViewModel.Object.ParseLights(jsonResponse));

            mockMainViewModel.Setup(mock => mock.GetUserIdAsync()).ReturnsAsync("invalid");

            var mainViewModel = mockMainViewModel.Object;

            // Act
            string userId = await mainViewModel.GetUserIdAsync();

            string stringLampsJson;

            if (userId != null)
            {
                stringLampsJson = await mainViewModel.LoadLamps();
                mainViewModel.Lamps = mainViewModel.ParseLights(stringLampsJson);
            }

            // Assert
            Assert.Equal(3, mainViewModel.Lamps.Count);
            Assert.Equal("3ce1b7fbb5910827a555c50c749c42b", userId);
            Assert.NotNull(userId);
            Assert.NotEmpty(userId);
            Assert.NotEqual("", userId);
        }

        [Fact]
        public async Task TestLampFunctionality()
        {
            // Arrange
            var mockLamp = new Mock<ILamp>();
            var mockILampInfoPageViewModel = new Mock<ILampInfoPageViewModel>();

            // Set up the behavior of the mocks
            // Values of lamp mock
            mockLamp.Setup(lamp => lamp.ID).Returns("1");
            mockLamp.Setup(lamp => lamp.Type).Returns("SmartLED");
            mockLamp.Setup(lamp => lamp.Name).Returns("Cosmic Glow");
            mockLamp.Setup(lamp => lamp.ModelID).Returns("CG-2024");
            mockLamp.Setup(lamp => lamp.SWVersion).Returns("v2.0");
            mockLamp.Setup(lamp => lamp.UniqueID).Returns("CG-2024-1234");
            mockLamp.Setup(lamp => lamp.Status).Returns(false);
            mockLamp.Setup(lamp => lamp.Brightness).Returns(75);
            mockLamp.Setup(lamp => lamp.BrightnessPercentage).Returns(75);
            mockLamp.Setup(lamp => lamp.Hue).Returns(77);
            mockLamp.Setup(lamp => lamp.Sat).Returns(120);

            // Functions of lamp mock
            mockLamp.Setup(mock => mock.ToggleLamp()).Callback(() => { mockLamp.Setup(lamp => lamp.Status).Returns(!mockLamp.Object.Status); });
            mockLamp.Setup(mock => mock.SetBrightness(It.IsAny<double>())).Callback<double>((brightness) => { mockLamp.Setup(lamp => lamp.Brightness).Returns(brightness); });
            mockLamp.Setup(mock => mock.SetColor(It.IsAny<int>(), It.IsAny<int>())).Callback<int, int>((hue, sat) => { mockLamp.Setup(lamp => lamp.Hue).Returns(hue); mockLamp.Setup(lamp => lamp.Sat).Returns(sat); });

            // Functions of InfoPageViewModel
            mockILampInfoPageViewModel.Setup(mock => mock.PercentageToValue(It.IsAny<double>())).Returns<double>((percentage) => { return percentage / 100.0 * 253.0 + 1.0; });

            // Expose mock object instance
            var lamp = mockLamp.Object;
            var LampInfoPageViewModel = mockILampInfoPageViewModel.Object;

            // Act
            // Toggle
            Assert.True(lamp.Status == false);
            await lamp.ToggleLamp();
            Assert.True(lamp.Status == false);

            // Set brightness
            Assert.True(lamp.Brightness == 75);
            await lamp.SetBrightness(260);
            Assert.True(lamp.Brightness == 75); 

            // Set color
            Assert.True(lamp.Hue == 77 && lamp.Sat == 120);
            await lamp.SetColor(300, 10);
            Assert.True(lamp.Hue == 77 && lamp.Sat == 120);
        }
    }
}
