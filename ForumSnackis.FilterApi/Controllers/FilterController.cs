using ForumSnackis.FilterApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.FilterApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilterController : ControllerBase
    {
        FilterService service;
            public FilterController(FilterService service)
            {
            this.service = service;
            }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]string content)
        {
          return Ok(service.CheckString(content)); 
        }
    }
}
