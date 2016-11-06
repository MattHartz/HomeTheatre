using HomeTheatre.Core;
using HomeTheatre.Models;
using System.Linq;
using System.Web.Mvc;
using HomeTheatre.DataAccess;
using HomeTheatre.Dto.Models;

namespace HomeTheatre.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly ApplicationDbContext _userContext;

        public RoomsController(IRoomManager manager, ApplicationDbContext context)
        {
            _roomManager = manager;
            _userContext = context;
        }

        // GET: Room
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!_roomManager.DoesRoomExist(id.Value))
            {
                return View("Error");
            }

            return View();
        }

        [HttpGet]
        [ActionName("Create")]
        public ActionResult CreateRoomForm()
        {
            return View("Create");
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreateRoom(RoomViewModel model)
        {
            var user = GetUserId(User.Identity.Name);
            var room = MvcApplication.Mapper.Map<RoomViewModel, Room>(model);
            room.Owner = user;

            var roomId = _roomManager.CreateRoom(room);

            return RedirectToAction("Index", "Rooms", new { id = roomId });
        }

        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult DestroyRoom()
        {
            return RedirectToAction("Index", "Rooms", new { id = 0 });
        }

        private string GetUserId(string name)
        {
            return _userContext.Users.FirstOrDefault(x => x.UserName == name)?.Id;
        }
    }
}