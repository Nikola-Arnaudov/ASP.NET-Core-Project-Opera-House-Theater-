namespace OperaHouseTheater.Controllers.Ticket
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Ticket;
    using System.Linq;


    public class TicketController : Controller
    {
        private readonly OperaHouseTheaterDbContext data;

        public TicketController(OperaHouseTheaterDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            var memberId = this.data
                .Members
                .Where(m => m.UserId == this.User.GetId())
                .Select(m=> m.Id)
                .FirstOrDefault();

            if (memberId == 0)
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become),"Member");
            }

            var crrEvent = this.data.Events.FirstOrDefault(x => x.Id == id);

            //TODO: Error message
            if (crrEvent == null)
            {
                return BadRequest();
            }

            var eventPerformance = this.data.Performances.FirstOrDefault(x => x.Id == crrEvent.PerformanceId);
            var performanceType = this.data.PerformanceTypes.FirstOrDefault(x => x.Id == eventPerformance.PerformanceTypeId).Type;

            var ticketData = new BuyTicketFormModel
            {
                Title = eventPerformance.Title,
                PerformanceType = performanceType,
                Composer = eventPerformance.Composer,
                Date = crrEvent.Date,
                ImageUrl = eventPerformance.ImageUrl,
                FreeSeats = crrEvent.FreeSeats,
                CurrEventId = crrEvent.Id,
                TicketPrice = crrEvent.TicketPrice,
            };

            return View(ticketData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Buy(BuyTicketFormModel ticket)
        {
            if (!this.UserIsMember())
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            if (ticket.SeatsCount < 1 || ticket.SeatsCount > 50)
            {
                this.ModelState.AddModelError(nameof(ticket.SeatsCount), "You can choose from 1 to 50 seats.");
            }

            if (ticket.SeatsCount > ticket.FreeSeats)
            {
                this.ModelState.AddModelError(nameof(ticket.SeatsCount), $"There are only {ticket.FreeSeats} free seats left.");
            }

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            var userId = this.User.GetId();

            var member = this.data
                .Members
                .FirstOrDefault(x => x.UserId == userId);

            var ticketData = new Ticket
            {
                Amount = ticket.TicketPrice * ticket.SeatsCount,
                Composer = ticket.Composer,
                Title = ticket.Title,
                SeatsCount = ticket.SeatsCount,
                Date = ticket.Date,
                PerformanceType = ticket.PerformanceType,
                EventId = ticket.CurrEventId,
                MemberId = member.Id
            };

            var crrEvent = this.data.Events.FirstOrDefault(x => x.Id == ticketData.EventId);
            crrEvent.FreeSeats -= ticket.SeatsCount;
            data.SaveChanges();

            this.data.Tickets.Add(ticketData);
            this.data.SaveChanges();

            return RedirectToAction("All","Event");
        }


        //public IActionResult Delete()
        //{
        //};


        private bool UserIsMember()
            => this.data
                .Members
                .Any(x => x.UserId == this.User.GetId());
    }
}
