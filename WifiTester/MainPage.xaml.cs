﻿using System;
using System.ComponentModel;
using WifiTester.Services;
using Xamarin.Forms;

namespace WifiTester
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

       async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var scanner = DependencyService.Resolve<IWifiScanner>();
            var results = await scanner.ScanForWifi();

            //foreach(var res in results)
            //{
            //    Console.WriteLine(res);
            //}
       }
    }
}
