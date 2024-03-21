class CinemaHall1 : CinemaHall
{
    public CinemaHall1(int id) : base(id, 150)
    {
        AddChairs("Standard", 100);
        AddChairs("ExtraLegroom", 30);
        AddChairs("LoveSeat", 20);
    }
}
// derived from Cinemahall, determines what the arrangement of the seat types will be and the max amount of seats
// keep in mind that the arrangement is a placeholder, we do not yet know what the actual arrangement will be