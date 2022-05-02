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
    public class CourseController : BaseController<Course>, IBaseController<Course>
    {
        public CourseController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{pCourseNo}")]
        public async Task<IActionResult> Delete(int pCourseNo)
        {

            var trans = _context.Database.BeginTransaction();
            try
            {

                var _pres = await _context.Courses.Where(x => x.Prerequisite == pCourseNo).ToListAsync();

                foreach (var _pre in _pres)
                {

                    _pre.Prerequisite = null;
                    _pre.PrerequisiteSchoolId = null;
                    _context.Courses.Update(_pre);
                    //await _context.SaveChangesAsync();
                }

                Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
                _context.Remove(itmCourse);

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
        [Route("GetCourse")]
        public async Task<IActionResult> Get()
        {
            List<Course> itmCourse = await _context.Courses.ToListAsync();
            return Ok(itmCourse);
        }

        [HttpGet]
        [Route("GetCourse/{pCourseNo}")]
        public async Task<IActionResult> Get(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpPost]
        
        public async Task<IActionResult> Post([FromBody] Course _Course)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Crse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                if (_Crse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                    _Crse = new Course();
                    _Crse.Cost = _Course.Cost;
                    _Crse.Description = _Course.Description;
                    _Crse.Prerequisite = _Course.Prerequisite;
                    _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                    _Crse.SchoolId = _Course.SchoolId;
                    _context.Add(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Course)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Crse = await _context.Courses
                    .Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();


                if (_Crse == null)
                {
                    bExist = false;
                    _Crse = new Course();
                }
                else
                    bExist = true;

                _Crse.Cost = _Course.Cost;
                _Crse.Description = _Course.Description;
                _Crse.Prerequisite = _Course.Prerequisite;
                _Crse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                _Crse.SchoolId = _Course.SchoolId;
                if (bExist)
                    _context.Update(_Crse);
                else
                    _context.Add(_Crse);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
