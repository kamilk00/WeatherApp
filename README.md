# WeatherApp

### Aim of the project

The aim of the project was to develop a simple Windows Forms app which uses data stored in JSON from https://weather.visualcrossing.com. The user can find information about the city, current weather and a 14-day weather forecast for the chosen city. The project helps me to get familiar with Windows Forms and using JSON in C#. 

### Description of the project

In "startupValues" directory, there is a hidden class "api" that stores an API key to the https://weather.visualcrossing.com.
If the user enters the name of the city in a textbox and submits it by clicking on "SUBMIT" button, the app tries to find the city in a service. If it's found, data is downloaded and the app window is refreshed. Otherwise, the message "CITY NOT FOUND" is shown. If the user clicks on "SET UP AS START CITY", the app changes startup value - the weather for the current city will be shown.

![image](https://user-images.githubusercontent.com/92810145/218309081-11d951bc-d9fd-4166-adb3-d9bc84cf94d2.png)
