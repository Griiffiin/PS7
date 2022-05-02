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
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, IBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context,
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
            GradeTypeWeight itmGTC = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();
            _context.Remove(itmGTC);
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
        [Route("GetGradeTypeWeight/{pSectionId}/{pGradeTypeCode}")]
        public async Task<IActionResult> GetGradeTypeWeight(int pSectionId, String pGradeTypeCode)
        {
            GradeTypeWeight itmGTC = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync();

            GradeTypeWeight itmSect = await _context.GradeTypeWeights.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();

            return Ok(itmGTC);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();

                if (_Gra != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Gra = new GradeTypeWeight();
                _Gra.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                _Gra.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                _Gra.DropLowest = _GradeTypeWeight.DropLowest;
                _Gra.SchoolId = _GradeTypeWeight.SchoolId;
                _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTypeWeight.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();


                if (_Gra == null)
                {
                    bExist = false;
                    _Gra = new GradeTypeWeight();
                }
                else
                    bExist = true;

                _Gra.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                _Gra.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                _Gra.DropLowest = _GradeTypeWeight.DropLowest;
                _Gra.SchoolId = _GradeTypeWeight.SchoolId;
                if (bExist)
                    _context.Update(_Gra);
                else
                    _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTypeWeight.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
