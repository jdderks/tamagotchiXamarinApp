using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Notes.Views
{
    public enum GiveState
    {
        food = 0,
        water = 1,
        attention = 2,
        tired = 3
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ClickPage : ContentPage
    {
        private GiveState currentGiveState = GiveState.food;
        private int cookiesGiven = 0;
        private string buttonText = "Click to give me treat";
        private string cookiesGivenText = "0";
        public Creature Smiley { get; set; } = new Creature
        {
            Name = "Smiley",
            Hunger = 0.5f,
            Thirst = 0.5f,
            Boredom = 0.5f,
            Loneliness = 0.5f,
            Stimulated = 0.5f,
            Tired = 0.5f
        };


        public Command SetGiveStateCommand
        {
            get
            {
                Console.WriteLine("Binding succesful setgivestatecommand");
                return new Command<string>((x) => OnChangeModeButtonClicked(x));
            }
        }

        private void OnChangeModeButtonClicked(string stateNumberString)
        {
            int stateNumber = int.Parse(stateNumberString);
            currentGiveState = (GiveState)stateNumber;
            Console.WriteLine("current state: " + currentGiveState);
            switch (currentGiveState)
            {
                case GiveState.food:
                    UpdatePercentageText(Smiley.Hunger);
                    break;
                case GiveState.water:
                    UpdatePercentageText(Smiley.Thirst);
                    break;
                case GiveState.attention:
                    UpdatePercentageText(Smiley.Boredom);
                    break;
                case GiveState.tired:
                    UpdatePercentageText(Smiley.Tired);
                    break;
                default:
                    break;
            }
        }

        public ClickPage()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
            Smiley = creatureDataStore.readItem();
            if (Smiley == null)
            {
                Smiley = new Creature
                {
                    Name = "Smiley",
                    Hunger = 0.5f,
                    Thirst = 0.5f,
                    Boredom = 0.5f,
                    Loneliness = 0.5f,
                    Stimulated = 0.5f,
                    Tired = 0.5f
                  
                };
                creatureDataStore.CreateItem(Smiley);
            }



            BindingContext = this;
            InitializeComponent();

            var timer = new Timer
            {
                Interval = 1000 * 60, //Once a second (1000ms) every 60 seconds
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Smiley.Hunger -= 0.005f;
            Smiley.Thirst -= 0.02f;
            Smiley.Boredom -= 0.005f;
            Smiley.Tired += 0.01f;
            OnPropertyChanged();
            switch (currentGiveState)
            {
                case GiveState.food:
                    UpdatePercentageText(Smiley.Hunger);
                    break;
                case GiveState.water:
                    UpdatePercentageText(Smiley.Thirst);
                    break;
                case GiveState.attention:
                    UpdatePercentageText(Smiley.Boredom);
                    break;
                case GiveState.tired:
                    UpdatePercentageText(Smiley.Tired);
                    break;
                default:
                    break;
            }
        }

        private void UpdatePercentageText(float amount)
        {
            CookiesGivenText = (int)(amount * 100) + "%";
            OnPropertyChanged();
        }


        async void OnClickButtonClicked(object sender, EventArgs e)
        {


            switch (currentGiveState)
            {
                case GiveState.food:
                    if (Smiley.Hunger <= 1)
                    {
                        ClickVibrate();
                        ButtonText = "Thanks for the food!";
                        Smiley.Hunger += 0.01f;
                        UpdatePercentageText(Smiley.Hunger);
                        await HollowHappyJump();
                    }
                    else
                    {
                        ButtonText = "My belly is full...";
                    }
                    break;
                case GiveState.water:
                    if (Smiley.Thirst <= 1)
                    {
                        ClickVibrate();
                        ButtonText = "Thanks for the water!";
                        Smiley.Thirst += 0.01f;
                        UpdatePercentageText(Smiley.Thirst);
                        await HollowHappyJump();
                    }
                    else
                    {
                        ButtonText = "I've had enough water.";
                    }
                    break;
                case GiveState.attention:
                    if (Smiley.Boredom <= 1 && Smiley.Stimulated > 0 ||
                        Smiley.Stimulated <= 1 && Smiley.Stimulated > 0)
                    {

                        Smiley.Boredom += 0.01f;
                        Smiley.Stimulated += 0.01f;
                        Smiley.Tired += 0.01f;


                        ClickVibrate();
                        ButtonText = "Thanks for the attention!";
                        UpdatePercentageText(Smiley.Boredom);
                        await HollowHappyJump();

                    }
                    else
                    {
                        ButtonText = "I would like to be alone for a little bit.";
                    }
                    break;
                case GiveState.tired:
                    if (Smiley.Tired <= 1)
                    {
                        ClickVibrate();
                        ButtonText = "Thanks for letting me sleep...";
                        Smiley.Tired += 0.01f;
                        UpdatePercentageText(Smiley.Tired);
                        await HollowHappyJump();
                    }
                    else
                    {
                        ButtonText = "I'm really not sleepy right now";
                    }
                    break;
                default:
                    break;
            }


        }

        async Task HollowHappyJump()
        {
            //Makes the hollow jump happily
            uint timeout = 50;
            await hollow.TranslateTo(0, -15, timeout);
            await hollow.TranslateTo(0, 15, timeout);
            await hollow.TranslateTo(0, -10, timeout);
            await hollow.TranslateTo(0, 10, timeout);
            await hollow.TranslateTo(0, -5, timeout);
            await hollow.TranslateTo(0, 5, timeout);
            hollow.TranslationX = 0;
        }

        private void Time_ElapsedEvent(object sender, ElapsedEventArgs e)
        {

        }

        private void ClickVibrate()
        {
            try
            {
                // Vibrate for a tenth of a second
                var duration = TimeSpan.FromSeconds(0.001f);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                //feature not supported
                Console.Write(ex);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                // Other error has occurred.
            }
        }

        public int CookiesGiven
        {
            get => cookiesGiven;
            set
            {
                cookiesGiven = value;
                OnPropertyChanged();
            }
        }
        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }

        public string CookiesGivenText
        {
            get => cookiesGivenText;
            set
            {
                cookiesGivenText = value;
                OnPropertyChanged();
            }
        }

    }
}