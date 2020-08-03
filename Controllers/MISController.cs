using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A91WEBAPI.DAL;
using A91WEBAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace A91WEBAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MISController : ControllerBase
    {
        private readonly IMISRepository _MISRepo;

        public MISController(IMISRepository MISRepo)
        {
            _MISRepo = MISRepo;

        }

        [HttpPost("postmisdata")]
        public async Task<IActionResult> postmisdata(MISUploadVM VM)
        {

            if (VM != null)
            { 
                var a = _MISRepo.PostMISData(VM);
                return StatusCode(201);
            }
            else
                return StatusCode(401);
        }
    }
}
