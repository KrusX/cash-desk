//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashTicket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public int id_reservation { get; set; }
        public int ticket_id { get; set; }
        public System.DateTime date_reservation { get; set; }
        public int client_id { get; set; }
        public int status_reservation_id { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Status_res Status_res { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
