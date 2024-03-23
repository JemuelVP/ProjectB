class CinemaHall3 : CinemaHall
{
    public CinemaHall3(int id) : base(id, 500)
    {
        AddChairs("Standard", 250);
        AddChairs("ExtraLegroom", 150);
        AddChairs("LoveSeat", 100);
    }
}
// derived from Cinemahall, determines what the arrangement of the seat types will be and the max amount of seats
// keep in mind that the arrangement is a placeholder, we do not yet know what the actual arrangement will be