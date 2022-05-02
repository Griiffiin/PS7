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
    public class GradeConversionController : BaseController<GradeConversion>, IBaseController<GradeConversion>
    {
        public GradeConversionController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{pLetterGrade}")]
        public async Task<IActionResult> Delete(String pLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.LetterGrade == pLetterGrade).FirstOrDefaultAsync();
            _context.Remove(itmGradeConversion);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> Delete(int itm)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetGradeConversion")]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(int itm)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetGradeConversion/{pLetterGrade}")]
        public async Task<IActionResult> Get(String pLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.LetterGrade == pLetterGrade).FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _GradeConversion)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.GradeConversions.Where(x => x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (_Gra != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Gra = new GradeConversion();
                _Gra.GradePoint = _GradeConversion.GradePoint;
                _Gra.MaxGrade = _GradeConversion.MaxGrade;
                _Gra.MinGrade = _GradeConversion.MinGrade;
                _Gra.SchoolId = _GradeConversion.SchoolId;
                _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeConversion.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _GradeConversion)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.GradeConversions.Where(x => x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();


                if (_Gra == null)
                {
                    bExist = false;
                    _Gra = new GradeConversion();
                }
                else
                    bExist = true;

                _Gra.GradePoint = _GradeConversion.GradePoint;
                _Gra.MaxGrade = _GradeConversion.MaxGrade;
                _Gra.MinGrade = _GradeConversion.MinGrade;
                _Gra.SchoolId = _GradeConversion.SchoolId;
                _Gra.SchoolId = _GradeConversion.SchoolId;
                if (bExist)
                    _context.Update(_Gra);
                else
                    _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeConversion.LetterGrade);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
