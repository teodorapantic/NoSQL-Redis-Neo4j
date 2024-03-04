using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBP_backend.Models;
using NBP_backend.Services;

namespace NBP_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

      

        //private readonly ILogger<UserController> _logger;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Get()
        {
            //var rng = await _client.Cypher.Match("(n:WeatherForecast)")
            //                              .Return(n => n.As<User>()).ResultsAsync;
            
            return Ok(_userServices.GetAll());
        }

    
        [HttpPost]
        [Route("CreateUser/{username}/{password}/{Name}/{SurName}/{PhoneNumber}/{Location}")]
        public async Task<IActionResult> Create(String username, String password, String Name, String SurName, String PhoneNumber, String Location)
        {
            _userServices.CreateUser(username, password, Name, SurName, PhoneNumber, Location );
            return Ok("Uspelo");
        }

        [HttpGet]
        [Route("LogIn/{username}/{password}")]
        public IActionResult LogIn(String username, String password)
        {
            try
            {
                Task<User> res = _userServices.LogInUser(username, password);
                User res1 = res.Result;
                if (res1 == null)
                {
                    return BadRequest("Korisnik ne postoji ili ste pogresli parametre za prijavu");
                }
               
                return Ok(res1);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("FollowProduct/{IDUser}/{IDProduct}")]
        public IActionResult FollowProduct(int IDUser, int IDProduct)
        {
            try
            {
                Task<bool> res = _userServices.FollowProduct(IDUser, IDProduct);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno ste zapratili proizvod");
                }
                return BadRequest("Nista");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("UnFollowProduct/{IDUser}/{IDProduct}")]
        public IActionResult UnFollowProduct(int IDUser, int IDProduct)
        {
            try
            {
                Task<bool> res = _userServices.UnFollowProduct(IDUser, IDProduct);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno ste otpratili proizvod");
                }
                return BadRequest("Nista");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetRecommended/{IDUser}")]
        public async Task<IActionResult> GetRecommended(int IDUser)
        {
            //async itd..
            try
            {
                List<Product> prod = _userServices.GetRecommended(IDUser);
                return Ok(prod);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetRecommendedSecond/{IDUser}")]
        public async Task<IActionResult> GetRecommendedSecond(int IDUser)
        {
            //async itd..
            try
            {
                List<Product> prod = _userServices.GetRecommendedSecond(IDUser);
                return Ok(prod);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetRecommendedByUsers")]
        public async Task<IActionResult> GetRecommendedByUsers()
        {
            //async itd..
            try
            {
                List<Product> prod = _userServices.GetRecommendedByUsers();
                return Ok(prod);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //kreiraj ako postoji, ima kod bogdanovica 
        [HttpPut]
        [Route("SearchedProducts/{IDUser}/{IDProduct}")]
        public IActionResult SearchedProducts(int IDUser, int IDProduct)
        {
            try
            {
                Task<bool> res = _userServices.SearchedProducts(IDUser, IDProduct);
                bool res1 = res.Result;
                if (res1)
                {
                    return Ok("Uspesno ste zapratili proizvod");
                }
                return BadRequest("Vec je pretrazen");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        
    }
}
