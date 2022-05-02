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
    public class GradeController : BaseController<Grade>, IBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            Grade itmStuID = await _context.Grades.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            _context.Remove(itmStuID);
            await _context.SaveChangesAsync();
            return Ok();
        }

        public Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        
        public async Task<IActionResult> Get(int pStudentId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetGrade/{pStudentId}/{pSectionId}/{pGradeTypeCode}/{pGradeCodeOccurrence}")]
        public async Task<IActionResult> GetGrade(int pStudentId, int pSectionId, String pGradeTypeCode, int pGradeCodeOccurrence)
        {
            Grade itmStuID = await _context.Grades.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();

            Grade itmSectID = await _context.Grades.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();

            Grade itmGradeType = await _context.Grades.Where(x => x.GradeTypeCode == pGradeTypeCode).FirstOrDefaultAsync(); 

            Grade itmGradeCodeOccurance = await _context.Grades.Where(x => x.GradeCodeOccurrence == pGradeCodeOccurrence).FirstOrDefaultAsync();
            
            return Ok(itmStuID);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Grade)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.Grades.Where(x => x.StudentId == _Grade.StudentId).FirstOrDefaultAsync();

                if (_Gra != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Gra = new Grade();
                _Gra.SectionId = _Grade.SectionId;
                _Gra.GradeTypeCode = _Grade.GradeTypeCode;
                _Gra.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                _Gra.NumericGrade = _Grade.NumericGrade;
                _Gra.Comments = _Grade.Comments;
                _Gra.SchoolId = _Grade.SchoolId;
                _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grade.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gra = await _context.Grades.Where(x => x.StudentId == _Grade.StudentId).FirstOrDefaultAsync();


                if (_Gra == null)
                {
                    bExist = false;
                    _Gra = new Grade();
                }
                else
                    bExist = true;

                _Gra.SectionId = _Grade.SectionId;
                _Gra.GradeTypeCode = _Grade.GradeTypeCode;
                _Gra.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;  
                _Gra.NumericGrade = _Grade.NumericGrade;
                _Gra.Comments = _Grade.Comments;
                _Gra.SchoolId = _Grade.SchoolId;
                if (bExist)
                    _context.Update(_Gra);
                else
                    _context.Add(_Gra);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grade.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
