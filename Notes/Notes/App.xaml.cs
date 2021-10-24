using System;

using Xamarin.Forms;
using System.Diagnostics;
using Xamarin.Essentials;

namespace Notes
{
    public partial class App : Application
    {
        public App()
        {
            DependencyService.RegisterSingleton<IDataStore<Creature>>(new LocalCreatureStore());

            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            var sleepTime = DateTime.Now;
            Preferences.Set("SleepTime", sleepTime);
        }

        protected override void OnResume()
        {
            var sleepTime = Preferences.Get("SleepTime", DateTime.Now);
            var wakeTime = DateTime.Now;

            TimeSpan timeAsleep = wakeTime - sleepTime;
        }
    }
}
