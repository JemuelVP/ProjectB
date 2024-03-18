class CinemaHall2 : CinemaHall 
{
    public CinemaHall2(int id) : base(id, 300)
    {
        FillChairs();
    }
}
// derived from Cinemahall, here the list gets filled with chairs throug method fillChairs(), until the max seats 
// are reached.