using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    public class ZipcodeController : BaseController<Zipcode>, IBaseController<Zipcode>
    {
        public ZipcodeController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }
  
        public async Task<IActionResult> Delete(int pZip)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{pZip}")]
        public async Task<IActionResult> DeleteStr (String pZip)
        {

            var trans = _context.Database.BeginTransaction();
            try
            {
                Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            _context.Remove(itmZipcode);
            await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
            }
        }

        [HttpGet]
        [Route("GetZipcode")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> itmZipcode = await _context.Zipcodes.ToListAsync();
            return Ok(itmZipcode);
        }

        public async Task<IActionResult> Get(int pZip)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetZipcode/{pZip}")]
        public async Task<IActionResult> Get(string pZip)
        {
            Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            return Ok(itmZipcode);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Zipcode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes.Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();

                if (_Zip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Zip = new Zipcode();
                _Zip.City = _Zipcode.City;
                _Zip.State = _Zipcode.State;
                    
                _context.Add(_Zip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zipcode.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Zipcode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes.Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();


                if (_Zip == null)
                {
                    bExist = false;
                    _Zip = new Zipcode();
                }
                else
                    bExist = true;

                _Zip.City = _Zipcode.City;
                _Zip.State = _Zipcode.State;
                if (bExist)
                    _context.Update(_Zip);
                else
                    _context.Add(_Zip);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zipcode.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
