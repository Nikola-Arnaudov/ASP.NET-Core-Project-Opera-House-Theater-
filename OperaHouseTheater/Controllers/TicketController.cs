namespace OperaHouseTheater.Controllers.Ticket
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OperaHouseTheater.Controllers.Member;
    using OperaHouseTheater.Data;
    using OperaHouseTheater.Data.Models;
    using OperaHouseTheater.Infrastructure;
    using OperaHouseTheater.Models.Ticket;
    using OperaHouseTheater.Services.Events;
    using OperaHouseTheater.Services.Members;
    using OperaHouseTheater.Services.Performances;
    using OperaHouseTheater.Services.Tickets;
    using System;
    using System.Linq;

    using static WebConstants;

    public class TicketController : Controller
    {
        //private readonly OperaHouseTheaterDbContext data;
        private readonly IMemberService members;
        private readonly ITicketService tickets; 
        private readonly IEventService events;
        private readonly IPerformanceService performances;

        public TicketController(/*OperaHouseTheaterDbContext data,*/
            ITicketService tickets, 
            IMemberService members,
            IEventService events, 
            IPerformanceService performances)
        {
            //this.data = data;
            this.tickets = tickets;
            this.members = members;
            this.events = events;
            this.performances = performances;
        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            //var memberId = this.data
            //    .Members
            //    .Where(m => m.UserId == this.User.GetId())
            //    .Select(m => m.Id)
            //    .FirstOrDefault();

            var isMember = this.members.UserIsMember(this.User.GetId());

            if (!isMember && !User.IsAdmin())
            {
                //TODO
                //this.TempData

                return RedirectToAction(nameof(MemberController.Become), "Member");
            }

            var crrEvent = this.events.GetEventById(id); 

            //TODO: Error message
            if (crrEvent == null)
            {
                return RedirectToAction("Error","Home");
            }

            var eventPerformance = this.performances.GetPerformanceById(crrEvent.PerformanceId);

            var ticketData = new BuyTicketFormModel
            {
                Title = eventPerformance.Title,
                Composer = eventPerformance.Composer,
                ImageUrl = eventPerformance.ImageUrl,
                //PerformanceType = performanceType,
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

            if (ticket.Date < DateTime.Today)
            {
                this.ModelState.AddModelError(nameof(ticket.SeatsCount), $"The show is over.");
            }

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }

            var userId = this.User.GetId();

            //var member = this.data
            //    .Members
            //    .FirstOrDefault(x => x.UserId == userId);

            //var ticketData = new Ticket
            //{
            //    Amount = ticket.TicketPrice * ticket.SeatsCount,
            //    Composer = ticket.Composer,
            //    Title = ticket.Title,
            //    SeatsCount = ticket.SeatsCount,
            //    Date = ticket.Date,
            //    PerformanceType = ticket.PerformanceType,
            //    EventId = ticket.CurrEventId,
            //    MemberId = member.Id
            //};

            //var crrEvent = this.data.Events.FirstOrDefault(x => x.Id == ticketData.EventId);
            //crrEvent.FreeSeats -= ticket.SeatsCount;
            //data.SaveChanges();

            //this.data.Tickets.Add(ticketData);
            //this.data.SaveChanges();

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

            //var member = this.data
            //    .Members
            //    .FirstOrDefault(m => m.UserId == this.User.GetId());

            if (myTickets == null)
            {
                //TODO Error Message

                return BadRequest();
            }

            //var myTicketsData = new MyTicketsViewModel
            //{
            //    Id = member.Id,
            //    MemberName = member.MemberName,
            //    Tickets = this.data.Tickets
            //            .Where(t => t.MemberId == member.Id)
            //            .OrderBy(t => t.Date)
            //            .Select(t => new TicketListingViewModel
            //            {
            //                SeatsCount = t.SeatsCount,
            //                Amount = t.Amount,
            //                Date = t.Date,
            //                Title = t.Title,
            //                Id = t.Id,
            //                EventId = t.EventId
            //            })
            //            .ToList()
            //};

            return View(myTickets);
        }

        public IActionResult Delete(int id)
        {
            var memberId = this.members.GetMemberId(this.User.GetId());

            if (!this.tickets.IsCurrMembersTicket(id,memberId))
            {
                //TODO Message

                return BadRequest();
            }

            var ticketExist = this.tickets.Delete(id);

            if (ticketExist == false)
            {
                //TODO Message

                return BadRequest();
            }

            return RedirectToAction(nameof(All), "Ticket");
        }

    }
}
