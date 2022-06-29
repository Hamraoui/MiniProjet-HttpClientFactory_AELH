using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myApiV1._0.Data;
using myApiV1._0.Models;
using myApiV1._0.Tools;

namespace myApiV1._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context) => _context = context;
        /*public UserController(UserDbContext context)
        {
         this._context = context;
           
        }*/

        /* Action method executed in response to an
        HTTP Request to get all users , decoration specify that the method
         will handle http get requests */

        [HttpGet]
        public async Task<IEnumerable<user>> Get()
        
           => await _context.Users.ToListAsync();

        /* decoration : action to resp to requests that have the id at end of url.
         * Iaction type used because we returning 2 types of responses */
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(user), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            // returns not found ( 404 )  if no user , else OK ( 200 )
            return user == null ? NotFound() : Ok(user);

        }

        /* the parameter user is bound to the body of req.*/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> create(user user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.id }, user);
        }



        /* the id param is bound to the url id.
         * the user is bound to the request body.
         * id placeholder decoration = id specified to be in url.
         */

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> update (int id , user user)
        {
            // test : if there is mismatch between url ID and the body request
            if (id != user.id) return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent(); // return 204 status code in resp.
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete (int id)
        {
            /* test if the user exists  => if it dosent return not found. 
             * else remove the user */

            var userToDelete = _context.Users.FindAsync(id);
            if (userToDelete == null) return NotFound();

            _context.Users.Remove(await userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin([FromBody] user user)
        {
            try
            {
                String password = Password.hashPassword(user.password);
                // username from db
                var dbUser = _context.Users.Where(u => u.username == user.username && u.password == password).Select(u => new
                {
                    u.id,
                    u.username
                }).FirstOrDefault();
                if (dbUser == null)
                {
                    return BadRequest("Username or Password is incorrect");
                }

                return Ok("You Are Logged in , Welcome " + dbUser.username); // return dbuser if it exists in db.

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }

        //User/api/register

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> userRegistration([FromBody] user user)
        {
           
                var dbUser = _context.Users.Where(u => u.username == user.username).FirstOrDefault();
                if (dbUser != null)
                {
                    return BadRequest("Username already exists");
                }

                // encrypt body request password ( user pw )
                user.password =  Password.hashPassword(user.password);
            
                 _context.Add(user);
                  await _context.SaveChangesAsync();

                return Ok("User Is Successfully Registered");
        }

    }
}
