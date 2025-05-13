using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auction.Application.Common.Attributes.Field.CityValidation
{
    public class CityValidator : ValidationAttribute
    {
        private static readonly List<City> _cities = LoadCities();

        private static readonly HashSet<string> _cityNames = new(
            _cities.Select(c => c.Name),
            StringComparer.OrdinalIgnoreCase  
        );

        private static List<City> LoadCities()
        {
            string jsonPath = @"C:\Users\Влад\Desktop\Developer\CSharp\Auction\Backend\Auction.Backend\russian-cities.json";
            string json = File.ReadAllText(jsonPath);
            return JsonSerializer.Deserialize<List<City>>(json)!;
        }

        public override bool IsValid(object? value)
        {
            if (value is string cityName)
            {
                if (!_cityNames.Contains(cityName))
                {
                    ErrorMessage = "Город не найден";
                    return false;
                }
                return true;
            }
            ErrorMessage = "Некорректный тип данных";
            return false;
        }
    }
}
