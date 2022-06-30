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
    public class PluginController : Controller
    {
        PluginList_DBContext Context;
        public PluginController(PluginList_DBContext context)
        {
            Context = context;
        }
        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="pluginAdd"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [EnableCors("any")]
        public IActionResult addPlugin(PluginAddModel pluginAdd)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                Plugin plugin = new Plugin();
                plugin.userId = userID;
                plugin.pluginName = pluginAdd.pluginName;
                plugin.instruction = pluginAdd.instruction;
                plugin.introduce = pluginAdd.introduce;
                plugin.webUrl = pluginAdd.webUrl;
                plugin.parTechnologyId = pluginAdd.parTechnologyId;
                plugin.status = Constant.STATE_IN;
                Context.Plugin.Add(plugin);
                Context.SaveChanges();
                return Ok("添加成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// 查看插件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [EnableCors("any")]
        public IActionResult getPlugin(int technologyId)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                List<Plugin> pluginList = Context.Plugin.Where(x => x.userId == userID && x.parTechnologyId == technologyId && x.status == Constant.STATE_IN).ToList();
                return Ok(pluginList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        /// <summary>
        /// 编辑插件
        /// </summary>
        /// <param name="pluginEdit"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [EnableCors("any")]
        public IActionResult editPlugin(PluginEditModel pluginEdit)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                Plugin plugin = Context.Plugin.FirstOrDefault(x => x.id == pluginEdit.id && x.userId == userID && x.status == Constant.STATE_IN);
                plugin.webUrl = pluginEdit.webUrl;
                plugin.instruction = pluginEdit.instruction;
                plugin.pluginName = pluginEdit.pluginName;
                plugin.introduce = pluginEdit.instruction;
                Context.SaveChanges();
                return Ok("编辑成功");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        /// <summary>
        /// 删除插件
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [EnableCors("any")]
        public IActionResult deletePlugin(int pluginId)
        {
            try
            {
                var auth = HttpContext.AuthenticateAsync();
                int userID = int.Parse(auth.Result.Principal.Claims.First(t => t.Type.Equals(ClaimTypes.Sid))?.Value);
                Plugin plugin = Context.Plugin.FirstOrDefault(x => x.id == pluginId && x.userId == userID);
                if (plugin != null)
                {
                    plugin.status = Constant.STATE_DELETE;
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
