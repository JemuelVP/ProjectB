class CinemaHall2 : CinemaHall
{
    public CinemaHall2(int id) : base(id, 300)
    {
        AddChairs("Standard", 150);
        AddChairs("ExtraLegroom", 90);
        AddChairs("LoveSeat", 60);
    }
}
// derived from Cinemahall, determines what the arrangement of the seat types will be and the max amount of seats
// keep in mind that the arrangement is a placeholder, we do not yet know what the actual arrangement will be