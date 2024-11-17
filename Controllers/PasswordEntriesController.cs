using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using passwordManagent2.Models;
using passwordManagent2.services;


namespace passwordManagent2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordEntriesController : ControllerBase
    {
        private readonly PasswordManagerContext _context;
        private readonly EncryptionPassword encryptionPassword = new EncryptionPassword();

        public PasswordEntriesController(PasswordManagerContext context)
        {
            _context = context;
        }

        // GET: api/PasswordEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PasswordEntry>>> GetpasswordEntry()
        {
            return await _context.passwordEntry.ToListAsync();
        }

        // GET: api/PasswordEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PasswordEntry>> GetPasswordEntry(long id)
        {
            var passwordEntry = await _context.passwordEntry.FindAsync(id);
            Console.WriteLine(passwordEntry?.Encryptedpassword); 

            if (passwordEntry == null)
            {
                return NotFound();
            }

            return passwordEntry;
        }

        // PUT: api/PasswordEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPasswordEntry(long id, PasswordEntry passwordEntry)
        {
            if (id != passwordEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(passwordEntry).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!PasswordEntryExists(id))
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

        // POST: api/PasswordEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PasswordEntry>> PostPasswordEntry(PasswordEntry passwordEntry)
        {
            // encrypta la informacion con un hash
            passwordEntry.Encryptedpassword = encryptionPassword.Encrypt(passwordEntry.Encryptedpassword, "200211");

            _context.passwordEntry.Add(passwordEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPasswordEntry", new { id = passwordEntry.Id }, passwordEntry);
        }

        // DELETE: api/PasswordEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePasswordEntry(long id)
        {
            var passwordEntry = await _context.passwordEntry.FindAsync(id);
            if (passwordEntry == null)
            {
                return NotFound();
            }

            _context.passwordEntry.Remove(passwordEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PasswordEntryExists(long id)
        {
            return _context.passwordEntry.Any(e => e.Id == id);
        }
    }
}
