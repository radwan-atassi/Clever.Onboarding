using Clever.Onboarding.CampaignDataAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clever.Onboarding.CampaignDataAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CampaignsController(ICampaignService campaignService) : ControllerBase
{
    private readonly ICampaignService _campaignService = campaignService ?? throw new ArgumentNullException(nameof(campaignService));


    /// <summary>
    /// Retrieves a campaign by its code.
    /// </summary>
    /// <param name="code">The campaign code.</param>
    /// <returns>The campaign details if found; otherwise, a NotFound response.</returns>
    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return BadRequest(new { Message = "Campaign code must be provided." });
        }

        var campaign = _campaignService.GetCampaignByCode(code);

        if (campaign == null)
        {
            return NotFound(new { Message = $"Campaign with code {code} not found." });
        }

        return Ok(campaign);
    }
}