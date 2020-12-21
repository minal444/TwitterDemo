
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twitter_Profile.Model;

namespace Twitter_Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly TwitterContext _context;

        public UserProfilesController(TwitterContext context)
        {
            _context = context;
        }

        // GET: api/UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetUserProfiles()
        {
            return await _context.UserProfiles.Select(x => ProfileDTO(x)).ToListAsync();
        }

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return ProfileDTO(userProfile);
        }

        // PUT: api/UserProfiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, UserProfileDTO userProfileDTO)
        {
            if (id != userProfileDTO.Id)
            {
                return BadRequest();
            }

            //_context.Entry(userProfile).State = EntityState.Modified;
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
                return NotFound();

            userProfile.FirstName = userProfileDTO.FirstName;
            userProfile.LastName = userProfileDTO.LastName;
            userProfile.PictureURL = userProfileDTO.PictureURL;
            userProfile.Email = userProfileDTO.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserProfiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfileDTO userProfileDTO)
        {
            var userProfile = new UserProfile
            {
                Id = userProfileDTO.Id,
                LastName = userProfileDTO.LastName,
                FirstName = userProfileDTO.FirstName,
                UserName = userProfileDTO.UserName,
                PictureURL = userProfileDTO.PictureURL,
                Email = userProfileDTO.Email,
                JoinDate = DateTime.Now
            };
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetUserProfile", new { id = userProfile.Id }, userProfile);
            return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.Id }, ProfileDTO(userProfile));
        }

        // DELETE: api/UserProfiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserProfile>> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return userProfile;
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.Id == id);
        }

        private static UserProfileDTO ProfileDTO(UserProfile userProfile) =>
            new UserProfileDTO
            {
                Id = userProfile.Id,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PictureURL = userProfile.PictureURL,
                UserName = userProfile.UserName,
                Email= userProfile.Email,
                JoinDate = userProfile.JoinDate
            };
        
    }
}
