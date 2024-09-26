using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using ApartmentTrackingService.Services;
using ApartmentTrackingService.Models;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;


namespace ApartmentTrackingService.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {
        private readonly SingletonHttpClient _singletonHttpClient;
        private readonly ApartmentRepository _apartmentRepository;
        private readonly UserRepository _userRepository;

        public ApartmentController(SingletonHttpClient singletonHttpClient, UserRepository userRepository, ApartmentRepository apartmentRepository)
        {

            _singletonHttpClient = singletonHttpClient;
            _apartmentRepository = apartmentRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeOnApartment(string url, string mail)
        {
            Regex emailRegex = new Regex(@"^[a-zA-Z0-9.]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(mail.ToLower()))
                return BadRequest("Некорректная почта");

            if (url[url.Length - 1].Equals('/'))
                url = url.Substring(0, url.Length - 1);

            Regex urlRegex = new Regex(@"^https:\/\/prinzip\.su\/apartments\/[a-zA-Z_-]+\/\d+$");
            if (!urlRegex.IsMatch(url))
                return BadRequest("Некорректная ссылка");


            var apartment = await _apartmentRepository.FindByUrlAsync(url);
            if (apartment is null)
            {
                apartment = new Apartment() {
                    Id = new Guid(),
                    URL = url,
                };
                var createaprt = await _apartmentRepository.CreateAsync(apartment);
                if (!createaprt)
                    return BadRequest("Ошибка при добавлении ссылки");
            }
            var user = await _userRepository.FindByMailAsync(mail);
            if (user is null)
            {
                user = new User()
                {
                    Id = new Guid(),
                    Email = mail,
                    Apartments = new List<Apartment>() { apartment }
                };
                var createuser = await _userRepository.CreateAsync(user);
                if (createuser)
                    return Ok();
                else return BadRequest();
            }else
            {
                user.Apartments.Add(apartment);

                var updateuser = await _userRepository.ChangeAsync(user);
                if (updateuser)
                    return Ok();
                else return BadRequest();
            }
        }

        [HttpGet]
        public async Task<Dictionary<string, int>> GetApartment(string mail)
        {
            var result = new Dictionary<string, int>();
            var user = await _userRepository.FindByMailAsync(mail);
            if (user is null)
                return null;
            foreach(var el in user.Apartments)
            {
                var httpClient = _singletonHttpClient.Instance;
                var response = await httpClient.GetAsync($"{el.URL}?ajax=1&similar=1");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var price = JsonDocument.Parse(jsonString).RootElement.GetProperty("price").ToString();
                    result.Add(el.URL, Int32.Parse(price));
                }
                else
                    result.Add(el.URL, -1);
            }
            if (result.Count == 0)
                return null;
            else return result;
        }
    }
}
