using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Notes
{
    public class LocalCreatureStore : IDataStore<Creature>
    {
        public bool CreateItem(Creature item)
        {
            string creatureAsString = JsonConvert.SerializeObject(item);

            Preferences.Set("myCreature", creatureAsString);

            return true;
        }

        public bool DeleteItem(Creature item)
        {
            Preferences.Remove("myCreature");
            return true;
        }

        public Creature readItem()
        {
            string creatureAsString = Preferences.Get("myCreature", "");

            Creature creatureFromText = JsonConvert.DeserializeObject<Creature>(creatureAsString);

            return creatureFromText;
        }

        public bool UpdateItem(Creature item)
        {
            if (Preferences.ContainsKey("myCreature"))
            {
                string creatureAsString = JsonConvert.SerializeObject(item);

                Preferences.Set("myCreature", creatureAsString);
                return true;
            }
            return false;
        }
    }
}
