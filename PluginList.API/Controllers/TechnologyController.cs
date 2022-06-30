using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PluginList.API.Utils;
using PluginList.Entity;
using PluginList.Model;
using System.Security.Claims;

namespace PluginList.API.Controllers
{
    [ApiController]
    [Route("api/User/[controller]/[action]")]
    public class TechnologyController : Controller
    {
        PluginList_DBContext Context;
        public TechnologyController(PluginList_DBContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 添加技术
        /// </summary>
        /// <param name="technologyAddModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [EnableCors("any")]
        public IActionResult addTechnology(TechnologyAddModel technologyAddModel)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                Technology technology = new Technology();
                technology.userId = userID;
                technology.technologyName = technologyAddModel.technologyName;
                technology.status = Constant.STATE_IN;
                Context.Technology.Add(technology);
                Context.SaveChanges();
                return Ok("添加成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// 查看技术
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [EnableCors("any")]
        public IActionResult getTechnology()
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                List<Technology> technology = Context.Technology.Where(x => x.userId == userID && x.status == Constant.STATE_IN).ToList();
                return Ok(technology);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        /// <summary>
        /// 编辑技术
        /// </summary>
        /// <param name="technologyEdit"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [EnableCors("any")]
        public IActionResult editTechnology(TechnologyEditModel technologyEdit)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                Technology technology = Context.Technology.FirstOrDefault(x => x.id == technologyEdit.id && x.userId == userID && x.status == Constant.STATE_IN);
                technology.technologyName = technologyEdit.technologyName;
                Context.SaveChanges();
                return Ok("编辑成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        /// <summary>
        /// 删除技术
        /// </summary>
        /// <param name="technologyId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [EnableCors("any")]
        public IActionResult deleteTechnology(int technologyId)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                Technology technology = Context.Technology.FirstOrDefault(x => x.id == technologyId && x.userId == userID);
                if(technology != null)
                {
                    technology.status = Constant.STATE_DELETE;
                }
                Context.SaveChanges();
                return Ok("删除成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
