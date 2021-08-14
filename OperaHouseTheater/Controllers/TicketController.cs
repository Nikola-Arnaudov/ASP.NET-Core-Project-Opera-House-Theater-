namespace OperaHouseTheater.Controllers.Ticket
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Ticket;
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Services.Members;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Tickets;

    using static WebConstants;

    public class TicketController : Controller
    {
        private readonly IMemberService members;
        private readonly ITicketService tickets; 
        private readonly IEventService events;
        private readonly IPerformanceService performances;

        public TicketController(
            ITicketService tickets, 
            IMemberService members,
            IEventService events, 
            IPerformanceService performances)
        {
            this.tickets = tickets;
            this.members = members;
            this.events = events;
            this.performances = performances;
        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            var isMember = this.members.UserIsMember(this.User.GetId());

            if (!isMember && !User.IsAdmin())
            {
                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            var crrEvent = this.events.GetEventById(id); 

            if (crrEvent == null)
            {
                TempData["ErrorMessage"] = "Тhe event doesn't exist.";

                return RedirectToAction("Error","Home");
            }

            var eventPerformance = this.performances.GetPerformanceById(crrEvent.PerformanceId);

            var ticketData = new BuyTicketFormModel
            {
                Title = eventPerformance.Title,
                Composer = eventPerformance.Composer,
                ImageUrl = eventPerformance.ImageUrl,
                PerformanceType = eventPerformance.PerformanceType,
                Date = crrEvent.Date,
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
            if (!this.members.UserIsMember(this.User.GetId()) && !User.IsAdmin())
            {
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

            if (ticket.Date < DateTime.Today)
            {
                this.ModelState.AddModelError(nameof(ticket.SeatsCount), $"The show is over.");
            }

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            var userId = this.User.GetId();

            this.tickets.Buy(userId, 
                ticket.TicketPrice,
                ticket.SeatsCount,
                ticket.Composer,
                ticket.Title,
                ticket.Date,
                ticket.PerformanceType,
                ticket.CurrEventId);

            return RedirectToAction("All", "Event");
        }

        [Authorize]
        public IActionResult All()
        {
            var myTickets = this.tickets.All(this.User.GetId());

            return View(myTickets);
        }

        public IActionResult Return(int id)
        {
            if (!this.tickets.TicketExist(id))
            {
                TempData["ErrorMessage"] = "Invalid ticket.";

                return RedirectToAction("Error", "Home");
            }

            var memberId = this.members.GetMemberId(this.User.GetId());

            if (!this.tickets.IsCurrMembersTicket(id,memberId))
            {
                TempData["ErrorMessage"] = "Invalid ticket.";

                return RedirectToAction("Error", "Home");
            }

            var ticketReturned = this.tickets.Return(id);

            if (ticketReturned == false)
            {
                TempData["ErrorMessage"] = "This ticket can't be returned.";

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(All), "Ticket");
        }

        public IActionResult Delete(int id)
        {
            var memberId = this.members.GetMemberId(this.User.GetId());

            if (!this.tickets.IsCurrMembersTicket(id, memberId))
            {
                TempData["ErrorMessage"] = "Invalid ticket.";

                return RedirectToAction("Error", "Home");
            }

            var ticketExist = this.tickets.Delete(id);

            if (ticketExist == false)
            {
                TempData["ErrorMessage"] = "Can't delete his ticket, because it still not expired.";

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(All), "Ticket");
        }

    }
}
