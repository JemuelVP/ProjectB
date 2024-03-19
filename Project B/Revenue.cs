class Revenue
{
    public double TotalRevenue(List<Chair> chairs)
    {
        double totalRevenue = 0.0;
        foreach (Chair chair in chairs)
        {
            if (chair.Sold)
            {
                totalRevenue += chair.Price; 
            }
        }    
        return totalRevenue;
    }
}
//