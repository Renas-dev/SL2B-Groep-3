// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document
  .getElementById("theme-switcher")
  .addEventListener("click", function () {
    var currentTheme = document.documentElement.getAttribute("data-theme");
    var switchToTheme = currentTheme === "night" ? "day" : "night";
    document.documentElement.setAttribute("data-theme", switchToTheme);
  });

  document.getElementById('theme-switcher').addEventListener('click', function() {
    $.ajax({
      url: '/YourController/ToggleDayNight', // Replace with your controller and action
      type: 'POST',
      success: function(response) {
        var theme = response.isDay ? 'light' : 'dark';
        document.documentElement.setAttribute('data-theme', theme);
      }
    });
  });