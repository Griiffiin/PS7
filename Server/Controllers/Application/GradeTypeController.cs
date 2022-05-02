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
    public class GradeTypeController : BaseController<GradeType>, IBaseController<GradeType>
    {
        public GradeTypeController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }

        public Task<IActionResult> Delete(int itm)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(String pGradeTypeCode)
        {
            GradeType itmStuID = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            _context.Remove(itmStuID);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }


        public Task<IActionResult> Get(int pGradeTypeCode)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetGradeType/{pGradeTypeCode}")]
        public async Task<IActionResult> GetGradeType(String pGradeTypeCode)
        {
            GradeType itmGTC = await _context.GradeTypes.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            
            return Ok(itmGTC);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _GradeType)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

                if (_Gra != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Gra = new GradeType();
                _Gra.Description = _GradeType.Description;
                _Gra.SchoolId = _GradeType.SchoolId;
                _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeType.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeType)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();


                if (_Gra == null)
                {
                    bExist = false;
                    _Gra = new GradeType();
                }
                else
                    bExist = true;

                _Gra.Description = _GradeType.Description;
                _Gra.SchoolId = _GradeType.SchoolId;
                if (bExist)
                    _context.Update(_Gra);
                else
                    _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeType.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}