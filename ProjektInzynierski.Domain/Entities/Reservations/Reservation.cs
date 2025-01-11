namespace ProjektInzynierski.Domain.Entities.Reservations
{
    public class Reservation
    {
        public Reservation(int requestingUserId,
            Guid clientId,
            DateTime startDate,
            DateTime endDate)
        {
            Id = Guid.NewGuid();
            EquipmentIds = new List<Guid>();
            Status = ReservationStatus.Issued;
            ReservationDate = DateTime.UtcNow;
            RequestingUserId = requestingUserId;
            ClientId = clientId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Reservation(Guid id,
            int requestingUserId,
            Guid clientId,
            ReservationStatus status,
            DateTime startDate,
            DateTime endDate,
            DateTime reservationDate,
            List<Guid> equipmentIds)
        {
            Id = id;
            EquipmentIds = equipmentIds;
            Status = status;
            ReservationDate = reservationDate;
            RequestingUserId = requestingUserId;
            ClientId = clientId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guid Id { get; }
        public DateTime ReservationDate { get; }
        public int RequestingUserId { get; }
        public Guid ClientId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public ReservationStatus Status { get; private set;  }
        public List<Guid> EquipmentIds { get; set; }

        public void SetStatus(ReservationStatus status)
        {
            Status = status;
        }

        public void AddEquipment(Guid equipmentId)
        {
            if (EquipmentIds.Contains(equipmentId))
                return;
            EquipmentIds.Add(equipmentId);
        }

        public void RemoveEquipment(Guid equipmentId)
        {
            EquipmentIds.Remove(equipmentId);
        }
    }
}
