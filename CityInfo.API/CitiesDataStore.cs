﻿using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities {  get; set; }

        public static CitiesDataStore Current { get; set; } = new CitiesDataStore();

        public CitiesDataStore() {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New Work City",
                    Description = "The one with the big park",
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with cathedral that was  never really finished"
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower"
                }
            };
        }
    }
}
