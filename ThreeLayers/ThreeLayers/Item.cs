/// <summary>
/// Item
/// </summary>
using Newtonsoft.Json;
using System;

namespace ThreeLayers
{
    class ParkInfo
    {
        [JsonProperty("type")]
        public PlaneType Type { get; set; }

        [JsonProperty("seats")] //кол-во вмещаемых людей в 1 самолете
        public int Seats { get; set; }

        [JsonProperty("ammount")] //всего этой модели
        public int Count { get; set; }

        public override string ToString() => $"Type = {Type} Seats = {Seats} Count = {Count}";
    }

    public enum PlaneType
    {
        Airbus_A220 = 0,
        Airbus_A310 = 1,
        Airbus_A320,
        Airbus_A330,
        Airbus_A340,
        Airbus_A350,
        Airbus_A380,
        Boeing_717,
        Boeing_737,
        Boeing_747,
        Boeing_757,
        Boeing_767,
        Boeing_777,
        Boeing_787,
        ATR_42_72,
        BAe_Avro_RJ,
        Bombardier_Dash_8,
        Bombardier_CRJ,
        Embraer_ERJ,
        Embraer_170_190,
        Saab,
        SUPERJET_100,
        Typolev_Ty_204,
        Ilushin_Il_96,
        Ilushin_Il_114,
        Antonov_An_38,
        Antonov_An_140,
        Antonov_An_148,
        Comac_ARJ21,
        Comac_C919,
        CRAIC_CR929,
        Harbin_Y_12,
        Xian_MA60,
        //invalidType //если указано выше 32 или ниже 0
    }

    public class Item
    {
        [JsonProperty("Base_id")]
        public uint base_id { get; set; } //id в базе данных

        [JsonProperty("flight_id")]
        public string Id { get; set; } //id рейса

        [JsonProperty("type")]
        public PlaneType Type { get; set; } //тип самолета

        [JsonProperty("date")]
        public DateTime Date { get; set; } //дата совершения рейса

        [JsonProperty("status")]
        public int Status { get; set; } //оценка качества самолета

        [JsonProperty("onboard_passangers")]
        public int Passengers { get; set; } //кол-во пассажиров на самолете

        [JsonProperty("lethals")]
        public int Deaths { get; set; } //смертей во время перелета

        public override string ToString() => '#' + Id + ", Type = " + Type +
            ", Day = " + Date.Day + ", Status = " + Status +
            ", Passengers = " + Passengers + ", RIP = " + Deaths;
    }

    class Probe
    {
        public int Day { get; set; }
        public double Chance { get; set; }
        public PlaneType Type { get; set; }

        public override string ToString() => $"{Type.ToString()}, {Day.ToString()}, {Chance.ToString()}";
    }
}