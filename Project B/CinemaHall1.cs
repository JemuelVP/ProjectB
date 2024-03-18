class CinemaHall1 : CinemaHall
{
    public CinemaHall1(int id) : base(id, 150)
    {
        FillChairs(id, "name", 20.0);
    }
}
// derived from Cinemahall, here the list gets filled with chairs throug method fillChairs(), until the max seats 
// are reached.
// added placeholder parameters to FillChairs()