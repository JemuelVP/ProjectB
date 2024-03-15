class Revenue
{
    public List<Chair> SoldTickets;

    public Revenue(List<Chair> soldTickets)
    {
        SoldTickets = soldTickets;
    }

    public double TotalRevenue()
    {
        totalRevenue = 0.0;

        foreach (Chair chair in SoldTickets)
        {
            totalRevenue += chair.Price; 
        }
        
        return totalRevenue;
    }
}