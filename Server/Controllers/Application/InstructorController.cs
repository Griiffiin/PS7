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
    public class InstructorController : BaseController<Instructor>, IBaseController<Instructor>
    {
        public InstructorController(SWARMOracleContext context,
        IHttpContextAccessor httpContextAccessor)
        : base(context, httpContextAccessor)
        {
        }

        [HttpDelete]
        [Route("Delete/{pInstructorId}")]
        public async Task<IActionResult> Delete(int pInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            _context.Remove(itmInstructor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetInstructor")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> itmInstructor = await _context.Instructors.ToListAsync();
            return Ok(itmInstructor);
        }

        [HttpGet]
        [Route("GetInstructor/{pInstructorId}")]
        public async Task<IActionResult> Get(int pInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Inst = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();

                if (_Inst != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }


                _Inst = new Instructor();
                _Inst.Salutation = _Instructor.Salutation;
                _Inst.FirstName = _Instructor.FirstName;
                _Inst.LastName = _Instructor.LastName;
                _Inst.StreetAddress = _Instructor.StreetAddress;
                _Inst.Zip = _Instructor.Zip;
                _Inst.Phone = _Instructor.Phone;
                _Inst.SchoolId = _Instructor.SchoolId;
                _context.Add(_Inst);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Inst = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();


                if (_Inst == null)
                {
                    bExist = false;
                    _Inst = new Instructor();
                }
                else
                    bExist = true;

                _Inst.Salutation = _Instructor.Salutation;
                _Inst.FirstName = _Instructor.FirstName;
                _Inst.LastName = _Instructor.LastName;
                _Inst.StreetAddress = _Instructor.StreetAddress;
                _Inst.Zip = _Instructor.Zip;
                _Inst.Phone = _Instructor.Phone;
                _Inst.SchoolId = _Instructor.SchoolId;
                if (bExist)
                    _context.Update(_Inst);
                else
                    _context.Add(_Inst);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
