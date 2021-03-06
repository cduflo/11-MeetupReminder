﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MeetupReminder.Core;
using MeetupReminder.Core.Services;
using MeetupReminder.Core.Domain;
using Spring.Social.OAuth1;
using System.Collections.ObjectModel;

namespace MeetupReminder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            comboBox.ItemsSource = Reference.queryType;
            buttonPinSubmit.Opacity = 0;
            textBoxPIN.Opacity = 0;

            dataGrid.IsReadOnly = true;
            dataGrid.Columns.Add(addColumn("Name", "name"));
            dataGrid.Columns.Add(addColumn("Status", "status"));
            dataGrid.Columns.Add(addColumn("Time", "time"));
        }

        OAuthToken j;

        public DataGridTextColumn addColumn(string header, string source)
        {
            DataGridTextColumn x = new DataGridTextColumn();
            x.Header = header;
            x.Binding = new Binding(source);
            return x;
        }


        public async void button_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show("Please make a query selection.");
            }
            else
            {
                j = await MeetupReminder.Core.Services.MeetupService.authenticate();
                MessageBox.Show("Please enter your PIN.");
                buttonPinSubmit.IsEnabled = true;
                buttonPinSubmit.Opacity = 100;
                textBoxPIN.IsEnabled = true;
                textBoxPIN.Opacity = 100;
            }
        }

        public async void buttonPinSubmit_Click(object sender, RoutedEventArgs e)
        {
            buttonPinSubmit.IsEnabled = false;
            buttonPinSubmit.Opacity = 0;
            textBoxPIN.IsEnabled = false;
            textBoxPIN.Opacity = 0;
            OAuthToken u = await MeetupService.authenticate2(j, textBoxPIN.Text);
            ObservableCollection<MeetupEvent> data = await MeetupService.GetMeetupsFor(textBoxMeetupGroup.Text, comboBox.SelectedItem.ToString(), u);
            dataGrid.ItemsSource = data;
            
            var message = "Here are your requested meetups:";
            ObservableCollection<string> MEvents = new ObservableCollection<string>();
            foreach (var i in data)
               {
                   MEvents.Add($"\n\nName: {i.name}\nStatus: {i.status}\nTime: {i.time}");
               }
            foreach (var x in MEvents)
            {
                message += x;
            }

            SmsService.SendSms(textBoxPhone.Text, message); //Issues if the message gets too long, won't arrive via SMS
       
            SmsService.Call(textBoxPhone.Text, "http://demo.twilio.com/welcome/voice/");



        }

    }
}
