using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;



public class Creature : INotifyPropertyChanged
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float Hunger { get; set; }
    public float Thirst { get; set; }
    public float Boredom { get; set; }
    public float Loneliness { get; set; }
    public float Stimulated { get; set; }
    public float Tired { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
}