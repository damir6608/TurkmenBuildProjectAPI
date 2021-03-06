//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServerAPI.Classes
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using TurkmenBuildProjectServer.Classes;

    public partial class Vehicles
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public double MaxWeight { get; set; }
        public double RentPrice { get; set; }
        public double GasolineLevel { get; set; }
        public string Information { get; set; }
        public string Picture { get; set; }
    
        public virtual Rent Rent { get; set; }

        public Vehicles(string Name, double MaxWeight, double Price, double GasLevel, string Information, Rent rent)
        {
            this.VehicleName = Name;
            this.MaxWeight = MaxWeight;
            this.RentPrice = Price;
            this.GasolineLevel = GasLevel;
            this.Information = Information;
            this.Rent = rent;
            this.Picture = @"\pictures\vehicles\default-gazel.jpg";
        }

        public Vehicles(string Name, double MaxWeight, double Price, double GasLevel, string Information, Rent rent, string Picture)
        {
            this.VehicleName = Name;
            this.MaxWeight = MaxWeight;
            this.RentPrice = Price;
            this.GasolineLevel = GasLevel;
            this.Information = Information;
            this.Rent = rent;
            this.Picture = Picture;
        }

        public Vehicles() { }

        public string ToJson()
        {
            Image VehiclePicture = Image.FromFile(this.Picture);
            byte[] VehiclePictureArray = ByteConverter.ConvertToByteArray(VehiclePicture);

            Dictionary<string, object> VehicleDictionary = new Dictionary<string, object>()
            {
                { "Id", this.Id },
                { "VehicleName", this.VehicleName},
                { "MaxWeight", this.MaxWeight},
                { "RentPrice", this.RentPrice},
                { "GasolineLevel", this.GasolineLevel},
                { "Information", this.Information},
                { "Picture", Convert.ToBase64String(VehiclePictureArray)}
            };

            return JsonConvert.SerializeObject(VehicleDictionary);
        }
    }
}
