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
    public class EnrollmentController : BaseController<Enrollment>, IBaseController<Enrollment>
    {
        public EnrollmentController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int itm)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(int itm)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("GetEnrollment/{pStudentId}/{pSectionId}")]
        public async Task<IActionResult> Get(int pStudentId, int pSectionId)
        {
            Enrollment itmSect = await _context.Enrollments.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();

            Enrollment itmStu = await _context.Enrollments.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();


            return Ok(itmSect);
        } 

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Enr = await _context.Enrollments.Where(x => x.SectionId == _Enrollment.SectionId).FirstOrDefaultAsync();

                if (_Enr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Enr = new Enrollment();
                _Enr.StudentId = _Enrollment.StudentId;
                _Enr.EnrollDate = _Enrollment.EnrollDate;
                _Enr.FinalGrade = _Enrollment.FinalGrade;
                _Enr.SchoolId = _Enrollment.SchoolId;
                _context.Add(_Enr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enrollment.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Enr = await _context.Enrollments.Where(x => x.SectionId == _Enrollment.SectionId).FirstOrDefaultAsync();


                if (_Enr == null)
                {
                    bExist = false;
                    _Enr = new Enrollment();
                }
                else
                    bExist = true;

                _Enr.StudentId = _Enrollment.StudentId;
                _Enr.EnrollDate = _Enrollment.EnrollDate;
                _Enr.FinalGrade = _Enrollment.FinalGrade;
                _Enr.SchoolId = _Enrollment.SchoolId;
                if (bExist)
                    _context.Update(_Enr);
                else
                    _context.Add(_Enr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enrollment.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
