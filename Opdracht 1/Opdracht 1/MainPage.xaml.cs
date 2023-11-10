
namespace Opdracht_1
{
    public partial class MainPage : ContentPage
    {
        private List<Double> data = new List<Double>();
        private HttpClient httpClient = new HttpClient();
        private string url = "";

        public MainPage()
        {
            data.Add(0);
            data.Add(1);
            data.Add(2);
            data.Add(3);
            data.Add(4);

            InitializeComponent();
            GetValues();
        }

        public async void GetValues()
        {
            ValueList.ItemsSource = data;

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage respones = await httpClient.SendAsync(message);

        }

    }

}
