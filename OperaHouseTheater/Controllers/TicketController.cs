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
                .Select(m => m.Id)
                .FirstOrDefault();

            if (memberId == 0)
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
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

            return RedirectToAction("All", "Event");
        }

        [Authorize]
        public IActionResult All()
        {
            var member = this.data
                .Members
                .FirstOrDefault(m => m.UserId == this.User.GetId());

            if (member == null)
            {
                //TODO Error Message

                return BadRequest();
            }

            var myTicketsData = new MyTicketsViewModel
            {
                Id = member.Id,
                MemberName = member.MemberName,
                Tickets = this.data.Tickets
                        .Where(t => t.MemberId == member.Id)
                        .OrderBy(t => t.Date)
                        .Select(t => new TicketListingViewModel
                        {
                            SeatsCount = t.SeatsCount,
                            Amount = t.Amount,
                            Date = t.Date,
                            Title = t.Title,
                            Id = t.Id,
                            EventId = t.EventId
                        })
                        .ToList()
            };

            return View(myTicketsData);
        }


        public IActionResult Delete(int id)
        {
            var ticket = this.data.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                //TODO Message

                return BadRequest();
            }

            this.data.Tickets.Remove(ticket);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All), "Ticket");
        }


        private bool UserIsMember()
            => this.data
                .Members
                .Any(x => x.UserId == this.User.GetId());
    }
}
