using Microsoft.AspNetCore.Mvc;
using SWARM.Shared.DTO;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Base
{
    public interface IBaseController<T>
    {
        [HttpDelete]
        Task<IActionResult> Delete(int itmID);
        [HttpGet]
        Task<IActionResult> Get();
        [HttpGet]
        Task<IActionResult> Get(int itmID);
        [HttpPost]
        Task<IActionResult> Post([FromBody] T _itm);
        [HttpPut]
        Task<IActionResult> Put([FromBody] T _itm);
    }
}