using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCEvent.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents ()
        {
            using (CalendarEntities CE = new CalendarEntities())
            {
                var events = CE.Events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Events events)
        {
            var status = false;
            using (CalendarEntities calendar = new CalendarEntities())
            {
                if (events.EventId > 0)
                {
                    var eve = calendar.Events.Where( c => c.EventId == events.EventId).FirstOrDefault();
                    if(eve != null)
                    {
                        eve.EventId = events.EventId;
                        eve.Subject = events.Subject;
                        eve.Start = events.Start;
                        eve.End = events.End;
                        eve.Description = events.Description;
                        eve.IsFullDay = events.IsFullDay;
                        eve.ThemeColor = events.ThemeColor;
                    }
                } else
                {
                    calendar.Events.Add(events);
                }
                calendar.SaveChanges();
                status = true;
            }
                
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int EventId)
        {
            var status = false;
            using (CalendarEntities calendar = new CalendarEntities())
            {
                var events = calendar.Events.Where(e => e.EventId == EventId).FirstOrDefault();
                if (events != null)
                {
                    calendar.Events.Remove(events);
                    calendar.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}