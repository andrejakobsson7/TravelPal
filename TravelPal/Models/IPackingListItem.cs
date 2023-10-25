namespace TravelPal.Models
{
    public interface IPackingListItem
    {
        public string Name { get; set; }

        public string GetInfo();
    }
}
