using FoodDelivery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodDelivery.Pages.Final
{
    public class FindRestaurantModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FindRestaurantModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string JsonRestaurants { get; set; }

        public async Task OnGetAsync()
        {
            var features = new List<Feature>();
            var root = new Root { type = "FeatureCollection", features = features };
            int currentId = 0;

            var restaurateurs =
                (from r in _context.Restaurateurs
                 select r).ToList();

            foreach (var restaurateur in restaurateurs)
            {
                currentId++;

                string address = restaurateur.Address + " " + restaurateur.PostalCode + " " + restaurateur.City;
                var requestUri = new Uri("https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf6248e188e9625d034e4fbdebd050f8b08bef&text=" + address);
                double lat = 0, lng = 0;

                using (var httpClient = new HttpClient { BaseAddress = requestUri })
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");

                    using (var response = await httpClient.GetAsync(requestUri))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        ResRoot data = JsonConvert.DeserializeObject<ResRoot>(responseData);

                        if (data.features.Count != 0)
                        {
                            var coordinates = data.features.First().geometry.coordinates;

                            lat = coordinates.First();
                            lng = coordinates.Last();
                        }
                    }
                }

                var feature = new Feature
                {
                    type = "Feature",
                    geometry = new Geometry
                    {
                        type = "Point",
                        coordinates = new List<double> { lat , lng }
                    },
                    properties = new Properties
                    {
                        name =  restaurateur.Name,
                        address = restaurateur.Address,
                        city = restaurateur.City,
                        postalCode = restaurateur.PostalCode,
                        userId = restaurateur.UserId,
                        id = currentId
                    }
                };

                features.Add(feature);
            }

            JsonRestaurants = JsonConvert.SerializeObject(root);
        }
    }

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string userId { get; set; }
        public int id { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }



    public class ResGeometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class ResFeature
    {
        public string type { get; set; }
        public ResGeometry geometry { get; set; }
    }

    public class ResRoot
    {
        public string type { get; set; }
        public List<ResFeature> features { get; set; }
    }
}
