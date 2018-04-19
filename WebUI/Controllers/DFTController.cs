using System.Collections.Generic;
using Lab3;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DFTController : Controller
    {
        private readonly IDFTManager _manager;
        public DFTController(IDFTManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("getTransformed")]
        public IEnumerable<double> GetTransformed([FromBody] List<double> inputSignal)
        {
            _manager.SetNewSignal(inputSignal);

            var result = _manager.CalculateDFT();
            var realPart = result.real;

            return realPart;
        }
    }
}