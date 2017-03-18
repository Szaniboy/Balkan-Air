﻿namespace BalkanAir.Web.Areas.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data.Common;
    using Data.Models;
    using Models.Flights;
    using Services.Common;
    using Services.Data.Contracts;

    [EnableCors("*", "*", "*")]
    [Authorize(Roles = UserRolesConstants.ADMINISTRATOR_ROLE)]
    public class FlightsController : ApiController
    {
        private readonly ILegInstancesServices legInstancesServices;

        public FlightsController(ILegInstancesServices legInstancesServices)
        {
            this.legInstancesServices = legInstancesServices;
        }

        [HttpPost]
        public IHttpActionResult Create(FlightRequestModel flight)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var flightToAdd = Mapper.Map<LegInstance>(flight);
            var addedFlightId = this.legInstancesServices.AddLegInstance(flightToAdd);

            return this.Ok(addedFlightId);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult All()
        {
            var flights = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .OrderByDescending(l => l.Departure)
                .ToList();
                
            return this.Ok(flights);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(ErrorMessages.INVALID_ID);
            }

            var flight = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .FirstOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return this.NotFound();
            }

            return this.Ok(flight);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetByFlightNumber(string flightNumber)
        {
            if (string.IsNullOrEmpty(flightNumber))
            {
                return this.BadRequest(ErrorMessages.NULL_OR_EMPTY_FLIGHT_NUMBER);
            }

            var flight = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .Where(f => f.Number.ToLower() == flightNumber.ToLower())
                .OrderByDescending(f => f.Departure)
                .ToList();

            return this.Ok(flight);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetByFlightStatus(string flightStatus)
        {
            if (string.IsNullOrEmpty(flightStatus))
            {
                return this.BadRequest(ErrorMessages.NULL_OR_EMPTY_FLIGHT_STATUS);
            }

            var flights = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .Where(f => f.FlightStatus.Name.ToLower() == flightStatus.ToLower())
                .OrderByDescending(f => f.Departure)
                .ToList();

            return this.Ok(flights);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetByDepartureAirport(string airportAbbreviation)
        {
            if (string.IsNullOrEmpty(airportAbbreviation))
            {
                return this.BadRequest(ErrorMessages.ABBREVIATION_CANNOT_BE_NULL_OR_EMPTY);
            }

            var flights = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .Where(f => f.DepartureAirport.Abbreviation.ToLower() == airportAbbreviation.ToLower())
                .OrderByDescending(f => f.Departure)
                .ToList();

            return this.Ok(flights);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetByArrivalAirport(string airportAbbreviation)
        {
            if (string.IsNullOrEmpty(airportAbbreviation))
            {
                return this.BadRequest(ErrorMessages.ABBREVIATION_CANNOT_BE_NULL_OR_EMPTY);
            }

            var flights = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .Where(f => f.ArrivalAirport.Abbreviation.ToLower() == airportAbbreviation.ToLower())
                .OrderByDescending(f => f.Departure)
                .ToList();

            return this.Ok(flights);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetByRoute(string originAbbreviation, string destinationAbbreviation)
        {
            if (string.IsNullOrEmpty(originAbbreviation) || string.IsNullOrEmpty(destinationAbbreviation))
            {
                return this.BadRequest(ErrorMessages.ABBREVIATION_CANNOT_BE_NULL_OR_EMPTY);
            }

            var flights = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .Where(f => f.DepartureAirport.Abbreviation.ToLower() == originAbbreviation.ToLower() &&
                            f.ArrivalAirport.Abbreviation.ToLower() == destinationAbbreviation.ToLower())
                .OrderByDescending(f => f.Departure)
                .ToList();

            return this.Ok(flights);
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetFromDateTime(DateTime dateTime)
        {
            var flights = this.legInstancesServices.GetAll()
                .ProjectTo<FlightResponseModel>()
                .Where(f => f.Departure.Equals(dateTime))
                .OrderByDescending(f => f.Departure)
                .ToList();

            return this.Ok(flights);
        }

        [HttpPut]
        public IHttpActionResult Update(int id, UpdateFlightRequestModel flight)
        {
            if (id <= 0)
            {
                return this.BadRequest(ErrorMessages.INVALID_ID);
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var flightToUpdate = Mapper.Map<LegInstance>(flight);
            var updatedFlight = this.legInstancesServices.UpdateLegInstance(id, flightToUpdate);

            if (updatedFlight == null)
            {
                return this.NotFound();
            }

            return this.Ok(updatedFlight.Id);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return this.BadRequest(ErrorMessages.INVALID_ID);
            }

            var deletedFlight = this.legInstancesServices.DeleteLegInstance(id);

            if (deletedFlight == null)
            {
                return this.NotFound();
            }

            return this.Ok(deletedFlight.Id);
        }
    }
}