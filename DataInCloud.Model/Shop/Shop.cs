﻿namespace DataInCloud.Model.Shop
{
    public class Shop
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public int PlacesAmount { get; set; }
    }
}
