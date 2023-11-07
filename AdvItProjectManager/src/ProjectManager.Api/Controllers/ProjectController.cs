using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NodaTime;
using ProjectManager.Api.Controllers.Models.Projects;
using ProjectManager.Api.Controllers.Models.Todos;
using ProjectManager.Data;
using ProjectManager.Data.Entities;
using ProjectManager.Data.Interfaces;
using System.Reflection;
using System.Runtime.Intrinsics;

namespace ProjectManager.Api.Controllers;

[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    private readonly IClock _clock;
    private readonly AppDbContext _dbContext;

    [HttpPost("api/v1/Project")]
    public async Task<ActionResult> Create([FromBody] ProjectCreateModel model)
    {
        var newProject = new Project
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description
        };

        _dbContext.Add(newProject);

        return Ok();
    }

    [HttpGet("api/v1/Project")]
    public async Task<ActionResult<IEnumerable<ProjectDetailModel>>> GetList()
    {

        var dbEntities = await _dbContext.Set<Project>().Select(x => new TodoDetailModel
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description
        }).ToListAsync();

        return Ok(dbEntities);
    }

    [HttpGet("api/v1/Project/{id}")]
    public async Task<ActionResult<ProjectDetailModel>> Get([FromRoute] Guid id)
    {

        var dbEntity = await _dbContext.Set<Project>().Where(x => x.Id == id).Select(x => new ProjectDetailModel
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
        }).SingleOrDefaultAsync();
        if (dbEntity == null) return NotFound();
        return Ok(dbEntity);
    }

    [HttpPatch("api/v1/Project/{id}")]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] ProjectCreateModel patch)
    {

        var dbEntity = await _dbContext.Set<Project>().SingleOrDefaultAsync(x => x.Id == id);
        if (dbEntity == null) return NotFound();

        // some code for update

        return NoContent();
    }

    [HttpDelete("api/v1/Project/{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {

        var dbEntity = await _dbContext.Set<Project>().SingleOrDefaultAsync(x => x.Id == id);
        if (dbEntity == null) return NotFound();
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}


