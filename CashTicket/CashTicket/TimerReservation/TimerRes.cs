using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using CashTicket.Models;
using System.Data.Entity;

namespace CashTicket.TimerReservation
{
    public class TimerRes : IHttpModule
    {
        /*       private static void SetTimer()
               {
                   Timer aTimer = new Timer(60000);
                   aTimer.Elapsed += OnTimedEvent;
                   aTimer.AutoReset = true;
                   aTimer.Enabled = true;
               }

               public static void OnTimedEvent(Object source, ElapsedEventArgs e)
               {
                   CashDeskEntities db = new CashDeskEntities();
                   var reservations = db.Reservations.Include(r => r.Client).Include(r => r.Status_res).Include(r => r.Ticket);
                   foreach (var item in reservations.ToList())
                   {
                       if (DateTime.Now > (item.date_reservation).AddDays(1))
                       {
                           db.Reservations.Remove(item);
                       }
                   }
               }
        */

        static Timer timer;
        long interval = 1800000;
        static object synclock = new object();

        public void Init(HttpApplication app)
        {
            timer = new Timer(new TimerCallback(DeleteRes), null, 0, interval);
        }

        private void DeleteRes(object obj)
        {
            lock (synclock)
            {
                CashDeskEntities db = new CashDeskEntities();
                var reservations = db.Reservations.Include(r => r.Client).Include(r => r.Status_res).Include(r => r.Ticket);

                foreach (var item in reservations.ToList())
                {
                    if (DateTime.Now >= (item.date_reservation).AddDays(1) )
                    {
                        var tick = db.Tickets.Where(c => c.id_ticket == item.ticket_id).FirstOrDefault();
                        tick.status_ticket_id = 1;
                        db.Reservations.Remove(item);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void Dispose()
        {

        }
    }
}