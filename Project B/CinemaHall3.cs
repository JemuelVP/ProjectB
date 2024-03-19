class CinemaHall3 : CinemaHall 
{
    public CinemaHall3(int id) : base(id, 500)
    {
        FillChairs();
    }
}
// derived from Cinemahall, here the list gets filled with chairs throug method fillChairs(), until the max seats 
// are reached.