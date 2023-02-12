using Microsoft.VisualBasic.ApplicationServices;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using System.Windows.Forms;
using WeatherApp.startupValues;

namespace WeatherApp
{
    public partial class WeatherApp : Form
    {

        string api_key = new api().api_key;
        public WeatherApp()
        {

            InitializeComponent();
            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;
            string fileName = "city.txt";
            string path = System.IO.Path.Combine(projectDirectory, "startupValues", fileName);
            string text = System.IO.File.ReadLines(path).FirstOrDefault();
            string city = text;
            string url = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + city + "?unitGroup=metric&include=days%2Ccurrent&key=" + api_key + "&contentType=json";

            //try to connect to the url
            try
            {

                //connecting to the url
                string JSON = new WebClient().DownloadString(url);

                //use data from json
                UseWeatherData(JSON);

            }

            catch
            {

                //showing messagebox
                Dialog DialogForm = new Dialog();
                DialogForm.ShowDialog();
                cityTextBox.Text = "";

            }

        }

        private void UseWeatherData(string JSON)
        {

            //deserializing from JSON
            dynamic weather = JsonConvert.DeserializeObject(JSON);
            string currentCity = weather.resolvedAddress;

            //current conditions
            string currentDate = weather.currentConditions.datetime;
            float currentTemp = weather.currentConditions.temp;
            string currentIcon = weather.currentConditions.icon;
            int i = 0;

            //displaying current conditions
            foreach (Label _label in Controls.OfType<Label>())
            {
                
                if (_label.Name == "tempLabel")
                    _label.Text = currentTemp.ToString() + "°C";

                else if (_label.Name == "cityLabel")
                    _label.Text = currentCity;

                else if (_label.Name == "dateLabel")
                    _label.Text = currentDate;

                _label.Visible = true;

            }

            //displaying an item for current conditions
            foreach (PictureBox _pictureBox in Controls.OfType<PictureBox>())
            {

                if (_pictureBox.Name == "currentPictureBox")
                {

                    var enviroment = System.Environment.CurrentDirectory;
                    string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;
                    string fileName = currentIcon + ".png";
                    string path = System.IO.Path.Combine(projectDirectory, "img", fileName);
                    _pictureBox.Image = Image.FromFile(@path);
                    _pictureBox.Visible = true;

                }

            }

            //creating a list of objects of WeatherInfo class
            List<WeatherInfo> WeatherInfoList = new List<WeatherInfo>();

            //adding objects to the list
            foreach (var day in weather.days)
                WeatherInfoList.Add(new WeatherInfo(day.datetime.ToString(), (float)day.tempmax,
                    (float)day.tempmin, day.icon.ToString()));

            
            //displaying panel for each day
            foreach (Panel panel in Controls.OfType<Panel>())
            {

                panel.Visible = true;
                WeatherInfo weatherInfo = WeatherInfoList[i];
                i = i + 1;
                foreach (Control control in panel.Controls)
                {

                    //displaying item for each day 
                    if (control is PictureBox)
                    {

                        PictureBox pictureBox = control as PictureBox;
                        var enviroment = System.Environment.CurrentDirectory;
                        string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;
                        string fileName = weatherInfo.weather_icon + ".png";
                        string path = System.IO.Path.Combine(projectDirectory, "img", fileName);
                        pictureBox.Image = Image.FromFile(@path);
                        
                    }

                    //displaying date for each day 
                    if (control is TextBox)
                    {

                        TextBox textBox = control as TextBox;
                        textBox.Text = weatherInfo.weatherDate;

                    }

                    //displaying temperature for each day 
                    if (control is Label)
                    {

                        Label label = control as Label;

                        if (label.Name.ToString().StartsWith("max"))
                            label.Text = weatherInfo.weather_tmax.ToString() + "°C";

                        else
                            label.Text = weatherInfo.weather_tmin.ToString() + "°C";

                    }

                    control.Visible = true;

                }

            }

        }

        private void submitButtonClick(object sender, EventArgs e)
        {

            string city = cityTextBox.Text;
            string url = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + city + "?unitGroup=metric&include=days%2Ccurrent&key=" + api_key + "&contentType=json";
 
            //try to connect to the url
            try
            {

                //connecting to the url
                string JSON = new WebClient().DownloadString(url);

                //use data from json
                UseWeatherData(JSON);

            }

            catch
            {

                //showing messagebox
                Dialog DialogForm = new Dialog();
                DialogForm.ShowDialog();
                cityTextBox.Text = "";

            }

        }

        private void setUpButtonClick(object sender, EventArgs e)
        {

            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;
            string fileName = "city.txt";
            string path = System.IO.Path.Combine(projectDirectory, "startupValues", fileName);
            System.IO.File.WriteAllText(path, cityLabel.Text);

        }

    }

    class WeatherInfo
    {

        public WeatherInfo(string date, float tmax, float tmin, string icon) {

            weatherDate = date;
            weather_icon = icon;
            weather_tmax = tmax;
            weather_tmin = tmin;

        }

        public string weatherDate;
        public float weather_tmax;
        public float weather_tmin;
        public string weather_icon;

    }

}