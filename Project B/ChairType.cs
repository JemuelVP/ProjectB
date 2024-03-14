class ChairType
{
    public int Id;
    public string Name;
    public double Price;

    public ChairType(int id, string name,double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }


    //manier 1 

    public int LoveSeat = 0;
    public int ExtraLegRoomSeat = 0;
    public int NormalSeat = 0;
    public void CheckChairtype(ChairType chairtype)
    {
        if (chairtype.Name.ToLower() == "Loveseat")
        {   
            LoveSeat++;
        }
        else if (chairtype.Name.ToLower() == "ExtraLegRoomSeat")
        {
            ExtraLegRoomSeat++;
        }
        else if (chairtype.Name.ToLower() == "NormalSeat")
        {
            NormalSeat++;
        }
    }

    // manier 2
    public List<ChairType> listOfLoveseats = new List<ChairType>();
    public List<ChairType> listOfExtraLegRoomSeats = new List<ChairType>();
    public List<ChairType> listOfNormalSeats = new List<ChairType>();

    public void CheckChair(ChairType chairtype)
    {
        if (chairtype.Name.ToLower() == "Loveseat")
        {   
            listOfLoveseats.Add(chairtype);
        }
        else if (chairtype.Name.ToLower() == "ExtraLegRoomSeat")
        {
            listOfExtraLegRoomSeats.Add(chairtype);
        }
        else if (chairtype.Name.ToLower() == "NormalSeat")
        {
            listOfNormalSeats.Add(chairtype);
        }
    }
}

