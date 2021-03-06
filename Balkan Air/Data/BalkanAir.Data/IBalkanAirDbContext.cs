﻿namespace BalkanAir.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Models;

    public interface IBalkanAirDbContext : IDisposable
    {
        IDbSet<Aircraft> Aircraft { get; set; }

        IDbSet<AircraftManufacturer> AircraftManufacturers { get; set; }

        IDbSet<Airport> Airports { get; set; }

        IDbSet<News> News { get; set; }

        IDbSet<Baggage> Baggages { get; set; }

        IDbSet<Booking> Booking { get; set; }

        IDbSet<Category> Categories { get; set; }

        IDbSet<Country> Countries { get; set; }

        IDbSet<CreditCard> CreditCards { get; set; }

        IDbSet<Fare> Fares { get; set; }

        IDbSet<Flight> Flights { get; set; }

        IDbSet<FlightLeg> FlightLegs { get; set; }

        IDbSet<LegInstance> LegInstances { get; set; }

        IDbSet<FlightStatus> FlightStatuses { get; set; }

        IDbSet<Notification> Notifications { get; set; }

        IDbSet<Route> Routes { get; set; }

        IDbSet<Seat> Seats { get; set; }

        IDbSet<TravelClass> TravelClasses { get; set; }

        IDbSet<UserNotification> UserNotifications { get; set; }

        IDbSet<Comment> Comments { get; set; }

        DbSet<TEntity> Set<TEntity>() 
            where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) 
            where TEntity : class;

        int SaveChanges();
    }
}
