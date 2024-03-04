using Microsoft.AspNetCore.Mvc;
using NBP_backend.Services;
using System.Threading.Tasks;
using System;

namespace NBP_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServices _reviewServices;

        public ReviewController(ReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpPost]
        [Route("ReviewPoduct/{text}/{username}/{idProduct}/{recommend}")]
        public async Task<IActionResult> Create(String text, String username, int idProduct,bool recommend)
        {
            _reviewServices.ReviewPoduct(text, username, idProduct, recommend);
            return Ok("Uspelo");
        }


        [HttpGet]
        [Route("GetReview/{ID}")]
        public async Task<IActionResult> GetReview(int ID)
        {
            var review =  _reviewServices.GetReview(ID);
            return Ok(review);
        }

        [HttpDelete]
        [Route("ReviewPoduct/{idReview}")]
        public async Task<IActionResult> Delete(int idReview)
        {
            _reviewServices.DeleteReviewPoduct(idReview);
            return Ok("Uspelo");
        }

    }
}
